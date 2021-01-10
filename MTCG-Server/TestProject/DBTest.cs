using MTCG_Server;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_ServerTest {
    public class DBTest {
        DBConnector dbcon;
        Dictionary<string, Card> cards;
        Dictionary<string, Card> preselectedCards;
        Dictionary<string, User> users;

        [SetUp]
        public void Setup() {
            dbcon = new DBConnector("localhost", "postgres", "abru13", "mtcg");
            cards = new Dictionary<string, Card>();
            preselectedCards = new Dictionary<string, Card>();
            users = new Dictionary<string, User>();

            dbcon.LoadCards(ref preselectedCards, new Types());
        }

        [Test]
        public void TestLoadCards() {
            dbcon.LoadCards(ref cards, new Types());

            Assert.AreEqual(54, cards.Count);
            Assert.AreEqual(true, cards.ContainsKey("Imperial Fire Archers"));
            
            Card card;
            cards.TryGetValue("Imperial Fire Archers", out card);

            Assert.AreEqual(25, card.damage);
        }

        [Test]
        public void TestLoadUsers() {
            dbcon.LoadUsers(ref users, preselectedCards);

            Assert.AreEqual(2, users.Count);
            Assert.AreEqual(true, users.ContainsKey("kjubie"));

            User user;
            users.TryGetValue("kjubie", out user);

            Assert.AreEqual("123", user.getPassword());

            Dictionary<string, CardInStack> userCards;
            user.GetStack().GetCards(out userCards);

            Assert.AreEqual(9, userCards.Count);
        }

        [Test]
        public void TestSave() {
            
        }

        [TearDown]
        public void TearDown() {
        }
    }
}