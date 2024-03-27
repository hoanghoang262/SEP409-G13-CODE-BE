using RestSharp;
using System.Net;
using UnitTest.Ultility;
using Xunit.Abstractions;

namespace UnitTest
{
    public class ModerationCourseTest
    {
        private readonly ITestOutputHelper _output;

        public ModerationCourseTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UTC01_Success()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Moderation/ModerationCourse");
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
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG16", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC02_InvalidCourse()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Moderation/ModerationCourse");
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
                Assert.Contains("MSG25", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
