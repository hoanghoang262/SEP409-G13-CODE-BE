using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseManagementFeature
{
    public class UpdatePracticeQuestionTest
    {
        private readonly ITestOutputHelper _output;

        public UpdatePracticeQuestionTest(ITestOutputHelper output)
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
            var restRequest = new RestRequest("/api/PracticeQuestion/UpdatePracticeQuestion");
            string payload = $$"""
                {
                  "practiceQuestionId": 1,
                  "practiceQuestion": {
                    "id": 0,
                    "description": "Examples description is edited",
                    "chapterId": 0,
                    "codeForm": "example",
                    "testCaseJava": "example",
                    "testCases": []
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
        public async Task UTC02_InvalidQuestion()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/PracticeQuestion/UpdatePracticeQuestion");
            string payload = $$"""
                {
                  "practiceQuestionId": -1,
                  "practiceQuestion": {
                    "id": 0,
                    "description": "Examples description is edited",
                    "chapterId": 0,
                    "codeForm": "example",
                    "testCaseJava": "example",
                    "testCases": []
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
                Assert.Contains("MSG31", response);
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
            var restRequest = new RestRequest("/api/PracticeQuestion/UpdatePracticeQuestion");
            string payload = $$"""
                {
                  "practiceQuestionId": 0,
                  "practiceQuestion": {
                    "id": 0,
                    "description": "",
                    "chapterId": 0,
                    "codeForm": "",
                    "testCaseJava": "",
                    "testCases": []
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
