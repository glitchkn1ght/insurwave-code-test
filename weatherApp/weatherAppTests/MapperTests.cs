//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// 16/01/2022 Ticket1 JS Team darkSaber - Added tests on tempInCelius param
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherAppTests
{
    using Newtonsoft.Json;
    using NUnit.Framework;
    using weatherApp.Models.Response;
    using weatherApp.Models.Weather;
    using weatherApp.Utility;
    using System.Net;
    using System.Net.Http;

    [TestFixture]
    public class MapperTests
    {
        private StandardSummaryMapper summaryMapper;

        private StandardErrorMapper errorMapper;

        private ErrorResponse error;

        private CommonTestData commonTestData;
          
        [SetUp]
        public void Setup()
        {
            this.error = new ErrorResponse();
            this.commonTestData = new CommonTestData();
            this.summaryMapper = new StandardSummaryMapper();
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

        [TestCase(1003, 400)]
        [TestCase(1005, 400)]
        [TestCase(1006, 400)]
        [TestCase(9999, 400)]
        [TestCase(1002, 401)]
        [TestCase(2006, 401)]
        [TestCase(2007, 403)]
        [TestCase(2008, 403)]
        [TestCase(1234, 500)]
        [TestCase(5, 500)]
        [TestCase(0, 500)]
        public void ApiCodesMapAsExpected(int apiCode, int expectedHttpCode)
        {
            
            this.error.Error = new Error
            {
                ApiCode = apiCode
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(this.error));

            this.errorMapper = new StandardErrorMapper();

            HttpResponseMessage msg = new HttpResponseMessage { Content = content };

            ErrorResponse actual =  this.errorMapper.MapError(msg,"resource").Result;

            Assert.AreEqual(expectedHttpCode, actual.Error.HttpStatusCode);
        }

        [TestCase(true)]
        public void WhenTempInCelciusTrue_ThenTemperatureIsReturnedInCelcius(bool tempInCelcius)
        {
            CurrentForecast fullForecast = this.commonTestData.GetValidCurrentForecast();

            CurrentForecastSummary forecastSummary = this.summaryMapper.mapSummaryResponse(fullForecast, tempInCelcius);

            Assert.AreEqual(fullForecast.CurrentConditions.Temperature_Celcius, forecastSummary.Temperature);
        }

        [TestCase(false)]
        public void WhenTempInCelciusTrueOrNull_ThenTemperatureIsReturnedInFahrenheit(bool tempInCelcius)
        {
            CurrentForecast fullForecast = this.commonTestData.GetValidCurrentForecast();

            CurrentForecastSummary forecastSummary = this.summaryMapper.mapSummaryResponse(fullForecast, tempInCelcius);

            Assert.AreEqual(fullForecast.CurrentConditions.Temperature_Fahrenheit, forecastSummary.Temperature);
        }
    }
}