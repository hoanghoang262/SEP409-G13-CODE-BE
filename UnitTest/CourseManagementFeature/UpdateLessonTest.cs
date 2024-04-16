using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseManagementFeature
{
    public class UpdateLessonTest
    {
        private readonly ITestOutputHelper _output;

        public UpdateLessonTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_UpdateSuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LessonModeration/UpdateLesson");
            string payload = $$"""
                {
                  "lessonId": 1,
                  "lesson": {
                    "id": 0,
                    "title": "Example lesson is edited",
                    "videoUrl": "https://youtu.be/dQw4w9WgXcQ?si=y6r2k1Wdk0TCL8vq",
                    "chapterId": 0,
                    "description": "Example description is edited",
                    "duration": 4.12,
                    "contentLesson": "Example content is edited",
                    "isCompleted": false,
                    "questions": []
                  }
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "1");

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
        public async Task UTC02_InvalidLesson()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LessonModeration/UpdateLesson");
            string payload = $$"""
                {
                  "lessonId": -1,
                  "lesson": {
                    "id": 0,
                    "title": "Example lesson is edited",
                    "videoUrl": "https://youtu.be/dQw4w9WgXcQ?si=y6r2k1Wdk0TCL8vq",
                    "chapterId": 0,
                    "description": "Example description is edited",
                    "duration": 4,
                    "contentLesson": "Example content is edited",
                    "isCompleted": false,
                    "questions": []
                  }
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
                Assert.Contains("MSG29", response);
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
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LessonModeration/UpdateLesson");
            string payload = $$"""
                {
                  "lessonId": 0,
                  "lesson": {
                    "id": 0,
                    "title": "",
                    "videoUrl": "",
                    "chapterId": 0,
                    "description": "",
                    "duration": 0,
                    "contentLesson": "",
                    "isCompleted": false,
                    "questions": []
                  }
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "0");

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
