using System;
using System.Collections.Generic;

namespace MTCG_Server.Battle {
    public class Battle {
        User user_a;
        Card[] user_a_handcards = new Card[2];

        User user_b;
        Card[] user_b_handcards = new Card[2];

        int roundCount;
        int overDamage_user_a;
        int overDamage_user_b;

        public Battle(ref User user_a, ref User user_b) {
            this.user_a = user_a;
            this.user_b = user_b;
            roundCount = 0;
        }

        public void StartBattle() {
            user_a.inBattle = 0;
            user_b.inBattle = 0;

            user_a.SetBattleDeck(user_a.GetDeck());
            user_b.SetBattleDeck(user_b.GetDeck());
        }

        public void Round() {
            if (roundCount > 100)
                EndBattle(1, ref user_a, ref user_b);
            ++roundCount;
            DrawCards();
        }

        public void DrawCards() {
            Random r = new Random();

            int user_a_deckCount = user_a.GetBattleDeck().Count();
            int user_a_handCount = 0;

            if (user_a_deckCount > 1)
                while (user_a_handCount < 2) {
                    int rCard = r.Next(0, user_a_deckCount - user_a_handCount);
                    user_a_handcards[user_a_handCount] = user_a.GetBattleDeck().GetCard(rCard);
                    user_a.GetBattleDeck().RemoveCard(user_a_handcards[user_a_handCount].name);
                    ++user_a_handCount;
                }
            else if (user_a_deckCount == 1) {
                user_a_handcards[0] = user_a.GetBattleDeck().GetCard(0);
                user_a.GetBattleDeck().RemoveCard(user_a_handcards[0].name);
            } else {
                EndBattle(0, ref user_b, ref user_a);
            }

            int user_b_deckCount = user_b.GetBattleDeck().Count();
            int user_b_handCount = 0;

            if (user_b_deckCount > 1)
                while (user_b_handCount < 2) {
                    int rCard = r.Next(0, user_b_deckCount - user_b_handCount);
                    user_b_handcards[user_b_handCount] = user_b.GetBattleDeck().GetCard(rCard);
                    user_b.GetBattleDeck().RemoveCard(user_b_handcards[user_a_handCount].name);
                    ++user_b_handCount;
                }
            else if (user_b_deckCount == 1) {
                user_b_handcards[0] = user_b.GetBattleDeck().GetCard(0);
                user_b.GetBattleDeck().RemoveCard(user_b_handcards[0].name);
            } else {
                EndBattle(0, ref user_a, ref user_b);
            }
        }

        public void ChooseCard(int user_a_chosenCard, int user_b_chosenCard) {
            for (int i = 0; i < 2; ++i) {
                if (user_a_chosenCard != i)
                    ReturnCardToDeck(user_a_handcards[i], ref user_a);
                if (user_b_chosenCard != i)
                    ReturnCardToDeck(user_b_handcards[i], ref user_b);
            }

            MatchCards(user_a_handcards[user_a_chosenCard], user_b_handcards[user_b_chosenCard]);
        }

