using System;
using System.Collections.Generic;

namespace MTCG_Server {
    public class Battle {
        User user_a;
        User user_b;

        int overDamage_user_a;
        int overDamage_user_b;

        public int battleInProgress = 1;

        public string roundEndText = "";

        public Battle(ref User user_a, ref User user_b) {
            this.user_a = user_a;
            this.user_b = user_b;
        }

        /*
         * Set Battle Deck to current Deck
         */
        public void StartBattle() {
            user_a.inBattle = 1;
            user_b.inBattle = 1;

            user_a.SetBattleDeck(user_a.GetDeck());
            user_b.SetBattleDeck(user_b.GetDeck());
        }

        public void Round() {
            DrawCards();
        }

        /*
         * Draws upto two random selected Cards for each User
         * 
         * Calls EndBattle() if one User has no Cards left to Draw 
         */
        public void DrawCards() {
            user_a.handcards[0] = null;
            user_a.handcards[1] = null;

            user_b.handcards[0] = null;
            user_b.handcards[1] = null;

            Console.WriteLine("New Round!");
            Random r = new Random();

            int user_a_deckCount = user_a.GetBattleDeck().Count();
            int user_a_handCount = 0;

            Console.WriteLine("User A draws!");

            if (user_a_deckCount > 1) {
                while (user_a_handCount < 2) {
                    int rCard = r.Next(0, user_a_deckCount - user_a_handCount);
                    user_a.handcards[user_a_handCount] = user_a.GetBattleDeck().GetCard(rCard);
                    user_a.GetBattleDeck().RemoveCard(user_a.handcards[user_a_handCount].name);
                    ++user_a_handCount;
                }
            } else if (user_a_deckCount == 1) {
                user_a.handcards[0] = user_a.GetBattleDeck().GetCard(0);
                user_a.GetBattleDeck().RemoveCard(user_a.handcards[0].name);
            } else {
                EndBattle(0, ref user_b, ref user_a);
                return;
            }

            int user_b_deckCount = user_b.GetBattleDeck().Count();
            int user_b_handCount = 0;

            Console.WriteLine("User B draws!");

            if (user_b_deckCount > 1) {
                while (user_b_handCount < 2) {
                    int rCard = r.Next(0, user_b_deckCount - user_b_handCount);
                    user_b.handcards[user_b_handCount] = user_b.GetBattleDeck().GetCard(rCard);
                    user_b.GetBattleDeck().RemoveCard(user_b.handcards[user_b_handCount].name);
                    ++user_b_handCount;
                }
            } else if (user_b_deckCount == 1) {
                user_b.handcards[0] = user_b.GetBattleDeck().GetCard(0);
                user_b.GetBattleDeck().RemoveCard(user_b.handcards[0].name);
            } else {
                EndBattle(0, ref user_a, ref user_b);
                return;
            }

            Console.WriteLine("Users have drawn!");
        }

        /*
         * Return the not chosen card to the Deck and sends the chosen ones into Battle
         * 
         * @params:
         *      - user_a_chosenCard: Index of the Card that User A chose
         *      - user_b_chosenCard: Index of the Card that User B chose
         */
        public void ChooseCard(int user_a_chosenCard, int user_b_chosenCard) {
            for (int i = 0; i < 2; ++i) {
                if (user_a_chosenCard != i && user_b.handcards[i] != null)
                    ReturnCardToDeck(user_a.handcards[i], ref user_a);
                if (user_b_chosenCard != i && user_b.handcards[i] != null)
                    ReturnCardToDeck(user_b.handcards[i], ref user_b);
            }

            MatchCards(user_a.handcards[user_a_chosenCard], user_b.handcards[user_b_chosenCard]);
        }

        /*
         * Evaluates the Damage of each Card with their Types, Effects, Races, etc taken into consideration.
         * 
         * @params:
         *      - user_a_card: Card from User A
         *      - user_b_card: Card from User B
         */
        public void MatchCards(Card user_a_card, Card user_b_card) {
            int user_a_calcDamage;
            int user_b_calcDamage;

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
                user_a_calcDamage = user_a_card.damage;
                user_b_calcDamage = user_b_card.damage;

                if (user_a_card.type.name.Equals(user_b_card.type.weak)) {
                    user_a_calcDamage *= 2;
                    user_b_calcDamage /= 2;
                } else if (user_b_card.type.name.Equals(user_a_card.type.weak)) {
                    user_b_calcDamage *= 2;
                    user_a_calcDamage /= 2;
                }
            }

            int user_a_calcDamageOver = user_a_calcDamage + overDamage_user_a; //Overdamage is the Damage bonus from last Round e.g.: Overwehlm of Buff 
            int user_b_calcDamageOver = user_b_calcDamage + overDamage_user_b;

