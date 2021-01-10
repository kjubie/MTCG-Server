using System.Collections.Generic;


namespace MTCG_Server {
    public class SpellCard : Card {
        public SpellCard(string name, ElementType type, int damage, Dictionary<string, Effect> effects) {
            this.name = name;
            this.type = type;
            this.damage = damage;
            this.effects = effects;
        }
    }
}
