using RestSharp;
using System.Net;
using UnitTest.Ultility;
using Xunit.Abstractions;

namespace UnitTest
{
    public class CreatePracticeQuestionTest
    {
        private readonly ITestOutputHelper _output;

        public CreatePracticeQuestionTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UTC01_AddSuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/PracticeQuestion/CreatePracticeQuestion");
            string payload = $$"""
                {
                  "chapterId": 1,
                  "practiceQuestion": {
                    "id": 0,
                    "description": "Example description",
                    "chapterId": 0,
                    "codeForm": "Example",
                    "testCaseJava": "Example",
                    "testCases": []
                  }
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                // Assert
                Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
            }

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC02_InvalidChapter()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/PracticeQuestion/CreatePracticeQuestion");
            string payload = $$"""
                {
                  "chapterId": -1,
                  "practiceQuestion": {
                    "id": 0,
                    "description": "Example description",
                    "chapterId": 0,
                    "codeForm": "Example",
                    "testCaseJava": "Example",
                    "testCases": []
                  }
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

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
        public void UTC03_NotEmpty()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/PracticeQuestion/CreatePracticeQuestion");
            string payload = $$"""
                {
                  "chapterId": 1,
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

            // Rest response
            var restResponse = restClient.Post(restRequest);

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
