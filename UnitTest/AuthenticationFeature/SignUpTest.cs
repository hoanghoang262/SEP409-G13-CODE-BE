using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.AuthenticationFeature
{
    public class SignUpTest
    {
        private readonly ITestOutputHelper _output;

        public SignUpTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_NotEmpty()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/SignUp");
            const string payload = $$"""
                {
                  "userName": "",
                  "password": "",
                  "email": ""
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
        public async Task UTC02_IvalidPassword()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/SignUp");
            const string payload = $$"""
                {
                  "userName": "Binh123456",
                  "password": "Binh12@",
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
                Assert.Contains("MSG17", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC03_IvalidPassword()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/SignUp");
            const string payload = $$"""
                {
                  "userName": "Binh123456",
                  "password": "binh123456",
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
                Assert.Contains("MSG17", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC04_InvalidEmail()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/SignUp");
            const string payload = $$"""
                {
                  "userName": "Binh123456",
                  "password": "Binh123456@",
                  "email": "Binh@@gmail.com"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG09", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC05_ExistedEmail()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/SignUp");
            const string payload = $$"""
                {
                  "userName": "Binh123456",
                  "password": "Binh123456@",
                  "email": "Binhphhe160537@fpt.edu.vn"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG06", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC06_InvalidUsername()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/SignUp");
            const string payload = $$"""
                {
                  "userName": "binh123",
                  "password": "Binh123456@",
                  "email": "binhprohp00@gmail.com"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG21", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC07_Success()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/SignUp");
            const string payload = $$"""
                {
                  "userName": "binh123456",
                  "password": "Binh123456@",
                  "email": "binhprohp00@gmail.com"
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

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}