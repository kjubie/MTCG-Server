using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    public abstract class Card {
        public string name { get; set; }
        public int damage { get; set; }
        public ElementType type { get; set; }

        public Dictionary<string, Effect> effects;
        public Card() {
            ;
        }
    }
}