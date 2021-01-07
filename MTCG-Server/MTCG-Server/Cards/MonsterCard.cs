using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class MonsterCard : Card {
        public Race race;
        public MonsterCard(string name, Type type, int damage, Dictionary<string, Effect> effects, Race race) {
            this.name = name;
            this.type = type;
            this.damage = damage;
            this.effects = effects;
            this.race = race;

        }
    }
}
