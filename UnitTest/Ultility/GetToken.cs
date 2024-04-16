using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text.Json;

namespace Test.Ultility
{
    public static class GetToken
    {
        public static string GetAdminBusinessToken()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/Login");
            string payload = $$"""
                {
                  "userName": "binh1234567",
                  "password": "Binh123456@"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Execute(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // convert response to object
                var loginModel = JsonSerializer.Deserialize<LoginModel>(response);

                return loginModel.value.token;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetAdminSystemToken()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.AuthenticateFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/Authenticate/Login");
            string payload = $$"""
                {
                  "userName": "kan",
                  "password": "12345"
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Execute(restRequest, Method.Post);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // convert response to object
                var loginModel = JsonSerializer.Deserialize<LoginModel>(response);

                return loginModel.value.token;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
