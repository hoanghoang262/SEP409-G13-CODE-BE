using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseFeature
{
    public class CreateCommentInLessonTest
    {
        private readonly ITestOutputHelper _output;

        public CreateCommentInLessonTest(ITestOutputHelper output)
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
            var restRequest = new RestRequest("/api/Comment/CreateComment");
            string payload = $$"""
                {
                  "lessonId": 2028,
                  "commentContent": "Bài học hay đó",
                  "userId": 1039
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

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
        public async Task UTC02_NotEmpty()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CommentFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Comment/CreateComment");
            string payload = $$"""
                {
                  "lessonId": 2028,
                  "commentContent": "",
                  "userId": 1039
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

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
