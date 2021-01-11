using System;
using System.Collections.Generic;
using System.Linq;

namespace MTCG_Server {
    public class User {
        public string name { get; set; } //Name of the User

        private string password; //Password of the User
        public int credits { get; set; } //Credits of the User
        public int elo { get; set; } //Elo of the User

        Deck deck; //Deck of the User
        Deck battleDeck; //Temporay Deck for in Battle stuff
        Stack collection; //Card collection of the User

        public Card[] handcards = new Card[2]; //Handcards in Battle
        public int pick = -1;

        string token; //AuthToken of the User
        public int inBattle { get; set; }
        public int online { get; set; }

        public int wins = 0;
        public int losses = 0;

        public User(string name, string password) {
            this.name = name;
            this.password = password;
            credits = 10;
            elo = 100;
            deck = new Deck();
            collection = new Stack();
            token = name + "-mtcgToken";
            online = 0;
        }

        public User(string name, string password, int credits, int elo, Stack collection, Deck deck) {
            this.name = name;
            this.password = password;
            this.credits = credits;
            this.elo = elo;
            this.deck = deck;
            this.collection = collection;
            token = name + "-mtcgToken";
            online = 0;
        }

        public void AddCardToCollection(Card card) {
            collection.AddCard(card);
        }

        public int AddCardToDeck(Card card) {
            return deck.AddCard(card);
        }

        public int RemoveCardFromDeck(Card card) {
            return deck.RemoveCard(card.name);
        }

        public string StackToString() {
            string cards = "";

            Dictionary<string, CardInStack> stack;
            collection.GetCards(out stack);

            foreach (var card in stack.Values) {
                cards += "Name: " + card.card.name + " Damage: " + card.card.damage + " Type: " + card.card.type.name + " Effects: ";
                foreach (var effect in card.card.effects.Values)
                    cards += effect.name + " ";
                cards += "\n\n";
            }

            return cards;
        }

        public string DeckToString() {
            string cards = "";

            Dictionary<string, Card> deck;
            this.deck.GetCards(out deck);

            foreach (var card in deck.Values) {
                cards += "Name: " + card.name + " Damage: " + card.damage + " Type: " + card.type.name + " Effects: ";
                foreach (var effect in card.effects.Values)
                    cards += effect.name + " ";
                cards += "\n\n";
            }

            return cards;
        }

        public string GetToken() {
            return token;
        }

        public void SetToken(string token) {
            this.token = token;
        }

        public ref Deck GetDeck() {
            return ref deck;
        }

        public ref Stack GetStack() {
            return ref collection;
        }

        public ref Deck GetBattleDeck() {
            return ref battleDeck;
        }

        public void SetBattleDeck(Deck deck) {
            battleDeck = deck;
        }

        public string getPassword() {
            return password;
        }

        public void setPassword(string password) {
            this.password = password;
        }

        public int UpdateDeck(string msg) {
            string[] names = msg.Split(',');

            Dictionary<string, CardInStack> stack;
            collection.GetCards(out stack);

            Deck tempDeck = new Deck();

            foreach (var name in names) {
                if (stack.ContainsKey(name)) {
                    CardInStack card;
                    stack.TryGetValue(name, out card);
                    tempDeck.AddCard(card.card);
                } else
                    return -1;
            }

            deck = tempDeck;

            return 0;
        }

        /*
         * Buys four random new Cards for the User
         * 
         * @out:
         *      - cardsString: Cards the User got
         */
        public int BuyPack(Dictionary<string, Card> cards, out string cardsString) {
            cardsString = "";

            if (credits < 2)
                return -1;

            credits -= 2;

            cardsString = "You got: \n\n";

            Dictionary<string, CardInStack> collectedCards;
            collection.GetCards(out collectedCards);

            for (int i = 0; i < 4; ++i) {
                Random r = new Random();
                int rCard = r.Next(0, cards.Count);
                Card card = cards.ElementAt(rCard).Value;

                if (collectedCards.ContainsKey(card.name)) {
                    --i;
                } else {
                    collection.AddCard(card);
                    cardsString += "Name: " + card.name + " Damage: " + card.damage + " Type: " + card.type.name + " Effects: ";
                    foreach (var effect in card.effects.Values)
                        cardsString += effect.name + " ";
                    cardsString += "\n\n";
                }
            }

            return 0;
        }

        public string ShowStats() {
            string stats = "\n Stats:";

            stats += "\n Username: " + name;
            stats += "\n Credits: " + credits;
            stats += "\n Elo: " + elo;
            stats += "\n Today Games: " + wins + " wins, " + losses + " losses";

            return stats;
        }
    }
}