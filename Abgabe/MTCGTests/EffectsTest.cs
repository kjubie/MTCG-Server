using MTCG_Server;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_ServerTest {
    public class EffectsTest {
        Card opposingCard;
        Card me;

        [SetUp]
        public void Setup() {
            opposingCard = new MonsterCard("TestCard", new ElementType("fire", "grass", "water"), 20, new Dictionary<string, Effect>(), new Human());
        }

        [Test]
        public void TestOnFire() {
            OnFire effect = new OnFire();

            Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
            effects.Add("OnFire", effect);

            me = new MonsterCard("TestCard", new ElementType("fire", "grass", "water"), 20, effects, new Human());
            effect.DoAfterEffect(ref opposingCard, 0, ref me, 0, out int dm);
            Assert.AreEqual(35, me.damage);

            me = new MonsterCard("TestCard", new ElementType("normal", "-", "-"), 20, effects, new Human());
            effect.DoAfterEffect(ref opposingCard, 0, ref me, 0, out dm);
            Assert.AreEqual(5, me.damage);
        }

        [Test]
        public void TestSetOnFire() {
            SetOnFire effect = new SetOnFire();

            Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
            effects.Add("SetOnFire", effect);

            me = new MonsterCard("TestCard", new ElementType("fire", "grass", "water"), 20, effects, new Human());
            effect.DoAfterEffect(ref opposingCard, 0, ref me, 0, out int dm);
            Assert.AreEqual(true, opposingCard.effects.ContainsKey("OnFire"));
        }

        [Test]
        public void TestOverWhelm() {
            Overwehlm effect = new Overwehlm();

            Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
            effects.Add("Overwehlm", effect);

            me = new MonsterCard("TestCard", new ElementType("fire", "grass", "water"), 40, effects, new Human());
            effect.DoAfterEffect(ref opposingCard, opposingCard.damage, ref me, me.damage, out int dm);
            Assert.AreEqual(20, dm);
        }

        [Test]
        public void TestBuff() {
            Buff effect = new Buff(20);

            Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
            effects.Add("Buff", effect);

            me = new MonsterCard("TestCard", new ElementType("fire", "grass", "water"), 20, effects, new Human());
            effect.DoAfterEffect(ref opposingCard, opposingCard.damage, ref me, me.damage, out int dm);
            Assert.AreEqual(20, dm);
        }

        [Test]
        public void TestSpellshield() {
            Card spell = new SpellCard("TestCard", new ElementType("normal", "-", "-"), 20, new Dictionary<string, Effect>());

            Spellshield effect = new Spellshield();

            Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
            effects.Add("Spellshield", effect);

            me = new MonsterCard("TestCard", new ElementType("fire", "grass", "water"), 40, effects, new Human());
            effect.DoBeforeEffect(ref spell, ref me);
            Assert.AreEqual(10, spell.damage);
        }

        [Test]
        public void TestNegateType() {
            NegateType effect = new NegateType();

            Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
            effects.Add("NegateType", effect);

            me = new MonsterCard("TestCard", new ElementType("fire", "grass", "water"), 40, effects, new Human());
            effect.DoBeforeEffect(ref opposingCard, ref me);
            Assert.AreEqual("normal", opposingCard.type.name);
        }

        [Test]
        public void TestDoublePower() {
            DoublePower effect = new DoublePower();

            Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
            effects.Add("DoublePower", effect);

            me = new MonsterCard("TestCard", new ElementType("fire", "grass", "water"), 40, effects, new Human());
            effect.DoAfterEffect(ref opposingCard, opposingCard.damage, ref me, me.damage, out int dm);
            Assert.AreEqual(80, me.damage);
        }

        [TearDown]
        public void TearDown() {
            
        }
    }
}