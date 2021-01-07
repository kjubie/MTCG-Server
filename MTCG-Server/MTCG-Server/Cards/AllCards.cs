using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server.Cards {
    class AllCards {
        public Dictionary<string, Card> cards;

        public AllCards(Dictionary<string, Card> cards) {
            this.cards = cards;
        }
    }
}