            foreach (KeyValuePair<string, Effect> effect in user_a_card.effects)
                effect.Value.DoAfterEffect(ref user_b_card, user_b_calcDamage, ref user_a_card, user_a_calcDamage, out overDamage_user_a);

            foreach (KeyValuePair<string, Effect> effect in user_b_card.effects)
                effect.Value.DoAfterEffect(ref user_a_card, user_a_calcDamage, ref user_b_card, user_b_calcDamage, out overDamage_user_b);

            DoDamage(ref user_a_card, user_a_calcDamageOver, ref user_b_card, user_b_calcDamageOver);
        }

        /*
         * Checks which Card has the higher calculated Damage
         * 
         * The User with the stronger Card wins the Round and gets both Cards into his Deck
         * 
         * @params:
         *      - user_a_card: Card from User A
         *      - user_a_damge: Calculated Damage from User A  
         *      - user_b_card: Card from User B
         *      - user_b_damge: Calculated Damage from User B
         */
        public void DoDamage(ref Card user_a_card, int user_a_damage, ref Card user_b_card, int user_b_damage) {
            Console.WriteLine(user_a_damage + " A " + user_b_damage + " B");

            if (user_a_damage > user_b_damage) {
                if (!user_a_card.effects.ContainsKey("Undead"))
                    ReturnCardToDeck(user_a_card, ref user_a);

                if (!user_b_card.effects.ContainsKey("Undead"))
                    ReturnCardToDeck(user_b_card, ref user_a);

                roundEndText = user_a.name + " is the Winner of this Round!";
                Console.WriteLine(user_a.name + " is the Winner of this Round!");
            } else if (user_a_damage < user_b_damage) {
                if (!user_a_card.effects.ContainsKey("Undead"))
                    ReturnCardToDeck(user_a_card, ref user_b);

                if (!user_b_card.effects.ContainsKey("Undead"))
                    ReturnCardToDeck(user_b_card, ref user_b);
                
                roundEndText = user_b.name + " is the Winner of this Round!";
                Console.WriteLine(user_b.name + " is the Winner of this Round!");
            } else {
                if (!user_a_card.effects.ContainsKey("Undead"))
                    ReturnCardToDeck(user_a_card, ref user_a);

                if (!user_b_card.effects.ContainsKey("Undead"))
                    ReturnCardToDeck(user_b_card, ref user_b);

                roundEndText = "Draw!";
                Console.WriteLine("Draw!");
            }
        }

        /*
         * Places the given Card into the Deck of a given User
         * 
         * @params:
         *      - card: Card to return
         *      - user: User to return Card to
         */
        public void ReturnCardToDeck(Card card, ref User user) {
            try {
                user.GetBattleDeck().AddCard(card);
            } catch {
                for (int i = 0; i < 10; ++i) {
                    try {
                        card.name = card.name + i;
                        user.GetBattleDeck().AddCard(card);
                        break;
                    } catch {

                    }
                }
            }
        }

        /*
         * Ends the Battle and calculates the Elo, etc
         * 
         * @params:
         *      - draw: 1 if battle was a draw, 0 if not
         *      - winner: User that won
         *      - loser: User that lost
         */
        public void EndBattle(int draw, ref User winner, ref User loser) {
            if (draw == 1) {
                battleInProgress = 0;
                roundEndText = "Battle Draw!";
                Console.WriteLine("Draw!");
            } else {
                battleInProgress = 0;

                roundEndText = winner.name + " is the Winner of this Battle! " + winner.name + ": +5 Elo " + loser.name + ": -10 Elo";
                Console.WriteLine(winner.name + " is the Winner of this Battle!");
                
                winner.elo += 5;
                winner.wins += 1;
                winner.credits += 1;

                loser.elo -= 10;
                loser.losses += 1;

                user_a.SetBattleDeck(null);
                user_b.SetBattleDeck(null);

                user_a.inBattle = 0;
                user_b.inBattle = 0;

                battleInProgress = 0;
            }
        }

        /*
         * I think this is self explanatory
         */
        public string HandCardsToString(string username) {
            string cards = "";

            if (username.Equals(user_a.name)) {
                foreach (var card in user_a.handcards) {
                    if (card != null) {
                        cards += "Name: " + card.name + " Damage: " + card.damage + " Type: " + card.type.name + " Effects: ";
                        foreach (var effect in card.effects.Values)
                            cards += effect.name + " ";
                        cards += "\n\n";
                    }
                }
            } else {
                foreach (var card in user_b.handcards) {
                    if (card != null) {
                        cards += "Name: " + card.name + " Damage: " + card.damage + " Type: " + card.type.name + " Effects: ";
                        foreach (var effect in card.effects.Values)
                            cards += effect.name + " ";
                        cards += "\n\n";
                    }
                }
            }

            return cards;
        }
    }
}