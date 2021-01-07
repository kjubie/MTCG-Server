using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class Ork : Race {
        public Ork() {
            race = "ork";
            text = "";
        }
        public override void DoRaceEffect(ref Card opposingCard, out int calcDamage) {
            calcDamage = opposingCard.damage;
        }
    }
}
