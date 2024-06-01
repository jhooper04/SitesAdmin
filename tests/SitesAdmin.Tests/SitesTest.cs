using Newtonsoft.Json;
using System.Dynamic;
using System.Text.Json;

namespace SitesAdmin.Tests
{
    [TestClass]
    public class SitesTest : BaseTest
    {
        [TestMethod]
        public async Task ListSites_ShouldBeUnauthorized()
        {
            var response = await _httpClient.GetAsync("sites");
            var stringResult = await response.Content.ReadAsStringAsync();

            dynamic? result = JsonConvert.DeserializeObject(stringResult);

            int status = result?.status ?? 0;

            Assert.AreEqual(401, status);
        }
    }
}