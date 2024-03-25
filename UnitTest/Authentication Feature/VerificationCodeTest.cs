using RestSharp;
using System.Text.Json;
using UnitTest.Ultility;
using Xunit.Abstractions;

namespace UnitTest
{
    public class VerificationCode
    {
        private readonly ITestOutputHelper _output;

        public VerificationCode(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UTC01_VerifySuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
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
        public void UTC02_VerifyFailed()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
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
            var restResponse = restClient.Post(restRequest);

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
