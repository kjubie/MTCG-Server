using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class Wizzard : Race {
        public Wizzard() {
            race = "wizzard";
            text = "Wizzards can control Orks, so Orks cant damage them.";
        }
        public override void DoRaceEffect(ref Card opposingCard, out int calcDamage) {
            if (typeof(MonsterCard).Equals(opposingCard.GetType())) {
                MonsterCard opCard = (MonsterCard)opposingCard;
                if (opCard.race.race.Equals("ork"))
                    calcDamage = 0;
                else
                    calcDamage = opposingCard.damage;
            } else {
                calcDamage = opposingCard.damage;
            }
        }
    }
}
