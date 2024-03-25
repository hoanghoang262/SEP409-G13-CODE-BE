using RestSharp;
using System.Net;
using UnitTest.Ultility;
using Xunit.Abstractions;

namespace UnitTest
{
    public class UpdatePracticeQuestionTest
    {
        private readonly ITestOutputHelper _output;

        public UpdatePracticeQuestionTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UTC01_UpdateSuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("");
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

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                // Assert
                Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC02_InvalidQuestion()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("");
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

            // Rest response
            var restResponse = restClient.Post(restRequest);

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
        public void UTC03_NotEmpty()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("");
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
