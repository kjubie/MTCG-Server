using System.Collections.Generic;

namespace MTCG_Server {
    public class MonsterCard : Card {
        public Race race; //Race of the Card
        public MonsterCard(string name, ElementType type, int damage, Dictionary<string, Effect> effects, Race race) {
            this.name = name;
            this.type = type;
            this.damage = damage;
            this.effects = effects;
            this.race = race;

        }
    }
}
