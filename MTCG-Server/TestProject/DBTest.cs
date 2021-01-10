using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_ServerTest {
    public class DBTest {
        public HttpClient client;

        [SetUp]
        public void Setup() {
            client = new HttpClient();
            client.PostAsync("http://127.0.0.1:25575/", new StringContent("Hello World!", Encoding.UTF8, "text/plain"));
        }

        [Test]
        public async Task TestDeleteMessage() {
            _ = client.DeleteAsync("http://127.0.0.1:25575/message/0");

            HttpResponseMessage responseGet = await client.GetAsync("http://127.0.0.1:25575/message/0");

            HttpResponseMessage responseGetAll = await client.GetAsync("http://127.0.0.1:25575/messages");
            responseGetAll.EnsureSuccessStatusCode();

            string sCodeGet = responseGet.StatusCode.ToString();
            string responseBodyGet = await responseGet.Content.ReadAsStringAsync();

            string sCodeGetAll = responseGetAll.StatusCode.ToString();
            string responseBodyGetAll = await responseGetAll.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("NotFound", sCodeGet);
            StringAssert.AreEqualIgnoringCase("Message Not Found!", responseBodyGet);

            StringAssert.AreEqualIgnoringCase("OK", sCodeGetAll);
            StringAssert.AreEqualIgnoringCase("No Messages Yet!", responseBodyGetAll);
        }

        [Test]
        public async Task TestDeleteBadRequest() {
            HttpResponseMessage response = await client.DeleteAsync("http://127.0.0.1:25575/messagedeshusguislg");

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("BadRequest", sCode);
            StringAssert.AreEqualIgnoringCase("Bad Request!", responseBody);
        }

        [Test]
        public async Task TestDeleteMessageNotExistent() {
            HttpResponseMessage response = await client.DeleteAsync("http://127.0.0.1:25575/message/32");

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("NotFound", sCode);
            StringAssert.AreEqualIgnoringCase("Invalid Message ID!", responseBody);
        }

        [TearDown]
        public void TearDown() {
            client.DeleteAsync("http://127.0.0.1:25575/message/0");
        }
    }
}