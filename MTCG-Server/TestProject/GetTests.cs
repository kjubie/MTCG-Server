using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_ServerTest {
    public class TestGet {
        public HttpClient client;

        [SetUp]
        public void Setup() {
            client = new HttpClient();

            client.PostAsync("http://127.0.0.1:25575/", new StringContent("Hello World!", Encoding.UTF8, "text/plain"));
            client.PostAsync("http://127.0.0.1:25575/", new StringContent("Hallo Welt!", Encoding.UTF8, "text/plain"));
        }

        [Test]
        public async Task TestGetAll() {
            HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:25575/messages");
            response.EnsureSuccessStatusCode();

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("OK", sCode);
            StringAssert.AreEqualIgnoringCase("Hello World!\nHallo Welt!\n", responseBody);
        }

        [Test]
        public async Task TestGetMessage0() {
            HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:25575/message/0");
            response.EnsureSuccessStatusCode();

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("OK", sCode);
            StringAssert.AreEqualIgnoringCase("Hello World!", responseBody);
        }

        [Test]
        public async Task TestGetMessage1() {
            HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:25575/message/1");
            response.EnsureSuccessStatusCode();

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("OK", sCode);
            StringAssert.AreEqualIgnoringCase("Hallo Welt!", responseBody);
        }

        [Test]
        public async Task TestGetMessageNotExistent() {
            HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:25575/message/34");

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("NotFound", sCode);
            StringAssert.AreEqualIgnoringCase("Message Not Found!", responseBody);
        }

        [Test]
        public async Task TestGetBadResource() {
            HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:25575/messagewds/jsahfa");

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("BadRequest", sCode);
            StringAssert.AreEqualIgnoringCase("Bad Request!", responseBody);
        }

        [TearDown]
        public void TearDown() {
            client.DeleteAsync("http://127.0.0.1:25575/message/1");
            client.DeleteAsync("http://127.0.0.1:25575/message/0");
        }
    }
}