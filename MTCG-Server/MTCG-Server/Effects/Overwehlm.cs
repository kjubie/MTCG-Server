using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    public class Overwehlm : Effect {
        public Overwehlm() {
            name = "Overwehlm";
            text = "If I kill the target monster, add my remaining damage to the monster/spell played next round.";
        }

        public Overwehlm(string name, string text) {
            this.name = name;
            this.text = text;
        }

        public override void DoBeforeEffect(ref Card opposingCard, ref Card me) {
            return;
        }

        public override void DoAfterEffect(ref Card opposingCard, int opposingCardCalcDamage, ref Card me, int meCalcDamage, out int damageModifier) {
            damageModifier = 0;

            if (meCalcDamage > opposingCardCalcDamage)
                damageModifier = meCalcDamage - opposingCardCalcDamage;
        }
    }
}
