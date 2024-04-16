using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseFeature
{
    public class UpdateReplyInCourseTest
    {
        private readonly ITestOutputHelper _output;

        public UpdateReplyInCourseTest(ITestOutputHelper output)
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
            var restRequest = new RestRequest("/api/Comment/UpdateReply");
            string payload = $$"""
                {
                  "replyId": 15,
                  "replyContent": "Tôi đồng ý cả 2 tay"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "15");

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Put);

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
            var restRequest = new RestRequest("/api/Comment/UpdateReply");
            string payload = $$"""
                {
                  "replyId": -1,
                  "replyContent": "Tôi đồng ý cả 2 tay"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "-1");

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Put);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG38", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC03_NotEmpty()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CommentFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Comment/UpdateReply");
            string payload = $$"""
                {
                  "replyId": 15,
                  "replyContent": ""
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "15");

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Put);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG11", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
