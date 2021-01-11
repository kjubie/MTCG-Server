namespace MTCG_Server {
    public class Buff : Effect {
        int buffValue;

        public Buff() {
            buffValue = 15;
            name = "Buff";
            text = "The next Monster you play gets + " + buffValue + " attack.";
        }

        public Buff(int buffValue) {
            this.buffValue = buffValue;
            name = "Buff";
            text = "The next Monster you play gets + " + buffValue + " attack.";
        }

        public Buff(string name, string text, int buffValue) {
            this.name = name;
            this.text = text;
            this.buffValue = buffValue;
        }

        public override void DoBeforeEffect(ref Card opposingCard, ref Card me) {
            return;
        }

        public override void DoAfterEffect(ref Card opposingCard, int opposingCardCalcDamage, ref Card me, int meCalcDamage, out int damageModifier) {
            damageModifier = buffValue;
        }
    }
}
