using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseFeature
{
    public class SubmitLastExamTest
    {
        private readonly ITestOutputHelper _output;

        public SubmitLastExamTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_Passed()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/CheckAnswer/SubmitLastExam");
            string payload = $$"""
                {
                  "lastExamId": 1,
                  "userId": 1,
                  "questions": [
                    {
                      "examId": 1,
                      "selectedAnswerIds": [
                        1
                      ]
                    }
                  ]
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG35", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC02_Failed()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/CheckAnswer/SubmitLastExam");
            string payload = $$"""
                {
                  "lastExamId": 1,
                  "userId": 1,
                  "questions": [
                    {
                      "examId": 1,
                      "selectedAnswerIds": [
                        0
                      ]
                    }
                  ]
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG36", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC03_InvalidQuestion()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/CheckAnswer/SubmitLastExam");
            string payload = $$"""
                {
                  "lastExamId": -1,
                  "userId": 1,
                  "questions": [
                    {
                      "examId": 0,
                      "selectedAnswerIds": [
                        0
                      ]
                    }
                  ]
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG31", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
