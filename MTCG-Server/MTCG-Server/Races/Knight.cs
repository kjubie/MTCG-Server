using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    public class Knight : Race {
        public Knight() {
            race = "knight";
            text = "Knights instantly drown in water spells.";
        }
        public override void DoRaceEffect(ref Card opposingCard, out int calcDamage) {
            if (typeof(SpellCard).Equals(opposingCard.GetType()))
                if (opposingCard.type.Equals("water"))
                    calcDamage = 9999;
                else
                    calcDamage = opposingCard.damage;
            else
                calcDamage = opposingCard.damage;
        }
    }
}
