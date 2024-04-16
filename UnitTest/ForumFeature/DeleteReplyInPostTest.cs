using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.ForumFeature
{
    public class DeleteReplyInPostTest
    {
        private readonly ITestOutputHelper _output;

        public DeleteReplyInPostTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_Success()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CommentFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Comment/DeleteReply");
            string payload = $$"""
                {

                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", 21);

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
        public async Task UTC02_InvalidReply()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CommentFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Comment/DeleteReply");
            string payload = $$"""
                {

                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", -1);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Delete);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG38", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
