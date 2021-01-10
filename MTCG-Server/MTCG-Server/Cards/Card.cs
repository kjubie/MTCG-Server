using System.Collections.Generic;

namespace MTCG_Server {
    public abstract class Card {
        public string name { get; set; } //Name of the Card
        public int damage { get; set; } //Damage of the Card
        public ElementType type { get; set; } //Element of the Card

        public Dictionary<string, Effect> effects; //Effects of the Card
        public Card() {
            ;
        }
    }
}