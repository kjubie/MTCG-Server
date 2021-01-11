namespace MTCG_Server {
    public class Kraken : Race {
        public Kraken() {
            race = "kraken";
            text = "Krakens dont know what spells are.";
        }
        public override void DoRaceEffect(ref Card opposingCard, out int calcDamage) {
            if (typeof(SpellCard).Equals(opposingCard.GetType()))
                calcDamage = 0;
            else
                calcDamage = opposingCard.damage;
        }
    }
}
