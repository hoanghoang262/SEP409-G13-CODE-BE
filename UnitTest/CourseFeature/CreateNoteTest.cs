using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseFeature
{
    public class CreateNoteTest
    {
        private readonly ITestOutputHelper _output;

        public CreateNoteTest(ITestOutputHelper output)
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
            var restRequest = new RestRequest("/api/Note/CreateNote");
            string payload = $$"""
                {
                  "lessonId": 2028,
                  "userId": 1039,
                  "contentNote": "Chỗ này cần lưu ý",
                  "videoLink": 30
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
        public async Task UTC02_NotEmpty()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Note/CreateNote");
            string payload = $$"""
                {
                  "lessonId": 2028,
                  "userId": 1039,
                  "contentNote": "",
                  "videoLink": 30
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

        [Fact]
        public async Task UTC03_InvalidNumber()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Note/CreateNote");
            string payload = $$"""
                {
                  "lessonId": 2028,
                  "userId": 1039,
                  "contentNote": "Chỗ này cần lưu ý",
                  "videoLink": -1
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
