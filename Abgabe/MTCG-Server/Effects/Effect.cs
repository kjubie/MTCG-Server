namespace MTCG_Server {
    public abstract class Effect {
        public string name { get; set; }
        public string text { get; set; }
        public abstract void DoBeforeEffect(ref Card opposingCard, ref Card me);
        public abstract void DoAfterEffect(ref Card opposingCard, int opposingCardCalcDamage, ref Card me, int meCalcDamage, out int damageModifier);
    }
}
