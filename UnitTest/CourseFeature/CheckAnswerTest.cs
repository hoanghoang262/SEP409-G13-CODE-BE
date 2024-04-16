using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseFeature
{
    public class CheckAnswerTest
    {
        private readonly ITestOutputHelper _output;

        public CheckAnswerTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_ReturnTrue()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/CheckAnswer/CheckAnswer");
            string payload = $$"""
                [
                    1
                ]
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "1");

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
        public async Task UTC01_ReturnFalse()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/CheckAnswer/CheckAnswer");
            string payload = $$"""
                [
                    0
                ]
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "1");

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
        public async Task UTC03_InvalidQuestion()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/CheckAnswer/CheckAnswer");
            string payload = $$"""
                [
                    0
                ]
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "-1");

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
