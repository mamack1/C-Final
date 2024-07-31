using C_DiscApp.Controllers;
using C_DiscApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json.Linq;
using Moq.Protected;
using System;

namespace C_DiscAppUnitTests
{
    public class CourseControllerTests
    {
        private readonly IConfiguration _configuration;

        public CourseControllerTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                { "GoogleApiKey", "TEST_GOOGLE_API_KEY" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        private IDiscService GetDiscService()
        {
            var mockDiscService = new Mock<IDiscService>();
            return mockDiscService.Object;
        }

        [Fact]
        public void Index_ReturnsView()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var controller = new CourseController(mockHttpClientFactory.Object, _configuration);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SearchNearestCourse_ReturnsJsonResultWithCourses()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"results\":[{\"name\":\"Course 1\",\"vicinity\":\"Address 1\",\"geometry\":{\"location\":{\"lat\":40.712776,\"lng\":-74.005974}}}]}")
                })
                .Verifiable();

            var client = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://maps.googleapis.com")
            };

            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var controller = new CourseController(mockHttpClientFactory.Object, _configuration);

            // Act
            var result = await controller.SearchNearestCourse(40.712776, -74.005974) as JsonResult;

            // Assert
            Assert.NotNull(result);

            // Parse directly to a JArray
            var resultsArray = JArray.FromObject(result.Value);
            Assert.NotNull(resultsArray);
            Assert.Single(resultsArray);

            // Now you can access elements within the array
            var firstResult = resultsArray.First as JObject;
            Assert.NotNull(firstResult);
            Assert.Equal("Course 1", firstResult["name"]);
        }

        [Fact]
        public async Task SearchNearestCourse_ReturnsJsonResultWithError()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("{\"error\":\"An error occurred\"}")
                })
                .Verifiable();

            var client = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://maps.googleapis.com")
            };

            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var controller = new CourseController(mockHttpClientFactory.Object, _configuration);

            // Act
            var result = await controller.SearchNearestCourse(40.712776, -74.005974) as JsonResult;

            // Assert
            Assert.NotNull(result);
            var jsonResult = JToken.FromObject(result.Value);
            Assert.NotNull(jsonResult);
            Assert.NotNull(jsonResult["error"]);
        }

        [Fact]
        public async Task SearchNearestCourse_ReturnsJsonResultWithMultipleCourses()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"results\":[{\"name\":\"Course 1\",\"vicinity\":\"Address 1\",\"geometry\":{\"location\":{\"lat\":40.712776,\"lng\":-74.005974}}},{\"name\":\"Course 2\",\"vicinity\":\"Address 2\",\"geometry\":{\"location\":{\"lat\":40.712786,\"lng\":-74.005984}}},{\"name\":\"Course 3\",\"vicinity\":\"Address 3\",\"geometry\":{\"location\":{\"lat\":40.712796,\"lng\":-74.005994}}}]}")
                })
                .Verifiable();

            var client = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://maps.googleapis.com")
            };

            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var controller = new CourseController(mockHttpClientFactory.Object, _configuration);

            // Act
            var result = await controller.SearchNearestCourse(40.712776, -74.005974) as JsonResult;

            // Assert
            Assert.NotNull(result);

            // Parse directly to a JArray
            var resultsArray = JArray.FromObject(result.Value);
            Assert.NotNull(resultsArray);
            Assert.Equal(3, resultsArray.Count); // Assert that we have 3 results

            // Access elements within the array
            var firstResult = resultsArray.First as JObject;
            Assert.NotNull(firstResult);
            Assert.Equal("Course 1", firstResult["name"]);

            var secondResult = resultsArray[1] as JObject;
            Assert.NotNull(secondResult);
            Assert.Equal("Course 2", secondResult["name"]);

            var thirdResult = resultsArray[2] as JObject;
            Assert.NotNull(thirdResult);
            Assert.Equal("Course 3", thirdResult["name"]);
        }
    }
}
