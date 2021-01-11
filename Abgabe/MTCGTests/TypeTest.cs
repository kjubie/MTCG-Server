using MTCG_Server;
using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_ServerTest {
    public class TypeTest {
        Types t;


        [SetUp]
        public void Setup() {
            t = new Types();
            
        }

        [Test]
        public void TestFire() {
            ElementType type;
            t.types.TryGetValue("fire", out type);

            Assert.AreEqual("fire", type.name);
            Assert.AreEqual("grass", type.strong);
            Assert.AreEqual("water", type.weak);
        }

        [Test]
        public void TestWater() {
            ElementType type;
            t.types.TryGetValue("water", out type);

            Assert.AreEqual("water", type.name);
            Assert.AreEqual("fire", type.strong);
            Assert.AreEqual("grass", type.weak);
        }

        [Test]
        public void TestGrass() {
            ElementType type;
            t.types.TryGetValue("grass", out type);

            Assert.AreEqual("grass", type.name);
            Assert.AreEqual("water", type.strong);
            Assert.AreEqual("fire", type.weak);
        }

        [Test]
        public void TestNormal() {
            ElementType type;
            t.types.TryGetValue("normal", out type);

            Assert.AreEqual("normal", type.name);
            Assert.AreEqual("-", type.strong);
            Assert.AreEqual("-", type.weak);
        }

        [TearDown]
        public void TearDown() {
            
        }
    }
}