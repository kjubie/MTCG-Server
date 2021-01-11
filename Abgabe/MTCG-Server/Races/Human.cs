namespace MTCG_Server {
    public class Human : Race {
        public Human() {
            race = "human";
            text = "";
        }
        public override void DoRaceEffect(ref Card opposingCard, out int calcDamage) {
            calcDamage = opposingCard.damage;
        }
    }
}
