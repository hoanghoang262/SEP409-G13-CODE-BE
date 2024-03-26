using RestSharp;
using System.Net;
using UnitTest.Ultility;
using Xunit.Abstractions;

namespace UnitTest
{
    public class DeleteLastExamTest
    {
        private readonly ITestOutputHelper _output;

        public DeleteLastExamTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UTC01_DeleteSuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LastExam/DeleteLastExam");
            string payload = $$"""
                {
                  "id": 1
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
        public void UTC02_InvalidExam()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LastExam/DeleteLastExam");
            string payload = $$"""
                {
                  "id": -1
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG32", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
