using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    public class CardInStack {
        Card card;
        int isInStore;
        int isInDeck;

        public CardInStack(Card card) {
            this.card = card;
            isInStore = 0;
            isInDeck = 0;
        }

        public CardInStack(Card card, int isInStore, int isInDeck) {
            this.card = card;
            this.isInStore = isInStore;
            this.isInDeck = isInDeck;
        }

        public int AddToStore() {
            if (isInStore == 1)
                return -1;

            if (isInDeck == 1)
                return -2;

            isInStore = 1;
            return 0;
        }

        public int RemoveFromStore() {
            if (isInStore == 1)
                isInStore = 0;
            else
                return -1;
            return 0;
        }

        public int AddToDeck() {
            if (isInDeck == 1)
                return -1;

            if (isInStore == 1)
                return -2;

            isInDeck = 1;
            return 0;
        }

        public int RemoveFromDeck() {
            if (isInDeck == 1)
                isInDeck = 0;
            else
                return -1;
            return 0;
        }

        public Card GetCard() {
            return card;
        }
    }
}
