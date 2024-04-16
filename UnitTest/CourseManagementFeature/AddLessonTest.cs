using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseManagementFeature
{
    public class AddLessonTest
    {
        private readonly ITestOutputHelper _output;

        public AddLessonTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_AddSuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LessonModeration/CreateLesson");
            string payload = $$"""
                {
                  "chapterId": 1,
                  "lesson": {
                    "id": 0,
                    "title": "Example lesson",
                    "videoUrl": "https://youtu.be/dQw4w9WgXcQ?si=y6r2k1Wdk0TCL8vq",
                    "chapterId": 0,
                    "description": "Example description",
                    "duration": 3.14,
                    "contentLesson": "Example content",
                    "isCompleted": false,
                    "questions": []
                  }
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
        public async Task UTC02_InvalidChapter()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LessonModeration/CreateLesson");
            string payload = $$"""
                {
                  "chapterId": -1,
                  "lesson": {
                    "id": 0,
                    "title": "Example lesson",
                    "videoUrl": "https://youtu.be/dQw4w9WgXcQ?si=y6r2k1Wdk0TCL8vq",
                    "chapterId": 0,
                    "description": "Example description",
                    "duration": 3.14,
                    "contentLesson": "Example content",
                    "isCompleted": false,
                    "questions": []
                  }
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG28", response);
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
            var restRequest = new RestRequest("/api/LessonModeration/CreateLesson");
            string payload = $$"""
                {
                  "chapterId": 0,
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
