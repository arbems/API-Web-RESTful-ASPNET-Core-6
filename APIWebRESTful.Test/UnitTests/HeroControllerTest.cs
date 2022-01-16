using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using APIWebRESTful.Models;
using Newtonsoft.Json;
using Xunit;

namespace APIWebRESTful.Test.UnitTests
{
    public class HeroControllerTest
    {
        [Theory]
        [InlineData("heroes")]
        [InlineData("heroes2")]
        public async Task ReturnsNotFoundForId0(string controllerName)
        {
            HttpClient _client = new HttpClient();
            var hero = new Hero()
            {
                Id = 0,
                Name = "test",
                IsPopulate = true,
                Secret = ""
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(hero),
               Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/v1/heroes/0", jsonContent);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Equal("0", stringResponse);
        }
    }
}
