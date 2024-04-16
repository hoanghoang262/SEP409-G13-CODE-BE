using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseFeature
{
    public class UpdateCommentInCourseTest
    {
        private readonly ITestOutputHelper _output;

        public UpdateCommentInCourseTest(ITestOutputHelper output)
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
            var restRequest = new RestRequest("/api/Comment/UpdateComment");
            string payload = $$"""
                {
                  "id": 13,
                  "commentContent": "Bổ ích quá",
                  "userId": 1039
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "13");

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Put);

            if (restResponse.Content != null)
            {
                // Assert
                Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC02_NotEmpty()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CommentFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Comment/UpdateComment");
            string payload = $$"""
                {
                  "id": 13,
                  "commentContent": "",
                  "userId": 1039
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "13");

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

        [Fact]
        public async Task UTC03_InvalidComment()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CommentFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Comment/UpdateComment");
            string payload = $$"""
                {
                  "id": -1,
                  "commentContent": "Bổ ích quá",
                  "userId": 1039
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
                Assert.Contains("MSG37", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
