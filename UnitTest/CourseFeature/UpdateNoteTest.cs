using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseFeature
{
    public class UpdateNoteTest
    {
        private readonly ITestOutputHelper _output;

        public UpdateNoteTest(ITestOutputHelper output)
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
            var restRequest = new RestRequest("/api/Note/UpdateNote");
            string payload = $$"""
                {
                  "id": 3,
                  "lessonId": 2028,
                  "userId": 1039,
                  "contentNote": "Chỗ này có kiến thức quan trọng",
                  "videoLink": 30
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "3");

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
        public async Task UTC02_NotEmpty()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Note/UpdateNote");
            string payload = $$"""
                {
                  "id": 3,
                  "lessonId": 2028,
                  "userId": 1039,
                  "contentNote": "",
                  "videoLink": 30
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "3");

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

        [Fact]
        public async Task UTC03_InvalidNumber()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Note/UpdateNote");
            string payload = $$"""
                {
                  "id": 3,
                  "lessonId": 2028,
                  "userId": 1039,
                  "contentNote": "Chỗ này có kiến thức quan trọng",
                  "videoLink": -1
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", "3");

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Put);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG26", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC03_InvalidNote()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Note/UpdateNote");
            string payload = $$"""
                {
                  "id": -1,
                  "lessonId": 2028,
                  "userId": 1039,
                  "contentNote": "Chỗ này có kiến thức quan trọng",
                  "videoLink": 30
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
                Assert.Contains("MSG39", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
