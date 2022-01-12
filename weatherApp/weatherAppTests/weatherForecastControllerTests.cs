//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherAppTests
{
    using NUnit.Framework;
    using Moq;
    using weatherApp;
    using weatherApp.Controllers;
    using weatherApp.Models.Weather;
    using weatherApp.Service;
    using weatherApp.Models.Configuration;
    using weatherApp.Models;
    using Microsoft.Extensions.Logging;
    using weatherApp.Utility;
    using System;

    [TestFixture]
    public class weatherForecastControllerTests
    {

        private Mock<ILogger<WeatherForecastController>> LoggerMock;

        private Mock<IForecastMapper> forecastMapperMock;

        private Mock<IWeatherService> weatherServiceMock;

        private WeatherForecastController weatherForecastController;

        [SetUp]
        public void Setup()
        {
            this.LoggerMock = new Mock<ILogger<WeatherForecastController>>();
            this.forecastMapperMock = new Mock<IForecastMapper>();
            this.weatherServiceMock = new Mock<IWeatherService>();
        }

        [Test]
        public void WhenConstructorCalledWithNullLogger_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("logger"), delegate
                {
                    this.weatherForecastController = new WeatherForecastController(null, this.forecastMapperMock.Object, this.weatherServiceMock.Object);
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullForecastMapper_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("forecastMapper"), delegate
                {
                    this.weatherForecastController = new WeatherForecastController(this.LoggerMock.Object, null, this.weatherServiceMock.Object);
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullService_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("weatherService"), delegate
                {
                    this.weatherForecastController = new WeatherForecastController(this.LoggerMock.Object, this.forecastMapperMock.Object, null);
                });
        }

        [Test]
        public void WhenConstructorCalledWithValidArguements_ThenNoExceptionThrown()
        {
            Assert.DoesNotThrow(
                delegate
                {
                    this.weatherForecastController = new WeatherForecastController(this.LoggerMock.Object, this.forecastMapperMock.Object, this.weatherServiceMock.Object);
                });
        }

    }
}