using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class Goblin : Race {
        public Goblin() {
            race = "goblin";
            text = "Goblins are too afraid of Dragons to attack them.";
        }
        public override void DoRaceEffect(ref Card opposingCard, out int calcDamage) {
            calcDamage = opposingCard.damage;
        }
    }
}
