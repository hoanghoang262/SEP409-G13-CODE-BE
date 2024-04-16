using RestSharp;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.ProfileFeature
{
    public class UpdateProfile
    {
        private readonly ITestOutputHelper _output;

        public UpdateProfile(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UTC01_NotExistedAccount()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Profile/UpdateProfile");
            string payload = $$"""
                {
                  "userId": -1,
                  "fullName": "Pham Hai Binh",
                  "address": "Hai Phong, Viet Nam",
                  "birthDate": "21/09/2002",
                  "facebookLink": "https://www.facebook.com/profile.php?id=100009869170794",
                  "profilePict": "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a6/Anonymous_emblem.svg/800px-Anonymous_emblem.svg.png",
                  "userName": "binh123456",
                  "phone": "0982268729"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Put(restRequest);

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
        public void UTC02_Null()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Profile/UpdateProfile");
            string payload = $$"""
                {
                  "userId": 1,
                  "fullName": "",
                  "address": "Hai Phong, Viet Nam",
                  "birthDate": "21/09/2002",
                  "facebookLink": "https://www.facebook.com/profile.php?id=100009869170794",
                  "profilePict": "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a6/Anonymous_emblem.svg/800px-Anonymous_emblem.svg.png",
                  "userName": "",
                  "phone": "0982268729"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Put(restRequest);

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
        public void UTC03_UpdateSuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Profile/UpdateProfile");
            string payload = $$"""
                {
                  "userId": 1,
                  "fullName": "Pham Hai Binh",
                  "address": "Hai Phong, Viet Nam",
                  "birthDate": "21/09/2002",
                  "facebookLink": "https://www.facebook.com/profile.php?id=100009869170794",
                  "profilePict": "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a6/Anonymous_emblem.svg/800px-Anonymous_emblem.svg.png",
                  "userName": "binh123456",
                  "phone": "0982268729"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Put(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG19", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC04_InvalidUsername()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Profile/UpdateProfile");
            string payload = $$"""
                {
                  "userId": 1,
                  "fullName": "Pham Hai Binh",
                  "address": "Hai Phong, Viet Nam",
                  "birthDate": "21/09/2002",
                  "facebookLink": "https://www.facebook.com/profile.php?id=100009869170794",
                  "profilePict": "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a6/Anonymous_emblem.svg/800px-Anonymous_emblem.svg.png",
                  "userName": "Pham Hai Binh",
                  "phone": "0982268729"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Put(restRequest);

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
        public void UTC05_InvalidPhone()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Profile/UpdateProfile");
            string payload = $$"""
                {
                  "userId": 1,
                  "fullName": "Pham Hai Binh",
                  "address": "Hai Phong, Viet Nam",
                  "birthDate": "21/09/2002",
                  "facebookLink": "https://www.facebook.com/profile.php?id=100009869170794",
                  "profilePict": "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a6/Anonymous_emblem.svg/800px-Anonymous_emblem.svg.png",
                  "userName": "Pham Hai Binh",
                  "phone": "121212121212"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Put(restRequest);

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
        public void UTC06_ExistedUsername()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Profile/UpdateProfile");
            string payload = $$"""
                {
                  "userId": 1,
                  "fullName": "Pham Hai Binh",
                  "address": "Hai Phong, Viet Nam",
                  "birthDate": "21/09/2002",
                  "facebookLink": "https://www.facebook.com/profile.php?id=100009869170794",
                  "profilePict": "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a6/Anonymous_emblem.svg/800px-Anonymous_emblem.svg.png",
                  "userName": "binh1234567",
                  "phone": "0982268729"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Put(restRequest);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG07", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
