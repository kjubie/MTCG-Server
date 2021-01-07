using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class Kraken : Race {
        public Kraken() {
            race = "kraken";
            text = "Krakens dont know what spells are.";
        }
        public override void DoRaceEffect(ref Card opposingCard, out int calcDamage) {
            if (typeof(SpellCard).Equals(opposingCard.GetType()))
                calcDamage = 0;
            else
                calcDamage = opposingCard.damage;
            }
        }
    }
}
