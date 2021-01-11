namespace MTCG_Server {
    public class Goblin : Race {
        public Goblin() {
            race = "goblin";
            text = "Goblins are too afraid of Dragons to attack them.";
        }
        public override void DoRaceEffect(ref Card opposingCard, out int calcDamage) {
            calcDamage = opposingCard.damage;
        }
    }
}
