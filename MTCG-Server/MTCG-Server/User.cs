using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class User {
        public string name { get; set; }
        public int credits { get; set; }
        public int elo { get; set; }

        Deck deck;
        Deck battleDeck;
        Stack collection;
        Stack notCollectedCards;

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

        public User(string name, int credits, int elo, Stack collection, Stack notCollectedCards, Deck deck) {
            this.name = name;
            this.credits = credits;
            this.elo = elo;
            this.deck = deck;
            this.collection = collection;
            this.notCollectedCards = notCollectedCards;
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

        public ref Deck GetBattleDeck() {
            return ref battleDeck;
        }

        public void SetBattleDeck(Deck deck) {
            battleDeck = deck;
        }
    }
}