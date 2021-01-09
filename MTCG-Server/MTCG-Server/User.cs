using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    public class User {
        public string name { get; set; }

        private string password;
        public int credits { get; set; }
        public int elo { get; set; }

        Deck deck;
        Deck battleDeck;
        Stack collection;
        Stack notCollectedCards;

        UserConnection uc;

        string token;
        public int inBattle { get; set; }


        public User(string name) {
            this.name = name;
            credits = 10;
            elo = 100;
            deck = new Deck();
            collection = new Stack();
            notCollectedCards = new Stack();
        }

        public User(string name, string password, int credits, int elo, Stack collection, Deck deck) {
            this.name = name;
            this.password = password;
            this.credits = credits;
            this.elo = elo;
            this.deck = deck;
            this.collection = collection;
        }

        public void AddCardToCollection(Card card) {
            collection.AddCard(card);
            notCollectedCards.RemoveCard(card.name);
        }

        public int AddCardToDeck(Card card) {
            return deck.AddCard(card);
        }

        public int RemoveCardFromDeck(Card card) {
            return deck.RemoveCard(card.name);
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
    }
}