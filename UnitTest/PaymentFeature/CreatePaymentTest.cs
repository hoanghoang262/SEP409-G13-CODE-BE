using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.PaymentFeature
{
    public class CreatePaymentTest
    {
        private readonly ITestOutputHelper _output;

        public CreatePaymentTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_Success()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.PaymentFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Payments/CreatePayment");
            string payload = $$"""
                {
                  "requiredAmount": 1000,
                  "userCreateCourseId": ,
                  "courseId": 0
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
    }
}
