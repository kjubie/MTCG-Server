using System.Collections.Generic;

namespace MTCG_Server {
    public class Silence : Effect {
        public Silence() {
            name = "Silence";
            text = "Remove all effects from the target for the rest of the game.";
        }

        public Silence(string name, string text) {
            this.name = name;
            this.text = text;
        }

        public override void DoBeforeEffect(ref Card opposingCard, ref Card me) {
            opposingCard.effects = new Dictionary<string, Effect>();
        }

        public override void DoAfterEffect(ref Card opposingCard, int opposingCardCalcDamage, ref Card me, int meCalcDamage, out int damageModifier) {
            damageModifier = 0;
        }
    }
}
