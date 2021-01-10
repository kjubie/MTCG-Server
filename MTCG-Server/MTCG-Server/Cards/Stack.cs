using System.Collections.Generic;

namespace MTCG_Server {
    public class Stack {
        private Dictionary<string, CardInStack> cards; //Cards in the Stack

        public Stack() {
            cards = new Dictionary<string, CardInStack>();
        }

        public Stack(Dictionary<string, CardInStack> cards) {
            this.cards = cards;
        }

        public void AddCard(Card card) {
            cards.Add(card.name, new CardInStack(card));
        }

        public void RemoveCard(string name) {
            cards.Remove(name);
        }

        public void GetCards(out Dictionary<string, CardInStack> cards) {
            cards = this.cards;
        }
    }
}
