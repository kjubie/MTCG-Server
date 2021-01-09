using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    public class SetOnFire : Effect {
        public SetOnFire() {
            name = "SetOnFire";
            text = "Grant an enemy Monster On Fire.";
        }
        
        public SetOnFire(string name, string text) {
            this.name = name;
            this.text = text;
        }

        public override void DoBeforeEffect(ref Card opposingCard, ref Card me) {
            return;
        }

        public override void DoAfterEffect(ref Card opposingCard, int opposingCardCalcDamage, ref Card me, int meCalcDamage, out int damageModifier) {
            damageModifier = 0;

            opposingCard.effects.Add("OnFire", new OnFire());
        }
    }
}
