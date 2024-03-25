using RestSharp;
using System.Text.Json;
using UnitTest.Ultility;
using Xunit.Abstractions;

namespace UnitTest
{
    public class DeactiveAccount
    {
        private readonly ITestOutputHelper _output;

        public DeactiveAccount(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UTC01()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("");
            string payload = $$"""
                {
                  
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
