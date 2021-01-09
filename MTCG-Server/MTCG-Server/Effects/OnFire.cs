using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    public class OnFire : Effect {
        public OnFire() {
            name = "OnFire";
            text = "After Combat, I permanently loose 10 attack, if my ElementType is fire I gain 10 attack instead.";
        }

        public OnFire(string name, string text) {
            this.name = name;
            this.text = text;
        }

        public override void DoBeforeEffect(ref Card opposingCard, ref Card me) {
            return;
        }

        public override void DoAfterEffect(ref Card opposingCard, int opposingCardCalcDamage, ref Card me, int meCalcDamage, out int damageModifier) {
            damageModifier = 0;

            if (me.type.name.Equals("fire"))
                me.damage += 15;
            else
                me.damage -= 15;
        }
    }
}
