using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class Deck {
        Dictionary<string, Card> cards;

        public Deck() {
            cards = new Dictionary<string, Card>();
        }
        
        public Deck(Dictionary<string, Card> cards) {
            this.cards = cards;
        }

        public Card GetCard(string name) {
            Card card;
            bool boo = cards.TryGetValue(name, out card);

            if (boo)
                return card;
            else
                return null;
        }

        public Card GetCard(int id) {
            return cards.ElementAt(id).Value;
        }

        public int AddCard(Card card) {
            if (cards.Count < 4) {
                cards.Add(card.name, card);
                return 0;
            } else
                return -1;
        }

        public int RemoveCard(string name) {
            if (cards.Remove(name)) {
                return 0;
            } else
                return -1;
        }

        public int Count() {
            return cards.Count();
        }
    }
}