namespace MTCG_Server {
    public class Ork : Race {
        public Ork() {
            race = "ork";
            text = "";
        }
        public override void DoRaceEffect(ref Card opposingCard, out int calcDamage) {
            calcDamage = opposingCard.damage;
        }
    }
}
