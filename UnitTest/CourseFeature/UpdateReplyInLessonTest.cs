using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseFeature
{
    public class UpdateReplyInLessonTest
    {
        private readonly ITestOutputHelper _output;

        public UpdateReplyInLessonTest(ITestOutputHelper output)
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
                  "replyId": 18,
                  "replyContent": "Bài học này quá dễ"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "18");

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
                  "replyContent": "Bài học này quá dễ"
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
                  "replyId": 18,
                  "replyContent": ""
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "18");

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
