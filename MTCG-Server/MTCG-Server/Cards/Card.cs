using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    abstract class Card {
        public string name { get; set; }
        public int damage { get; set; }
        public Type type { get; set; }

        public Dictionary<string, Effect> effects;
        public Card() {
            throw new NotImplementedException();
        }
    }
}