namespace MTCG_Server {
    public class Spellshield : Effect {
        public Spellshield() {
            name = "Spellshield";
            text = "I only take half the damage from spells.";
        }

        public Spellshield(string name, string text) {
            this.name = name;
            this.text = text;
        }

        public override void DoBeforeEffect(ref Card opposingCard, ref Card me) {
            if (typeof(SpellCard).Equals(opposingCard.GetType())) 
                opposingCard.damage /= 2;
        }

        public override void DoAfterEffect(ref Card opposingCard, int opposingCardCalcDamage, ref Card me, int meCalcDamage, out int damageModifier) {
            damageModifier = 0;
            
            if (typeof(SpellCard).Equals(opposingCard.GetType()))
                opposingCard.damage *= 2;
        }
    }
}