        public void MatchCards(Card user_a_card, Card user_b_card) {
            int user_a_calcDamage = 0;
            int user_b_calcDamage = 0;

            if (user_a_card.effects.ContainsKey("Silence") && user_b_card.effects.ContainsKey("Silence")) {
                user_a_card.effects.TryGetValue("Silence", out Effect e_user_a);
                user_a_card.effects.TryGetValue("Silence", out Effect e_user_b);

                e_user_a.DoBeforeEffect(ref user_b_card, ref user_a_card);
                e_user_b.DoBeforeEffect(ref user_a_card, ref user_b_card);
            } else if (user_a_card.effects.ContainsKey("Silence")) {
                foreach (KeyValuePair<string, Effect> effect in user_a_card.effects)
                    effect.Value.DoBeforeEffect(ref user_b_card, ref user_a_card);
            } else if (user_a_card.effects.ContainsKey("Silence")) {
                foreach (KeyValuePair<string, Effect> effect in user_b_card.effects)
                    effect.Value.DoBeforeEffect(ref user_a_card, ref user_b_card);
            } else {
                foreach (KeyValuePair<string, Effect> effect in user_a_card.effects)
                    effect.Value.DoBeforeEffect(ref user_b_card, ref user_a_card);

                foreach (KeyValuePair<string, Effect> effect in user_b_card.effects)
                    effect.Value.DoBeforeEffect(ref user_a_card, ref user_b_card);
            }

            if (typeof(MonsterCard).Equals(user_a_card.GetType()) && typeof(MonsterCard).Equals(user_b_card.GetType())) {
                MonsterCard user_a_cardM = (MonsterCard)user_a_card;
                user_a_cardM.race.DoRaceEffect(ref user_b_card, out user_b_calcDamage);

                MonsterCard user_b_cardM = (MonsterCard)user_b_card;
                user_b_cardM.race.DoRaceEffect(ref user_a_card, out user_a_calcDamage);
            } else {
                if (user_a_card.type.name.Equals(user_b_card.type.weak)) {
                    user_a_calcDamage *= 2;
                    user_b_calcDamage /= 2;
                } else if (user_b_card.type.name.Equals(user_a_card.type.weak)) {
                    user_b_calcDamage *= 2;
                    user_a_calcDamage /= 2;
                }
            }

            foreach (KeyValuePair<string, Effect> effect in user_a_card.effects)
                effect.Value.DoAfterEffect(ref user_b_card, user_b_calcDamage, ref user_a_card, user_a_calcDamage, out overDamage_user_a);

            foreach (KeyValuePair<string, Effect> effect in user_a_card.effects)
                effect.Value.DoAfterEffect(ref user_a_card, user_a_calcDamage, ref user_a_card, user_b_calcDamage, out overDamage_user_b);

            DoDamage(ref user_a_card, user_a_calcDamage + overDamage_user_a, ref user_b_card, user_b_calcDamage + overDamage_user_b);
        }

        public void DoDamage(ref Card user_a_card, int user_a_damage, ref Card user_b_card, int user_b_damage) {
            if (user_a_damage > user_b_damage) {
                if (!user_a_card.effects.ContainsKey("Undead"))
                    ReturnCardToDeck(user_a_card, ref user_a);

                if (!user_b_card.effects.ContainsKey("Undead"))
                    ReturnCardToDeck(user_b_card, ref user_a);

                Console.WriteLine(user_a.name + " is the Winner of this Round!");
            } else if (user_a_damage < user_b_damage) {
                if (!user_a_card.effects.ContainsKey("Undead"))
                    ReturnCardToDeck(user_a_card, ref user_b);

                if (!user_b_card.effects.ContainsKey("Undead"))
                    ReturnCardToDeck(user_b_card, ref user_b);

                Console.WriteLine(user_b.name + " is the Winner of this Round!");
            } else {
                if (!user_a_card.effects.ContainsKey("Undead"))
                    ReturnCardToDeck(user_a_card, ref user_a);

                if (!user_b_card.effects.ContainsKey("Undead"))
                    ReturnCardToDeck(user_b_card, ref user_b);

                Console.WriteLine("Draw!");
            }
        }

        public void ReturnCardToDeck(Card card, ref User user) {
            user.GetBattleDeck().AddCard(card);
        }

        public void EndBattle(int draw, ref User winner, ref User loser) {
            if (draw == 1)
                Console.WriteLine("Draw!");
            else {
                Console.WriteLine(winner.name + " is the Winner of this Battle!");
                
                winner.elo += 5;
                loser.elo -= 10;

                user_a.SetBattleDeck(null);
                user_b.SetBattleDeck(null);

                user_a.inBattle = 0;
                user_b.inBattle = 0;
            }
        }
    }
}