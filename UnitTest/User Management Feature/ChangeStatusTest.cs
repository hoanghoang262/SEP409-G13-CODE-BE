using RestSharp;
using System.Net;
using UnitTest.Ultility;
using Xunit.Abstractions;

namespace UnitTest
{
    public class ChangeStatusTest
    {
        private readonly ITestOutputHelper _output;

        public ChangeStatusTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UTC01_Success()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/ChangeStatus");
            string payload = $$"""
                {
                  "userId": 1
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG16", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC02_InvalidUser()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/ChangeStatus");
            string payload = $$"""
                {
                  "userId": -1
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG01", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
