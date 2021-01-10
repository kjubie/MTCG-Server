using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_ServerTest {
    public class BattleTest {
        public HttpClient client;

        [SetUp]
        public void Setup() {
            client = new HttpClient();
            client.PostAsync("http://127.0.0.1:25575/", new StringContent("Hello World!", Encoding.UTF8, "text/plain"));
            client.PostAsync("http://127.0.0.1:25575/", new StringContent("Hallo Welt!", Encoding.UTF8, "text/plain"));
        }

        [Test]
        public async Task TestPutMessage() {
            HttpResponseMessage responsePut = await client.PutAsync("http://127.0.0.1:25575/message/0", new StringContent("Hello Earth!", Encoding.UTF8, "text/plain"));
            responsePut.EnsureSuccessStatusCode();

            HttpResponseMessage responseGet = await client.GetAsync("http://127.0.0.1:25575/message/0");
            responseGet.EnsureSuccessStatusCode();

            string sCodePut = responsePut.StatusCode.ToString();
            string responseBodyPut = await responsePut.Content.ReadAsStringAsync();

            string sCodeGet = responseGet.StatusCode.ToString();
            string responseBodyGet = await responseGet.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("OK", sCodePut);
            StringAssert.AreEqualIgnoringCase("Updated Message!", responseBodyPut);

            StringAssert.AreEqualIgnoringCase("OK", sCodeGet);
            StringAssert.AreEqualIgnoringCase("Hello Earth!", responseBodyGet);
        }

        [Test]
        public async Task TestPutBadContentType() {
            HttpResponseMessage response = await client.PutAsync("http://127.0.0.1:25575/message/1", new StringContent("<html></html>", Encoding.UTF8, "text/html"));

            HttpResponseMessage responseGet = await client.GetAsync("http://127.0.0.1:25575/message/1");
            responseGet.EnsureSuccessStatusCode();

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            string sCodeGet = responseGet.StatusCode.ToString();
            string responseBodyGet = await responseGet.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("BadRequest", sCode);
            StringAssert.AreEqualIgnoringCase("Bad Content Type!", responseBody);

            StringAssert.AreEqualIgnoringCase("OK", sCodeGet);
            StringAssert.AreEqualIgnoringCase("Hallo Welt!", responseBodyGet);
        }

        [Test]
        public async Task TestPutBadRequest() {
            HttpResponseMessage response = await client.PutAsync("http://127.0.0.1:25575/messagedeshusguislg", new StringContent("Text", Encoding.UTF8, "text/plain"));

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("BadRequest", sCode);
            StringAssert.AreEqualIgnoringCase("Bad Request!", responseBody);
        }

        [Test]
        public async Task TestPutMessageNotExistent() {
            HttpResponseMessage response = await client.PutAsync("http://127.0.0.1:25575/message/32", new StringContent("Text", Encoding.UTF8, "text/plain"));

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("NotFound", sCode);
            StringAssert.AreEqualIgnoringCase("Message Does Not Exist!", responseBody);
        }

        [TearDown]
        public void TearDown() {
            client.DeleteAsync("http://127.0.0.1:25575/message/1");
            client.DeleteAsync("http://127.0.0.1:25575/message/0");
        }
    }
}