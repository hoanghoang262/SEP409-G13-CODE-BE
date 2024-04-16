using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.ForumFeature
{
    public class DeletePostTest
    {
        private readonly ITestOutputHelper _output;

        public DeletePostTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_Success()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ForumFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Forum/DeletePost");
            string payload = $$"""
                {

                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("postId", "13");

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Delete);

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
        public async Task UTC02_InvalidPost()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ForumFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Forum/DeletePost");
            string payload = $$"""
                {

                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("postId", "-1");

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Delete);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG34", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
