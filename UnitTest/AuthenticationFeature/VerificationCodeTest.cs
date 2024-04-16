using RestSharp;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.AuthenticationFeature
{
    public class VerificationCode
    {
        private readonly ITestOutputHelper _output;

        public VerificationCode(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_VerifySuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/ForgortPassword/VerificationCode");
            string payload = $$"""
                {
                  "code": "123456",
                  "email": "binhprohp01@gmail.com"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

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
        public async Task UTC02_VerifyFailed()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/ForgortPassword/VerificationCode");
            string payload = $$"""
                {
                  "code": "123456",
                  "email": "binhprohp01@gmail.com"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG15", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
