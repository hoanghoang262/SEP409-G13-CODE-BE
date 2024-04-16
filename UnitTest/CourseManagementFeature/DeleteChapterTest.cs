using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseManagementFeature
{
    public class DeleteChapterTest
    {
        private readonly ITestOutputHelper _output;

        public DeleteChapterTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_Sucess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/ChapterModeration/DeleteChapter?id={id}");
            string payload = $$"""
                {

                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", 1);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Delete);

            if (restResponse.Content != null)
            {
                // Assert
                Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC02_InvalidChapter()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/ChapterModeration/DeleteChapter");
            string payload = $$"""
                {

                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);
            restRequest.AddQueryParameter("id", -1);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Delete);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG28", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
