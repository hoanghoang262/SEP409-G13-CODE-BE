using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseFeature
{
    public class AddCourseEvaluationTest
    {
        private readonly ITestOutputHelper _output;

        public AddCourseEvaluationTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_Success()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/EvaluateCourse/AddCourseEvaluation");
            string payload = $$"""
                {
                  "courseId": 5,
                  "userId": 1040,
                  "star": 5
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
        public async Task UTC02_InvalidCourse()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/EvaluateCourse/AddCourseEvaluation");
            string payload = $$"""
                {
                  "courseId": -1,
                  "userId": 1040,
                  "star": 5
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG25", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC03_InvalidAccount()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/EvaluateCourse/AddCourseEvaluation");
            string payload = $$"""
                {
                  "courseId": 5,
                  "userId": -1,
                  "star": 5
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG01", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC04_InvalidNumber()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/EvaluateCourse/AddCourseEvaluation");
            string payload = $$"""
                {
                  "courseId": 5,
                  "userId": 1040,
                  "star": -1
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG26", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
