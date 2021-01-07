using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class Elve : Race {
        public Elve() {
            race = "elve";
            text = "Elves can dodge the attacks of dragons.";
        }
        public override void DoRaceEffect(ref Card opposingCard, out int calcDamage) {
            if (typeof(MonsterCard).Equals(opposingCard.GetType())) {
                MonsterCard opCard = (MonsterCard)opposingCard;
                if (opCard.race.race.Equals("dragon"))
                    calcDamage = 0;
                else
                    calcDamage = opposingCard.damage;
            } else {
                calcDamage = opposingCard.damage;
            }
        }
    }
}
