using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class SpellCard : Card {
        public SpellCard(string name, Type type, int damage, Dictionary<string, Effect> effects) {
            this.name = name;
            this.type = type;
            this.damage = damage;
            this.effects = effects;
        }
    }
}
