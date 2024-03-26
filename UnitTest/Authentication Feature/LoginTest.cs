using RestSharp;
using System.Text.Json;
using UnitTest.Ultility;
using Xunit.Abstractions;

namespace UnitTest
{
    public class LoginTest
    {
        private readonly ITestOutputHelper _output;

        public LoginTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UTC01_LoginSuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/Login");
            string payload = $$"""
                {
                  "userName": "string",
                  "password": "string"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            // Assert
            Assert.Contains("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9", restResponse.Content);

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC02_IvalidAccount()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/Login");
            string payload = $$"""
                {
                  "userName": "asdasdasd",
                  "password": "Asdasdasd12@"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG01", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC03_NullUsername()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/Login");
            string payload = $$"""
                {
                  "userName": "",
                  "password": "Binh123456@"
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
        public void UTC04_IncorrectPassword()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri("http://localhost:5000")
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/Login");
            string payload = $$"""
                {
                  "userName": "Binh123456",
                  "password": "Asdasdasd12@"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG04", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC05_NullPassword()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/Login");
            string payload = $$"""
                {
                  "userName": "Binh123456",
                  "password": ""
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
        public void UTC06_EmailNotConfirm()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/Login");
            string payload = $$"""
                {
                  "userName": "Binh123456",
                  "password": "Binh123456@"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG03", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
