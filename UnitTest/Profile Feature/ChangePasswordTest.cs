using RestSharp;
using UnitTest.Ultility;
using Xunit.Abstractions;

namespace UnitTest
{
    public class ChangePassword
    {
        private readonly ITestOutputHelper _output;

        public ChangePassword(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UTC01_ChangePasswordSuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/ChangePassword/ChangePass");
            string payload = $$"""
                {
                  "email": "binhprohp01@gmail.com",
                  "oldPassword": "Binh123456@",
                  "newPassword": "Binh1234567@"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG12", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC02_NullPassword()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/ChangePassword/ChangePass");
            string payload = $$"""
                {
                  "email": "binhprohp01@gmail.com",
                  "oldPassword": "",
                  "newPassword": ""
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

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
        public void UTC03_IncorrectOldPassword()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/ChangePassword/ChangePass");
            string payload = $$"""
                {
                  "email": "binhprohp01@gmail.com",
                  "oldPassword": "Binh123456@@",
                  "newPassword": "Binh1234567@"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG13", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC04_InvalidPassword()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/ChangePassword/ChangePass");
            string payload = $$"""
                {
                  "email": "binhprohp01@gmail.com",
                  "oldPassword": "Binh123456@",
                  "newPassword": "binh123"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG17", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC05_SamePassword()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/ChangePassword/ChangePass");
            string payload = $$"""
                {
                  "email": "binhprohp01@gmail.com",
                  "oldPassword": "Binh123456@",
                  "newPassword": "Binh123456@"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG18", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
