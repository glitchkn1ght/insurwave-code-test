//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// 16/01/2022 Ticket2 JS Team darkSaber - Updated tests with new tempInCelcius parameter
// 16/01/2022 Ticket3 JS Team darkSaber - Refactored tests to take astronomy mapper into account.
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

        private Mock<IForecastMapper> forecastMapperMock;

        private Mock<IAstronomyMapper> astronomyMapperMock;

        private Mock<IWeatherService> weatherServiceMock;

        private Mock<IErrorMapper> errorMapperMock; 

        private WeatherForecastController weatherForecastController;

        private StandardForecastMapper forecastMapper;

        private StandardSummaryMapper summaryMapper;

        private CommonTestData commonTestData;

        [SetUp]
        public void Setup()
        {
            this.LoggerMock = new Mock<ILogger<WeatherForecastController>>();
            this.forecastMapperMock = new Mock<IForecastMapper>();
            this.astronomyMapperMock = new Mock<IAstronomyMapper>();
            this.weatherServiceMock = new Mock<IWeatherService>();
            this.errorMapperMock = new Mock<IErrorMapper>();
            this.commonTestData = new CommonTestData();
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
        public void WhenBothAPIRequestsReturnWithValidData_ThenGetReturns200OK()
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
        }

        [Test]
        public void WhenCalledWithNullTempInCelciusParam_ThenGetReturn200OK()
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

            ObjectResult actual = (ObjectResult)this.weatherForecastController.Get(It.IsAny<string>(), It.IsAny<string>(), null).Result;

            Assert.AreEqual(200, actual.StatusCode);
        }


        [Test]
        public void WhenCalledWithNullLocationDTParam_ThenGetReturnsForecastSummaryDataOnly()
        {
            HttpContent weatherContent = new StringContent(JsonConvert.SerializeObject(this.commonTestData.GetValidCurrentForecast()));

            this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = weatherContent });

            this.forecastMapperMock.Setup(x => x.mapWeatherAPIResponse(It.IsAny<string>(), It.IsAny<bool>())).Returns(this.commonTestData.GetValidForecastSummary());

            this.weatherForecastController = new WeatherForecastController
              (
                this.LoggerMock.Object,
                this.forecastMapperMock.Object,
                this.astronomyMapperMock.Object,
                this.weatherServiceMock.Object,
                this.errorMapperMock.Object
              );

            ObjectResult actual = (ObjectResult)this.weatherForecastController.Get(It.IsAny<string>(), null, null).Result;

            Assert.AreEqual(200, actual.StatusCode);
            Assert.IsInstanceOf<CurrentForecastSummary>(actual.Value);
        }


        //[TestCase(1003)]
        //[TestCase(1005)]
        //[TestCase(1006)]
        //[TestCase(9999)]
        //public void WhenWeatherServiceReturnsTheseErrorCodes_ThenGetReturn400BadRequest(int apiErrorCode)
        //{

        //    ErrorDetails errordetails = new ErrorDetails
        //    {
        //        error = new Error
        //        {
        //            apiCode = apiErrorCode
        //        }
        //    };

        //    HttpContent content = new StringContent(JsonConvert.SerializeObject(errordetails));

        //    this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = content });

        //    this.errorMapperMock.Setup(x => x.MapApiErrorCode(It.IsAny<int>())).Returns(400);

        //    this.weatherForecastController = new WeatherForecastController(this.LoggerMock.Object, this.forecastMapperMock.Object, this.astronomyMapperMock.Object, this.weatherServiceMock.Object, this.errorMapperMock.Object);

        //    ObjectResult actual = (ObjectResult)this.weatherForecastController.Get(It.IsAny<string>(), It.IsAny<bool>()).Result;

        //    Assert.AreEqual(400, actual.StatusCode);
        //}

        //[TestCase(2006)]
        //[TestCase(1002)]
        //public void WhenWeatherServiceReturnsTheseErrorCodes_ThenGetReturn401Unauthorized(int apiErrorCode)
        //{

        //    ErrorDetails errordetails = new ErrorDetails
        //    {
        //        error = new Error
        //        {
        //            apiCode = apiErrorCode
        //        }
        //    };

        //    HttpContent content = new StringContent(JsonConvert.SerializeObject(errordetails));

        //    this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = content });

        //    this.errorMapperMock.Setup(x => x.MapApiErrorCode(It.IsAny<int>())).Returns(401);

        //    this.weatherForecastController = new WeatherForecastController(this.LoggerMock.Object, this.forecastMapperMock.Object, this.weatherServiceMock.Object, this.errorMapperMock.Object);

      //      ObjectResult actual = (ObjectResult)this.weatherForecastController.Get(It.IsAny<string>(), It.IsAny<bool>()).Result;

        //    Assert.AreEqual(401, actual.StatusCode);
        //}

        //[TestCase(2008)]
        //[TestCase(2007)]
        //public void WhenWeatherServiceReturnsTheseErrorCodes_ThenGetReturn403Forbidden(int apiErrorCode)
        //{

        //    ErrorDetails errordetails = new ErrorDetails
        //    {
        //        error = new Error
        //        {
        //            apiCode = apiErrorCode
        //        }
        //    };

        //    HttpContent content = new StringContent(JsonConvert.SerializeObject(errordetails));

        //    this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = content });

        //    this.errorMapperMock.Setup(x => x.MapApiErrorCode(It.IsAny<int>())).Returns(403);

        //    this.weatherForecastController = new WeatherForecastController(this.LoggerMock.Object, this.forecastMapperMock.Object, this.weatherServiceMock.Object, this.errorMapperMock.Object);

         //   ObjectResult actual = (ObjectResult)this.weatherForecastController.Get(It.IsAny<string>(), It.IsAny<bool>()).Result;

        //    Assert.AreEqual(403, actual.StatusCode);
        //}

        //[Test]
        //public void WhenExceptiontThrown_ThenGetReturn500InternalServerError()
        //{
        //    this.weatherServiceMock.Setup(x => x.GetCurrentConditions(It.IsAny<string>())).Throws(new Exception());

        //    this.weatherForecastController = new WeatherForecastController(this.LoggerMock.Object, this.forecastMapperMock.Object, this.weatherServiceMock.Object, this.errorMapperMock.Object);

       //     ObjectResult actual = (ObjectResult)this.weatherForecastController.Get(It.IsAny<string>(), It.IsAny<bool>()).Result;

        //    Assert.AreEqual(500, actual.StatusCode);
        //}
    }
}