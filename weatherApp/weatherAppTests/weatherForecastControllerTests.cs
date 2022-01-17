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

        private WeatherForecastController WeatherForecastController;

        private StandardErrorMapper ErrorMapper;

        private CommonTestData CommonTestData;


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
                        this.ForecastMapperMock.Object, 
                        this.astronomyMapperMock.Object, 
                        this.weatherServiceMock.Object, 
                        this.ErrorMapperMock.Object
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullForecastMapper_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("forecastMapper"), delegate
                {
                    this.WeatherForecastController = new WeatherForecastController
                    (
                        this.LoggerMock.Object,
                        null,
                        this.astronomyMapperMock.Object,
                        this.weatherServiceMock.Object,
                        this.ErrorMapperMock.Object
                    ) ;
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullAstronomyMapper_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("astronomyMapper"), delegate
                {
                    this.WeatherForecastController = new WeatherForecastController
                    (
                        this.LoggerMock.Object,
                        this.ForecastMapperMock.Object,
                        null,
                        this.weatherServiceMock.Object,
                        this.ErrorMapperMock.Object
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
                        this.ForecastMapperMock.Object,
                        this.astronomyMapperMock.Object,
                        null,
                        this.ErrorMapperMock.Object
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullErrorMapper_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("errorMapper"), delegate
                {
                    this.WeatherForecastController = new WeatherForecastController
                    (
                        this.LoggerMock.Object, 
                        this.ForecastMapperMock.Object,
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
                    this.WeatherForecastController = new WeatherForecastController
                    (
                        this.LoggerMock.Object,
                        this.ForecastMapperMock.Object,
                        this.astronomyMapperMock.Object,
                        this.weatherServiceMock.Object,
                        this.ErrorMapperMock.Object
                    );
                });
        }

        [Test]
        public void WhenNullTempInCelciusParam_ThenGetReturn200OK_WithFullSummary()
        {
            HttpContent weatherContent = new StringContent(JsonConvert.SerializeObject(this.CommonTestData.GetValidCurrentForecast()));

            HttpContent astronomyContent = new StringContent(JsonConvert.SerializeObject(this.CommonTestData.GetValidCurrentAstronomy()));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = weatherContent });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions("london", "2022-01-17")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = astronomyContent });

            this.ForecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), It.IsAny<bool>())).Returns(this.CommonTestData.GetValidForecastSummary());

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.ForecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.ErrorMapperMock.Object
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", "2022-01-17", null).Result;

            Assert.AreEqual(200, actual.StatusCode);
            Assert.IsInstanceOf<CurrentForecastAndAstronomySummary>(actual.Value);
        }

        [Test]
        public void WhenNullLocationDTParamAndWeatherServiceOk_ThenGetReturnsForecastSummaryDataOnly()
        {
            HttpContent weatherContent = new StringContent(JsonConvert.SerializeObject(this.CommonTestData.GetValidCurrentForecast()));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = weatherContent });

            this.ForecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), It.IsAny<bool>())).Returns(this.CommonTestData.GetValidForecastSummary());

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.ForecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.ErrorMapperMock.Object
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", null, true).Result;

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

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.ForecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.ErrorMapper
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get(It.IsAny<string>(), null, null).Result;

            Assert.AreEqual(401, actual.StatusCode);
        }

        [Test]
        public void WhenBothRequestsReturnOK_ThenGetReturns200OK_WithFullSummary()
        {

            HttpContent weatherContent = new StringContent(JsonConvert.SerializeObject(this.CommonTestData.GetValidCurrentForecast()));

            HttpContent astronomyContent = new StringContent(JsonConvert.SerializeObject(this.CommonTestData.GetValidCurrentAstronomy()));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = weatherContent });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions("london", "2022-01-17")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = astronomyContent });

            this.ForecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), true)).Returns(this.CommonTestData.GetValidForecastSummary());

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.ForecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.ErrorMapperMock.Object
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", "2022-01-17", It.IsAny<bool>()).Result;

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

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = weatherContent });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions("london","2022-01-17")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = astronomyContent });

            this.ForecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), It.IsAny<bool>())).Returns(this.CommonTestData.GetValidForecastSummary());

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.ForecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.ErrorMapper
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", "2022-01-17", It.IsAny<bool>()).Result;

            Assert.AreEqual(207, actual.StatusCode);

        }

        [Test]
        public void WhenWeatherRequestSuccess_AndAstronomyRequestNonSuccess_ThenGetReturnsError()
        {
            HttpContent weatherContent = new StringContent(JsonConvert.SerializeObject(this.CommonTestData.GetValidCurrentForecast()));

            ErrorDetails errorDetailsAstronomy = new ErrorDetails
            {
                error = new Error
                {
                    apiCode = 1006
                }
            };

            HttpContent astronomyContent = new StringContent(JsonConvert.SerializeObject(errorDetailsAstronomy));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = weatherContent });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions("london", "2022-01-17")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = astronomyContent });

            this.ForecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), It.IsAny<bool>())).Returns(this.CommonTestData.GetValidForecastSummary());

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.ForecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.ErrorMapper
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", "2022-01-17", true).Result;

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

            HttpContent astronomyContent = new StringContent(JsonConvert.SerializeObject(this.CommonTestData.GetValidCurrentAstronomy()));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions("london")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = weatherContent });

            this.weatherServiceMock.Setup(x => x.GetAstronomyConditions("london", "2022-01-17")).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = astronomyContent });

            this.ForecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), It.IsAny<bool>())).Returns(this.CommonTestData.GetValidForecastSummary());

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.ForecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.ErrorMapper
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", "2022-01-17", It.IsAny<bool>()).Result;

            Assert.AreEqual(401, actual.StatusCode);
        }

        [Test]
        public void WhenExceptiontThrown_ThenGetReturn500InternalServerError()
        {
            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).Throws(new Exception());

            this.WeatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.ForecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.ErrorMapper
              );

            ObjectResult actual = (ObjectResult)this.WeatherForecastController.Get("london", "2022-01-17", It.IsAny<bool>()).Result;

            Assert.AreEqual(500, actual.StatusCode);
        }
    }
}