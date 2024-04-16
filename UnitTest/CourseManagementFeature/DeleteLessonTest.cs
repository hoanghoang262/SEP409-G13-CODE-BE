using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseManagementFeature
{
    public class DeleteLessonTest
    {
        private readonly ITestOutputHelper _output;

        public DeleteLessonTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_DeleteSuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LessonModeration/DeleteLesson");
            string payload = $$"""
                {

                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", 1);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Delete);

            if (restResponse.Content != null)
            {
                // Assert
                Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC02_InvalidLesson()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LessonModeration/DeleteLesson");
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
                // Assert
                Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
            }

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG29", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
