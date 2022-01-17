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
    public class weatherForecastControllerTests
    {

        private Mock<ILogger<WeatherForecastController>> LoggerMock;

        private Mock<IForecastMapper> ForecastMapperMock;

        private Mock<IAstronomyMapper> astronomyMapperMock;

        private Mock<IWeatherService> weatherServiceMock;

        private Mock<IErrorMapper> ErrorMapperMock; 

        private WeatherForecastController WeatherForecastController;

        private StandardForecastMapper forecastMapper;

        private StandardSummaryMapper summaryMapper;

        private StandardErrorMapper errorMapper;

        private CommonTestData commonTestData;

        private ErrorDetails errorDetails;

        [SetUp]
        public void Setup()
        {
            this.LoggerMock = new Mock<ILogger<WeatherForecastController>>();
            this.forecastMapperMock = new Mock<IForecastMapper>();
            this.astronomyMapperMock = new Mock<IAstronomyMapper>();
            this.weatherServiceMock = new Mock<IWeatherService>();
            this.errorMapperMock = new Mock<IErrorMapper>();
            this.commonTestData = new CommonTestData();

            this.errorMapper = new StandardErrorMapper();
        }

        [Test]
        public void WhenConstructorCalledWithNullLogger_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("logger"), delegate
                {
                    this.weatherForecastController = new WeatherForecastController
                    (
                        null, 
                        this.forecastMapperMock.Object, 
                        this.astronomyMapperMock.Object, 
                        this.weatherServiceMock.Object, 
                        this.errorMapperMock.Object
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullForecastMapper_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("forecastMapper"), delegate
                {
                    this.weatherForecastController = new WeatherForecastController
                    (
                        this.LoggerMock.Object,
                        null,
                        this.astronomyMapperMock.Object,
                        this.weatherServiceMock.Object,
                        this.errorMapperMock.Object
                    ) ;
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullAstronomyMapper_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("astronomyMapper"), delegate
                {
                    this.weatherForecastController = new WeatherForecastController
                    (
                        this.LoggerMock.Object,
                        this.forecastMapperMock.Object,
                        null,
                        this.weatherServiceMock.Object,
                        this.errorMapperMock.Object
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullService_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("weatherService"), delegate
                {
                    this.weatherForecastController = new WeatherForecastController
                    (
                        this.LoggerMock.Object,
                        this.forecastMapperMock.Object,
                        this.astronomyMapperMock.Object,
                        null,
                        this.errorMapperMock.Object
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullErrorMapper_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("errorMapper"), delegate
                {
                    this.weatherForecastController = new WeatherForecastController
                    (
                        this.LoggerMock.Object, 
                        this.forecastMapperMock.Object,
                        this.astronomyMapperMock.Object,
                        this.weatherServiceMock.Object, 
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
                    this.weatherForecastController = new WeatherForecastController
                    (
                        this.LoggerMock.Object,
                        this.forecastMapperMock.Object,
                        this.astronomyMapperMock.Object,
                        this.weatherServiceMock.Object,
                        this.errorMapperMock.Object
                    );
                });
        }

        [Test]
        public void WhenNullTempInCelciusParam_ThenGetReturn200OK_WithFullSummary()
        {
            HttpContent weatherContent = new StringContent(JsonConvert.SerializeObject(this.commonTestData.GetValidCurrentForecast()));

            HttpContent astronomyContent = new StringContent(JsonConvert.SerializeObject(this.commonTestData.GetValidCurrentAstronomy()));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = weatherContent });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions("london", "2022-01-17")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = astronomyContent });

            this.forecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), It.IsAny<bool>())).Returns(this.commonTestData.GetValidForecastSummary());

            this.weatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.forecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.errorMapperMock.Object
              );

            ObjectResult actual = (ObjectResult)this.weatherForecastController.Get("london", "2022-01-17", null).Result;

            Assert.AreEqual(200, actual.StatusCode);
            Assert.IsInstanceOf<CurrentForecastAndAstronomySummary>(actual.Value);
        }

        [Test]
        public void WhenNullLocationDTParamAndWeatherServiceOk_ThenGetReturnsForecastSummaryDataOnly()
        {
            HttpContent weatherContent = new StringContent(JsonConvert.SerializeObject(this.commonTestData.GetValidCurrentForecast()));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = weatherContent });

            this.forecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), It.IsAny<bool>())).Returns(this.commonTestData.GetValidForecastSummary());

            this.weatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.forecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.errorMapperMock.Object
              );

            ObjectResult actual = (ObjectResult)this.weatherForecastController.Get("london", null, null).Result;

            Assert.AreEqual(200, actual.StatusCode);
            Assert.IsInstanceOf<CurrentForecastSummary>(actual.Value);
        }

        [Test]
        public void WhenNullLocationDTParamAndWeatherServiceNotOk_ThenGetReturnsError()
        {
            ErrorDetails errorDetails = new ErrorDetails
            {
                error = new Error
                {
                    apiCode = 1002
                }
           };
            
            HttpContent weatherContent = new StringContent(JsonConvert.SerializeObject(errorDetails));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = weatherContent });

            this.weatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.forecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.errorMapper
              );

            ObjectResult actual = (ObjectResult)this.weatherForecastController.Get(It.IsAny<string>(), null, null).Result;

            Assert.AreEqual(401, actual.StatusCode);
        }

        [Test]
        public void WhenBothRequestsReturnOK_ThenGetReturns200OK_WithFullSummary()
        {

            HttpContent weatherContent = new StringContent(JsonConvert.SerializeObject(this.commonTestData.GetValidCurrentForecast()));

            HttpContent astronomyContent = new StringContent(JsonConvert.SerializeObject(this.commonTestData.GetValidCurrentAstronomy()));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = weatherContent });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = astronomyContent });

            this.forecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), It.IsAny<bool>())).Returns(this.commonTestData.GetValidForecastSummary());

            this.weatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.forecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.errorMapperMock.Object
              );

            ObjectResult actual = (ObjectResult)this.weatherForecastController.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()).Result;

            Assert.AreEqual(200, actual.StatusCode);
            Assert.IsInstanceOf<CurrentForecastAndAstronomySummary>(actual.Value);
        }

        [Test]
        public void WhenBothRequestsReturnWithErrorCodes_ThenGetReturns207MultiStatus()
        {

            ErrorDetails errorDetailsWeather = new ErrorDetails
            {
                error = new Error
                {
                    apiCode = 1002
                }
            };

            ErrorDetails errorDetailsAstronomy = new ErrorDetails
            {
                error = new Error
                {
                    apiCode = 1006
                }
            };

            HttpContent weatherContent = new StringContent(JsonConvert.SerializeObject(errorDetailsWeather));

            HttpContent astronomyContent = new StringContent(JsonConvert.SerializeObject(errorDetailsAstronomy));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = weatherContent });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = astronomyContent });

            this.forecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), It.IsAny<bool>())).Returns(this.commonTestData.GetValidForecastSummary());

            this.weatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.forecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.errorMapper
              );

            ObjectResult actual = (ObjectResult)this.weatherForecastController.Get(It.IsAny<string>(),"2022-09-01", It.IsAny<bool>()).Result;

            Assert.AreEqual(207, actual.StatusCode);

        }

        [Test]
        public void WhenWeatherRequestSuccess_AndAstronomyRequestNonSuccess_ThenGetReturnsError()
        {
            HttpContent weatherContent = new StringContent(JsonConvert.SerializeObject(this.commonTestData.GetValidCurrentForecast()));

            ErrorDetails errorDetailsAstronomy = new ErrorDetails
            {
                error = new Error
                {
                    apiCode = 1006
                }
            };

            HttpContent astronomyContent = new StringContent(JsonConvert.SerializeObject(errorDetailsAstronomy));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = weatherContent });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = astronomyContent });

            this.forecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), It.IsAny<bool>())).Returns(this.commonTestData.GetValidForecastSummary());

            this.weatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.forecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.errorMapper
              );

            ObjectResult actual = (ObjectResult)this.weatherForecastController.Get(It.IsAny<string>(), "2022-09-01", It.IsAny<bool>()).Result;

            Assert.AreEqual(400, actual.StatusCode);
        }

        [Test]
        public void WhenWeatherRequestNonSuccess_AndAstronomyRequestSucceess_ThenGetReturnsError()
        {
            ErrorDetails errorDetailsWeather = new ErrorDetails
            {
                error = new Error
                {
                    apiCode = 1002
                }
            };

            HttpContent weatherContent = new StringContent(JsonConvert.SerializeObject(errorDetailsWeather));

            HttpContent astronomyContent = new StringContent(JsonConvert.SerializeObject(this.commonTestData.GetValidCurrentAstronomy()));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = weatherContent });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions(It.IsAny<string>(), "2022-09-01")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = astronomyContent });

            this.forecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), It.IsAny<bool>())).Returns(this.commonTestData.GetValidForecastSummary());

            this.weatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.forecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.errorMapper
              );

            ObjectResult actual = (ObjectResult)this.weatherForecastController.Get(It.IsAny<string>(), "2022-09-01", It.IsAny<bool>()).Result;

            Assert.AreEqual(401, actual.StatusCode);
        }

        [Test]
        public void WhenExceptiontThrown_ThenGetReturn500InternalServerError()
        {
            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).Throws(new Exception());

            this.weatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.forecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.errorMapper
              );

            ObjectResult actual = (ObjectResult)this.weatherForecastController.Get(It.IsAny<string>(), "2022-09-01", It.IsAny<bool>()).Result;

            Assert.AreEqual(500, actual.StatusCode);
        }
    }
}