using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.ForumModerationFeature
{
    public class ModerationPostTest
    {
        private readonly ITestOutputHelper _output;

        public ModerationPostTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_Success()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Moderation/ModerationPost");
            string payload = $$"""
                {

                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("postId", "8");

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                // Assert
                Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
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
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Moderation/ModerationPost");
            string payload = $$"""
                {

                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("postId", "-1");

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

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
