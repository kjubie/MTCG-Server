using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_ServerTest {
    public class TypeTest {
        public HttpClient client;

        [SetUp]
        public void Setup() {
            client = new HttpClient();
            
        }

        [Test]
        public async Task TestPostMessage() {
            HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:25575/", new StringContent("Hello World!", Encoding.UTF8, "text/plain"));
            response.EnsureSuccessStatusCode();

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("OK", sCode);
            StringAssert.AreEqualIgnoringCase("Message sent!", responseBody);
        }

        [Test]
        public async Task TestPostBadContentType() {
            HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:25575/", new StringContent("<html></html>", Encoding.UTF8, "text/html"));

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("BadRequest", sCode);
            StringAssert.AreEqualIgnoringCase("Bad Content Type!", responseBody);
        }


        [TearDown]
        public void TearDown() {
            client.DeleteAsync("http://127.0.0.1:25575/message/0");
        }
    }
}