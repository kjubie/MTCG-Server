using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    public class Human : Race {
        public Human() {
            race = "human";
            text = "";
        }
        public override void DoRaceEffect(ref Card opposingCard, out int calcDamage) {
            calcDamage = opposingCard.damage;
        }
    }
}
