using RestSharp;
using System.Net;
using Test.Ultility;
using Xunit.Abstractions;

namespace Test.CourseManagementFeature
{
    public class UpdateLastExamTest
    {
        private readonly ITestOutputHelper _output;

        public UpdateLastExamTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task UTC01_UpdateSuccess()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LastExam/UpdateLastExam?id=1");
            string payload = $$"""
                {
                  "lastExamId": 1,
                  "lastExam": {
                    "id": 0,
                    "chapterId": 0,
                    "percentageCompleted": 0,
                    "name": "Quiz 1 is edited",
                    "time": 0,
                    "questionExams": [
                      {
                        "id": 0,
                        "contentQuestion": "2 + 2 =",
                        "score": 1,
                        "status": true,
                        "lastExamId": 0,
                        "answerExams": [
                          {
                            "id": 0,
                            "examId": 0,
                            "optionsText": "4",
                            "correctAnswer": null
                          }
                        ]
                      }
                    ]
                  }
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Put);

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

        [Fact]
        public async Task UTC02_InvalidExam()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LastExam/UpdateLastExam?id=-1");
            string payload = $$"""
                {
                  "lastExamId": -1,
                  "lastExam": {
                    "id": 0,
                    "chapterId": 0,
                    "percentageCompleted": 0,
                    "name": "Quiz 1 is edited",
                    "time": 0,
                    "questionExams": [
                      {
                        "id": 0,
                        "contentQuestion": "2 + 2 =",
                        "score": 1,
                        "status": true,
                        "lastExamId": 0,
                        "answerExams": [
                          {
                            "id": 0,
                            "examId": 0,
                            "optionsText": "4",
                            "correctAnswer": null
                          }
                        ]
                      }
                    ]
                  }
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Put);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG32", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }

