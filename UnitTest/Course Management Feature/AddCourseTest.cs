using RestSharp;
using System.Net;
using UnitTest.Ultility;
using Xunit.Abstractions;

namespace UnitTest
{
    public class AddCourseTest
    {
        private readonly ITestOutputHelper _output;

        public AddCourseTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UTC01_AddSuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/CourseModeration/AddCourse");
            string payload = $$"""
                {
                  "name": "Java",
                  "description": "This is examples description",
                  "picture": "https://codehustler.org/wp-content/uploads/2012/12/java_logo.png",
                  "tag": "#easy #java",
                  "createdBy": 1
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            if (restResponse.Content != null)
            {
                // Assert
                Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC02_NotEmpty()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/CourseModeration/AddCourse");
            string payload = $$"""
                {
                  "name": "",
                  "description": "",
                  "picture": "",
                  "tag": "",
                  "createdBy": 0
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
        public void UTC03_InvalidLength()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/CourseModeration/AddCourse");
            string payload = $$"""
                {
                  "name": "rEzpiCh3QFZno4q8Wjbk6VlmLDyf7gJBS05XxGcv91R2auMTKwHOYsNtUAPeI,RYZc6H7JSDXu4Z8e2Fj3n1ayrQbGWtxB5UoKOMqEgCTsvzpmhI9dw0fNPALVl,SuHc8nFYf1C9QVzJe5jsr0ETl2Lm4qAwv3PhKGBd6XpNRgZaIyWkbiUDo7xOMt,JC2jZd4K7TNBOH9esuQfqk3hUg5lFz6t1DWXvEwAobGm0Lp8PYcVnxiRMrayIS,QwEeKTX6IrWD8s1UCJ2BnROo0ALfGh5Pzumk9ydNv4cVjx7aYb3qHSpMtgZlF,yO0xkqNz2PnLcJ5giE7m6doTsAlSKhWfXQ3ZpB48u1MVYtavj9CIRwDUFHrbeG,Q25gbW4riX3uDl1KsG7OPZdN0MhmH8zqRvaYy6ptBLkfInUjScJTwAxVeo9CEx,9Lu4C2Vc8kqyMi6n0rThE5smgj1PwFaUeXzDGt7SQKZfWRoHb3IJlYxBdNpvO,1NcPd90Jt5ZueO6FmKw7YBs2X8G3RyTrnzHQIvfAWV4xSkLplDEbUjqoMiagC,45f7VKUvzrc6NpHQB3oLmSqT1Wha9EX2CYIuk0sjO8iPZDwFJRlxdeMnAtyGg,UIpcw2aLtul6SoEzV7YrA5xKmQXWCR38v9HgbGd4i0qjyefJOMFBhTnZ1DkNs",
                  "description": "This is examples description",
                  "picture": "https://codehustler.org/wp-content/uploads/2012/12/java_logo.png",
                  "tag": "#easy #java",
                  "createdBy": 1
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            // Convert response to json
            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG27", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public void UTC04_InvalidUser()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.BaseUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/CourseModeration/AddCourse");
            string payload = $$"""
                {
                  "name": "Java",
                  "description": "This is examples description",
                  "picture": "https://codehustler.org/wp-content/uploads/2012/12/java_logo.png",
                  "tag": "#easy #java",
                  "createdBy": -1
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = restClient.Post(restRequest);

            // Convert response to json
            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG24", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
