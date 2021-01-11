namespace MTCG_Server {
    public class NegateType : Effect {
        public NegateType() {
            name = "NegateType";
            text = "Set the targets type to normal for the rest of the game.";
        }

        public NegateType(string name, string text) {
            this.name = name;
            this.text = text;
        }

        public override void DoBeforeEffect(ref Card opposingCard, ref Card me) {
            Types types = new Types();
            ElementType t;

            types.types.TryGetValue("normal", out t);
            opposingCard.type = t;
        }

        public override void DoAfterEffect(ref Card opposingCard, int opposingCardCalcDamage, ref Card me, int meCalcDamage, out int damageModifier) {
            damageModifier = 0;
        }
    }
}
