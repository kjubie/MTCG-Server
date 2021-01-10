namespace MTCG_Server {
    public class Dragon : Race {
        public Dragon() {
            race = "dragon";
            text = "";
        }
        public override void DoRaceEffect(ref Card opposingCard, out int calcDamage) {
            if (typeof(MonsterCard).Equals(opposingCard.GetType())) {
                MonsterCard opCard = (MonsterCard)opposingCard;
                if (opCard.race.race.Equals("goblin"))
                    calcDamage = 0;
                else
                    calcDamage = opposingCard.damage;
            } else {
                calcDamage = opposingCard.damage;
            }
        }
    }
}
