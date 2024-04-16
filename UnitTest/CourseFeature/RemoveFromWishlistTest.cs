using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseFeature
{
    public class RemoveFromWishlistTest
    {
        private readonly ITestOutputHelper _output;

        public RemoveFromWishlistTest(ITestOutputHelper output)
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
            var restRequest = new RestRequest("/api/WishList/RemoveFromWishlist");
            string payload = $$"""
                {
                  "wishlistId": 1
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

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
        public async Task UTC02_InvalidWishlist()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.CourseFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/WishList/RemoveFromWishlist");
            string payload = $$"""
                {
                  "wishlistId": -1
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Delete);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG40", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
