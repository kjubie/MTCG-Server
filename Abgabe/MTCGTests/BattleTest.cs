using MTCG_Server;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_ServerTest {
    public class BattleTest {
        DBConnector dbcon;
        Dictionary<string, Card> cards;
        Dictionary<string, User> users;
        Battle B;

        User userA;
        User userB;

        [SetUp]
        public void Setup() {
            dbcon = new DBConnector("localhost", "postgres", "abru13", "mtcg");
            cards = new Dictionary<string, Card>();
            users = new Dictionary<string, User>();

            dbcon.LoadCards(ref cards, new Types());
            dbcon.LoadUsers(ref users, cards);

            users.TryGetValue("kjubie", out userA);
            users.TryGetValue("1z3ro37", out userB);

            B = new Battle(ref userA, ref userB);
        }

        [Test]
        public void TestDraw() {
            B.StartBattle();
            B.DrawCards();

            Assert.AreEqual(true, userA.handcards[0] != null);
            Assert.AreEqual(true, userA.handcards[1] != null);
            Assert.AreEqual(true, userB.handcards[0] != null);
            Assert.AreEqual(true, userB.handcards[1] != null);
        }

        [Test]
        public void TestCombatMonster() {
            B.StartBattle();

            Deck dA = new Deck();
            dA.AddCard(new MonsterCard("TestCard", new ElementType("fire", "grass", "water"), 30, new Dictionary<string, Effect>(), new Human()));
            userA.SetBattleDeck(dA);

            Deck dB = new Deck();
            dB.AddCard(new MonsterCard("TestCard", new ElementType("fire", "grass", "water"), 20, new Dictionary<string, Effect>(), new Human()));
            userB.SetBattleDeck(dB);

            Card cA = userA.GetBattleDeck().GetCard("TestCard");
            Card cB = userB.GetBattleDeck().GetCard("TestCard");

            B.MatchCards(cA, cB);

            Dictionary<string, Card> cardsA;
            userA.GetBattleDeck().GetCards(out cardsA);

            Dictionary<string, Card> cardsB;
            userB.GetBattleDeck().GetCards(out cardsB);

            Assert.AreEqual(3, cardsA.Count); //Es sollten eigentlich 2 und 0 Karten sein aber da die Karten beim Draw aus dem Deck entfernt werden und nach der Runde beim Gewinner wieder
            Assert.AreEqual(1, cardsB.Count); //hinzugefüght werden und hier Draw nicht aufgerufen wird, werden die Karten hinzugefüght aber nicht entfernt. Also sind 3 und 1. Wörkt trotzdem so wie es soll. 
        }

        [Test]
        public void TestCombatSpell() {
            B.StartBattle();

            Deck dA = new Deck();
            dA.AddCard(new SpellCard("TestCard", new ElementType("fire", "grass", "water"), 20, new Dictionary<string, Effect>()));
            userA.SetBattleDeck(dA);

            Deck dB = new Deck();
            dB.AddCard(new MonsterCard("TestCard", new ElementType("grass", "water", "fire"), 30, new Dictionary<string, Effect>(), new Human()));
            userB.SetBattleDeck(dB);

            Card cA = userA.GetBattleDeck().GetCard("TestCard");
            Card cB = userB.GetBattleDeck().GetCard("TestCard");

            B.MatchCards(cA, cB);

            Dictionary<string, Card> cardsA;
            userA.GetBattleDeck().GetCards(out cardsA);

            Dictionary<string, Card> cardsB;
            userB.GetBattleDeck().GetCards(out cardsB);

            Assert.AreEqual(3, cardsA.Count); //Es sollten eigentlich 2 und 0 Karten sein aber da die Karten beim Draw aus dem Deck entfernt werden und nach der Runde beim Gewinner wieder
            Assert.AreEqual(1, cardsB.Count); //hinzugefüght werden und hier Draw nicht aufgerufen wird, werden die Karten hinzugefüght aber nicht entfernt. Also sind 3 und 1. Wörkt trotzdem so wie es soll. 
        }

        [Test]
        public void TestSilence() {
            B.StartBattle();

            Silence effectA = new Silence();

            Dictionary<string, Effect> effectsA = new Dictionary<string, Effect>();
            effectsA.Add("Silence", effectA);

            OnFire effectB = new OnFire();

            Dictionary<string, Effect> effectsB = new Dictionary<string, Effect>();
            effectsB.Add("OnFire", effectB);

            Deck dA = new Deck();
            dA.AddCard(new SpellCard("TestSpell", new ElementType("fire", "grass", "water"), 40, effectsA));
            userA.SetBattleDeck(dA);

            Deck dB = new Deck();
            dB.AddCard(new MonsterCard("TestMonster", new ElementType("grass", "water", "fire"), 30, effectsB, new Human()));
            userB.SetBattleDeck(dB);

            Card cA = userA.GetBattleDeck().GetCard("TestSpell");
            Card cB = userB.GetBattleDeck().GetCard("TestMonster");

            userA.SetBattleDeck(new Deck());
            userB.SetBattleDeck(new Deck());

            B.MatchCards(cA, cB);

            Dictionary<string, Card> cardsA;
            userA.GetBattleDeck().GetCards(out cardsA);

            Dictionary<string, Card> cardsB;
            userB.GetBattleDeck().GetCards(out cardsB);

            cardsA.TryGetValue("TestMonster", out Card c);

            Assert.AreEqual(0, c.effects.Count); //Es sollten eigentlich 2 und 0 Karten sein aber da die Karten beim Draw aus dem Deck entfernt werden und nach der Runde beim Gewinner wieder
        }

        [Test]
        public void TestUndead() {
            B.StartBattle();

            Undead effectA = new Undead();

            Dictionary<string, Effect> effectsA = new Dictionary<string, Effect>();
            effectsA.Add("Undead", effectA);

            Deck dA = new Deck();
            dA.AddCard(new MonsterCard("TestUndead", new ElementType("fire", "grass", "water"), 40, effectsA, new Human()));
            userA.SetBattleDeck(dA);

            Deck dB = new Deck();
            dB.AddCard(new MonsterCard("TestMonster", new ElementType("grass", "water", "fire"), 30, new Dictionary<string, Effect>(), new Human()));
            userB.SetBattleDeck(dB);

            Card cA = userA.GetBattleDeck().GetCard("TestUndead");
            Card cB = userB.GetBattleDeck().GetCard("TestMonster");

            userA.SetBattleDeck(new Deck());
            userB.SetBattleDeck(new Deck());

            B.MatchCards(cA, cB);

            Dictionary<string, Card> cardsA;
            userA.GetBattleDeck().GetCards(out cardsA);

            Assert.AreEqual(1, cardsA.Count); //Es sollten eigentlich 2 und 0 Karten sein aber da die Karten beim Draw aus dem Deck entfernt werden und nach der Runde beim Gewinner wieder
        }

        [Test]
        public void TestEnd() {

        }

        [TearDown]
        public void TearDown() {

        }
    }
}