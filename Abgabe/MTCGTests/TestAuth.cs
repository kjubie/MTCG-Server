using MTCG_Server;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG_ServerTest {
    public class TestAuth {
        public HttpClient client;

        [SetUp]
        public void Setup() {
            client = new HttpClient();
        }

        [Test]
        public async Task TestLoginCorrect() {
            HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:25575/sessions", new StringContent("{\"Username\":\"kjubie\", \"Password\":\"123\"}", Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("OK", sCode);
            StringAssert.AreEqualIgnoringCase("Successfully logged in! Your token: kjubie-mtcgToken", responseBody);
        }

        [Test]
        public async Task TestLoginIncorrect() {
            HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:25575/sessions", new StringContent("{\"Username\":\"kjobie\", \"Password\":\"125\"}", Encoding.UTF8, "application/json"));

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("OK", sCode);
            StringAssert.AreEqualIgnoringCase("Login Failed!", responseBody);
        }

        [Test]
        public async Task TokenAuthCorrect() {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("kjubie-mtcgToken", "kjubie-mtcgToken");

            HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:25575/deck");
            response.EnsureSuccessStatusCode();

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("OK", sCode);
        }

        [Test]
        public async Task TokenAuthIncorrect() {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("sdghs-mtcgToken", "dsgjuifd-mtcgToken");

            HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:25575/deck");

            string sCode = response.StatusCode.ToString();
            string responseBody = await response.Content.ReadAsStringAsync();

            StringAssert.AreEqualIgnoringCase("Unauthorized", sCode);
        }

        [TearDown]
        public void TearDown() {
            
        }
    }
}