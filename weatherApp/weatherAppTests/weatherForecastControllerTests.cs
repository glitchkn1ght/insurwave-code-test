//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherAppTests
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using System;
    using System.Net;
    using System.Net.Http;
    using weatherApp.Controllers;
    using weatherApp.Models.Response;
    using weatherApp.Models.Weather;
    using weatherApp.Service;
    using weatherApp.Utility;

    [TestFixture]
    public class weatherForecastControllerTests
    {

        private Mock<ILogger<WeatherForecastController>> LoggerMock;

        private Mock<IForecastMapper> ForecastMapperMock;

        private Mock<IWeatherService> weatherServiceMock;

        private Mock<IErrorMapper> ErrorMapperMock; 

        private WeatherForecastController WeatherForecastController;

        [SetUp]
        public void Setup()
        {
            this.LoggerMock = new Mock<ILogger<WeatherForecastController>>();
            this.ForecastMapperMock = new Mock<IForecastMapper>();
            this.weatherServiceMock = new Mock<IWeatherService>();
            this.ErrorMapperMock = new Mock<IErrorMapper>();
        }

        public CurrentForecastSummary GetValidForecastSummary() 
        {
            return new CurrentForecastSummary
            {
                City = "London",
                Region = "City of London, Greater London",
                Country = "UK",
                LocalTime = "2022-01-12 21:45",
                Temperature = 31.1M
            };
        }

        public CurrentForecast GetValidCurrentForecast()
        {
            return new CurrentForecast
            {
                WeatherLocation = this.GetValidLocation(),
                CurrentConditions = GetValidCurrent()
            };
          
        }

        public Current GetValidCurrent()
        {
            return new Current
            {
                Last_Updated_Epoch = 1642023900,
                Last_Updated = DateTime.Parse("2022-01-12 21:45"),
                Temperature_Celcius = 4.0M,
                Temperature_Fahrenheit = 39.2M,
                Is_day = 0,
                Condition = GetValidCondition(),
                Wind_Mph = 0.0M,
                Wind_Kph = 0.0M,
                Wind_Degree = 278,
                Wind_Dir = "W",
                Pressure_mb = 1042.0M,
                Precipitation_in = 30.77M,
                Precipitation_mm = 0.0M,
                Pressure_in = 0.0M,
                Humidity = 93,
                Cloud = 0,
                Feelslike_Celcius = 3.8M,
                Feelslike_Fahrenheit = 38.8M,
                Visibility_Km = 9.0M,
                Visibility_Miles = 5.0M,
                UV = 1.0M,
                Gust_Mph = 3.6M,
                Gust_Kph = 5.8M
            };
        }

        public Location GetValidLocation()
        {
            return new Location
            {
                LocationName = "London",
                Region = "City of London, Greater London",
                Country = "United Kingdom",
                Latitude = 51.52,
                Longitude = -0.11,
                TimeZoneId = "Europe/London",
                LocalTime_Epoch = 1642024750,
                LocalTime = "2022-01-12 21:59"
            };
        }

        public Condition GetValidCondition()
        {
            return new Condition()
            {
                Text = "clear",
                Icon = "//cdn.weatherapi.com/weather/64x64/night/113.png",
                Code = 1000
            };
        }

        [Test]
        public void WhenConstructorCalledWithNullLogger_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("logger"), delegate
                {
                    this.WeatherForecastController = new WeatherForecastController(null, this.ForecastMapperMock.Object, this.weatherServiceMock.Object, this.ErrorMapperMock.Object);
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullForecastMapper_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("forecastMapper"), delegate
                {
                    this.WeatherForecastController = new WeatherForecastController(this.LoggerMock.Object, null, this.weatherServiceMock.Object, this.ErrorMapperMock.Object);
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullService_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("weatherService"), delegate
                {
                    this.WeatherForecastController = new WeatherForecastController(this.LoggerMock.Object, this.ForecastMapperMock.Object, null, this.ErrorMapperMock.Object);
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullErrorMapper_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("errorMapper"), delegate
                {
                    this.WeatherForecastController = new WeatherForecastController(this.LoggerMock.Object, this.ForecastMapperMock.Object, this.weatherServiceMock.Object, null);
                });
        }

        [Test]
        public void WhenConstructorCalledWithValidArguements_ThenNoExceptionThrown()
        {
            Assert.DoesNotThrow(
                delegate
                {
                    this.WeatherForecastController = new WeatherForecastController(this.LoggerMock.Object, this.ForecastMapperMock.Object, this.weatherServiceMock.Object, this.ErrorMapperMock.Object);
                });
        }


        [Test]
        public void WhenWeatherServiceReturnsWithValidData_ThenGetReturn200OK()
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(GetValidCurrentForecast()));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content });

            this.ForecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>())).Returns(GetValidForecastSummary());

            this.WeatherForecastController = new WeatherForecastController(this.LoggerMock.Object, this.ForecastMapperMock.Object, this.weatherServiceMock.Object, this.ErrorMapperMock.Object);

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london").Result;

            Assert.AreEqual(200, actual.StatusCode);
        }

        [TestCase(9999)]
        public void WhenWeatherServiceReturnsError_ThenGetReturnsError(int apiErrorCode)
        {

            ErrorDetails errordetails = new ErrorDetails
            {
                error = new Error
                {
                    apiCode = apiErrorCode
                }
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(errordetails));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = content });

            this.ErrorMapperMock.Setup(x => x.MapApiErrorCode(It.IsAny<int>())).Returns(400);

            this.WeatherForecastController = new WeatherForecastController(this.LoggerMock.Object, this.ForecastMapperMock.Object, this.weatherServiceMock.Object, this.ErrorMapperMock.Object);

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london").Result;

            Assert.AreEqual(400, actual.StatusCode);
        }

        [Test]
        public void WhenExceptiontThrown_ThenGetReturn500InternalServerError()
        {
            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).Throws(new Exception());

            this.WeatherForecastController = new WeatherForecastController(this.LoggerMock.Object, this.ForecastMapperMock.Object, this.weatherServiceMock.Object, this.ErrorMapperMock.Object);

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get(It.IsAny<string>()).Result;

            Assert.AreEqual(500, actual.StatusCode);
        }
    }
}