        [Fact]
        public async Task UTC03_NotEmpty()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LastExam/UpdateLastExam?id=0");
            string payload = $$"""
                {
                  "lastExamId": 0,
                  "lastExam": {
                    "id": 0,
                    "chapterId": 0,
                    "percentageCompleted": 0,
                    "name": "",
                    "time": 0,
                    "questionExams": [
                      {
                        "id": 0,
                        "contentQuestion": "",
                        "score": 0,
                        "status": true,
                        "lastExamId": 0,
                        "answerExams": [
                          {
                            "id": 0,
                            "examId": 0,
                            "optionsText": "",
                            "correctAnswer": true
                          }
                        ]
                      }
                    ]
                  }
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Put);

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
        public async Task UTC04_InvalidLength()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LastExam/UpdateLastExam?id=1");
            string payload = $$"""
                {
                  "lastExamId": 1,
                  "lastExam": {
                    "id": 0,
                    "chapterId": 0,
                    "percentageCompleted": 80,
                    "name": "rEzpiCh3QFZno4q8Wjbk6VlmLDyf7gJBS05XxGcv91R2auMTKwHOYsNtUAPeI,RYZc6H7JSDXu4Z8e2Fj3n1ayrQbGWtxB5UoKOMqEgCTsvzpmhI9dw0fNPALVl,SuHc8nFYf1C9QVzJe5jsr0ETl2Lm4qAwv3PhKGBd6XpNRgZaIyWkbiUDo7xOMt,JC2jZd4K7TNBOH9esuQfqk3hUg5lFz6t1DWXvEwAobGm0Lp8PYcVnxiRMrayIS,QwEeKTX6IrWD8s1UCJ2BnROo0ALfGh5Pzumk9ydNv4cVjx7aYb3qHSpMtgZlF,yO0xkqNz2PnLcJ5giE7m6doTsAlSKhWfXQ3ZpB48u1MVYtavj9CIRwDUFHrbeG,Q25gbW4riX3uDl1KsG7OPZdN0MhmH8zqRvaYy6ptBLkfInUjScJTwAxVeo9CEx,9Lu4C2Vc8kqyMi6n0rThE5smgj1PwFaUeXzDGt7SQKZfWRoHb3IJlYxBdNpvO,1NcPd90Jt5ZueO6FmKw7YBs2X8G3RyTrnzHQIvfAWV4xSkLplDEbUjqoMiagC,45f7VKUvzrc6NpHQB3oLmSqT1Wha9EX2CYIuk0sjO8iPZDwFJRlxdeMnAtyGg,UIpcw2aLtul6SoEzV7YrA5xKmQXWCR38v9HgbGd4i0qjyefJOMFBhTnZ1DkNs",
                    "time": 0,
                    "questionExams": [
                      {
                        "id": 0,
                        "contentQuestion": "rEzpiCh3QFZno4q8Wjbk6VlmLDyf7gJBS05XxGcv91R2auMTKwHOYsNtUAPeI,RYZc6H7JSDXu4Z8e2Fj3n1ayrQbGWtxB5UoKOMqEgCTsvzpmhI9dw0fNPALVl,SuHc8nFYf1C9QVzJe5jsr0ETl2Lm4qAwv3PhKGBd6XpNRgZaIyWkbiUDo7xOMt,JC2jZd4K7TNBOH9esuQfqk3hUg5lFz6t1DWXvEwAobGm0Lp8PYcVnxiRMrayIS,QwEeKTX6IrWD8s1UCJ2BnROo0ALfGh5Pzumk9ydNv4cVjx7aYb3qHSpMtgZlF,yO0xkqNz2PnLcJ5giE7m6doTsAlSKhWfXQ3ZpB48u1MVYtavj9CIRwDUFHrbeG,Q25gbW4riX3uDl1KsG7OPZdN0MhmH8zqRvaYy6ptBLkfInUjScJTwAxVeo9CEx,9Lu4C2Vc8kqyMi6n0rThE5smgj1PwFaUeXzDGt7SQKZfWRoHb3IJlYxBdNpvO,1NcPd90Jt5ZueO6FmKw7YBs2X8G3RyTrnzHQIvfAWV4xSkLplDEbUjqoMiagC,45f7VKUvzrc6NpHQB3oLmSqT1Wha9EX2CYIuk0sjO8iPZDwFJRlxdeMnAtyGg,UIpcw2aLtul6SoEzV7YrA5xKmQXWCR38v9HgbGd4i0qjyefJOMFBhTnZ1DkNs",
                        "score": 1,
                        "status": true,
                        "lastExamId": 0,
                        "answerExams": [
                          {
                            "id": 0,
                            "examId": 0,
                            "optionsText": "rEzpiCh3QFZno4q8Wjbk6VlmLDyf7gJBS05XxGcv91R2auMTKwHOYsNtUAPeI,RYZc6H7JSDXu4Z8e2Fj3n1ayrQbGWtxB5UoKOMqEgCTsvzpmhI9dw0fNPALVl,SuHc8nFYf1C9QVzJe5jsr0ETl2Lm4qAwv3PhKGBd6XpNRgZaIyWkbiUDo7xOMt,JC2jZd4K7TNBOH9esuQfqk3hUg5lFz6t1DWXvEwAobGm0Lp8PYcVnxiRMrayIS,QwEeKTX6IrWD8s1UCJ2BnROo0ALfGh5Pzumk9ydNv4cVjx7aYb3qHSpMtgZlF,yO0xkqNz2PnLcJ5giE7m6doTsAlSKhWfXQ3ZpB48u1MVYtavj9CIRwDUFHrbeG,Q25gbW4riX3uDl1KsG7OPZdN0MhmH8zqRvaYy6ptBLkfInUjScJTwAxVeo9CEx,9Lu4C2Vc8kqyMi6n0rThE5smgj1PwFaUeXzDGt7SQKZfWRoHb3IJlYxBdNpvO,1NcPd90Jt5ZueO6FmKw7YBs2X8G3RyTrnzHQIvfAWV4xSkLplDEbUjqoMiagC,45f7VKUvzrc6NpHQB3oLmSqT1Wha9EX2CYIuk0sjO8iPZDwFJRlxdeMnAtyGg,UIpcw2aLtul6SoEzV7YrA5xKmQXWCR38v9HgbGd4i0qjyefJOMFBhTnZ1DkNs",
                            "correctAnswer": null
                          }
                        ]
                      }
                    ]
                  }
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Put);

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
        public async Task UTC05_InvalidNumber()
        {
            // Rest client
            var restClient = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Url.ModerationFeatureUrl)
            });

            // Rest request
            var restRequest = new RestRequest("/api/LastExam/UpdateLastExam?id=1");
            string payload = $$"""
                {
                  "lastExamId": 1,
                  "lastExam": {
                    "id": 0,
                    "chapterId": 0,
                    "percentageCompleted": 0,
                    "name": "Quiz 1 is edited",
                    "time": -1,
                    "questionExams": [
                      {
                        "id": 0,
                        "contentQuestion": "2 + 2 =",
                        "score": -1,
                        "status": true,
                        "lastExamId": 0,
                        "answerExams": [
                          {
                            "id": 0,
                            "examId": 0,
                            "optionsText": "4",
                            "correctAnswer": null
                          }
                        ]
                      }
                    ]
                  }
                }
                """;

            restRequest.AddStringBody(payload, DataFormat.Json);

            // Rest response
            var restResponse = await restClient.ExecuteAsync(restRequest, Method.Put);

            if (restResponse.Content != null)
            {
                string response = restResponse.Content;

                // Assert
                Assert.Contains("MSG26", response);
            }

            // Log
            _output.WriteLine(restResponse.Content);
        }
    }
}
