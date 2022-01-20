//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// 16/01/2022 Ticket2 JS Team darkSaber - Updated tests with new tempInCelcius parameter
// 16/01/2022 Ticket3 JS Team darkSaber - Refactored tests to take astronomy mapper into account.
// 16/01/2022 Ticket1 JS Team darkSaber - Updated tests with new tempInCelcius parameter
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherAppTests
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Http;
    using weatherApp.Controllers;
    using weatherApp.Models.Response;
    using weatherApp.Models.Weather;
    using weatherApp.Service;
    using weatherApp.Utility;

    [TestFixture]
    public class WeatherForecastControllerTests
    {

        private Mock<ILogger<WeatherForecastController>> LoggerMock;

        private Mock<IForecastMapper> ForecastMapperMock;

        private Mock<IAstronomyMapper> astronomyMapperMock;

        private Mock<IWeatherService> weatherServiceMock;

        private Mock<IErrorMapper> ErrorMapperMock; 

        private StandardErrorMapper ErrorMapper;

        private CommonTestData CommonTestData;

        private WeatherForecastController WeatherForecastController;

        [SetUp]
        public void Setup()
        {
            this.LoggerMock = new Mock<ILogger<WeatherForecastController>>();
            this.ForecastMapperMock = new Mock<IForecastMapper>();
            this.astronomyMapperMock = new Mock<IAstronomyMapper>();
            this.weatherServiceMock = new Mock<IWeatherService>();
            this.ErrorMapperMock = new Mock<IErrorMapper>();
            this.CommonTestData = new CommonTestData();

            this.ErrorMapper = new StandardErrorMapper();
        }

        [Test]
        public void WhenConstructorCalledWithNullLogger_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("logger"), delegate
                {
                    this.WeatherForecastController = new WeatherForecastController
                    (
                        null, 
                        this.weatherServiceMock.Object
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullService_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("weatherService"), delegate
                {
                    this.WeatherForecastController = new WeatherForecastController
                    (
                        this.LoggerMock.Object,
                        null
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithValidArguements_ThenNoExceptionThrown()
        {
            Assert.DoesNotThrow(
                delegate
                {
                    this.WeatherForecastController = new WeatherForecastController
                    (
                        this.LoggerMock.Object,
                        this.weatherServiceMock.Object
                    );
                });
        }

        [Test]
        public void WhenNullTempInCelciusParam_ThenGetReturn200OK_WithFullSummary()
        {
            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london", true)).ReturnsAsync(new ForecastResponse{ IsSuccess = true, forecastSummary = this.CommonTestData.GetValidForecastSummary()});

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions("london")).ReturnsAsync(new AstronomyResponse { IsSuccess = true, astronomySummary = this.CommonTestData.GetValidCurrentAstronomySummary()});

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.weatherServiceMock.Object
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london",null ,true).Result;

            Assert.AreEqual(200, actual.StatusCode);
            Assert.IsInstanceOf<CurrentForecastAndAstronomySummary>(actual.Value);
        }

        [Test]
        public void WhenNullAstronomyParamAndWeatherServiceOk_ThenGetReturnsForecastSummaryDataOnly()
        {
            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london", true)).ReturnsAsync( new ForecastResponse { IsSuccess = true, forecastSummary = this.CommonTestData.GetValidForecastSummary() } );

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.weatherServiceMock.Object
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", true, null).Result;

            Assert.AreEqual(200, actual.StatusCode);
            Assert.IsInstanceOf<CurrentForecastSummary>(actual.Value);
        }

        [Test]
        public void WhenNullAstronomyParamAndWeatherServiceNotOk_ThenGetReturnsError()
        {
            Error error = new Error
            {
                ErrorDetails = new ErrorDetails
                {
                    HttpStatusCode = 401
                }
           };
            
            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>(), true)).ReturnsAsync(new ForecastResponse { Error = error });

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.weatherServiceMock.Object
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get(It.IsAny<string>(), null, null).Result;

            Assert.AreEqual(401, actual.StatusCode);
        }

        [Test]
        public void WhenBothRequestsReturnOK_ThenGetReturns200OK_WithFullSummary()
        {
            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london", true)).ReturnsAsync(new ForecastResponse { IsSuccess = true, forecastSummary = this.CommonTestData.GetValidForecastSummary() });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions("london")).ReturnsAsync(new AstronomyResponse { IsSuccess = true, astronomySummary = this.CommonTestData.GetValidCurrentAstronomySummary() });

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.weatherServiceMock.Object
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", true, true).Result;

            Assert.AreEqual(200, actual.StatusCode);
            Assert.IsInstanceOf<CurrentForecastAndAstronomySummary>(actual.Value);
        }

        [Test]
        public void WhenBothRequestsReturnWithErrorCodes_ThenGetReturns207MultiStatus()
        {

            Error weatherError = new Error
            {
                ErrorDetails = new ErrorDetails
                {
                    HttpStatusCode = 401
                }
            };

            Error astronomyError = new Error
            {
                ErrorDetails = new ErrorDetails
                {
                   HttpStatusCode = 401
                }
            };


            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london", true)).ReturnsAsync(new ForecastResponse { IsSuccess = false, Error = weatherError });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions("london")).ReturnsAsync(new AstronomyResponse { IsSuccess = false, Error = astronomyError });

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.weatherServiceMock.Object
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", true, true).Result;

            Assert.AreEqual(207, actual.StatusCode);

        }

        [Test]
        public void WhenWeatherRequestSuccess_AndAstronomyRequestNonSuccess_ThenGetReturns207MultiStatus()
        {
           
            Error astronomyError = new Error
            {
                ErrorDetails = new ErrorDetails
                {
                    HttpStatusCode = 401
                }
            };

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london", true)).ReturnsAsync(new ForecastResponse { IsSuccess = true, forecastSummary = this.CommonTestData.GetValidForecastSummary() });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions("london")).ReturnsAsync(new AstronomyResponse { IsSuccess = false, Error = astronomyError });

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.weatherServiceMock.Object
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", true, true).Result;

            Assert.AreEqual(207, actual.StatusCode);
        }

        [Test]
        public void WhenWeatherRequestNonSuccess_AndAstronomyRequestSucceess_ThenGetReturns207MultiStatus()
        { 
            
            Error weatherError = new Error
            {
                ErrorDetails = new ErrorDetails
                {
                    HttpStatusCode = 401
                }
            };

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london",true)).ReturnsAsync(new ForecastResponse { IsSuccess = false, Error = weatherError });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions("london")).ReturnsAsync(new AstronomyResponse { IsSuccess = true, astronomySummary = this.CommonTestData.GetValidCurrentAstronomySummary() });

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.weatherServiceMock.Object
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", true, true).Result;

            Assert.AreEqual(207, actual.StatusCode);
        }

        [Test]
        public void WhenExceptiontThrown_ThenGetReturn500InternalServerError()
        {
            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>(),true)).Throws(new Exception());

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.weatherServiceMock.Object
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", true, true).Result;

            Assert.AreEqual(500, actual.StatusCode);
        }
    }
}