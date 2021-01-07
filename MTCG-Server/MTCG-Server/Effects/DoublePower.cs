using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class DoublePower : Effect {
        public DoublePower() {
            name = "DoublePower";
            text = "When I kill another monster, double my power for the rest of the game.";
        }

        public DoublePower(string name, string text) {
            this.name = name;
            this.text = text;
        }

        public override void DoBeforeEffect(ref Card opposingCard, ref Card me) {
            return;
        }

        public override void DoAfterEffect(ref Card opposingCard, int opposingCardCalcDamage, ref Card me, int meCalcDamage, out int damageModifier) {
            damageModifier = 0;
            
            if(meCalcDamage > opposingCardCalcDamage)
                me.damage *= 2;

        }
    }
}
