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

    [TestFixture]
    public class MapperTests
    {
        private StandardSummaryMapper summaryMapper;

        private StandardErrorMapper errorMapper;

        private ErrorDetails errorDetails;

        private CommonTestData commonTestData;
          
        [SetUp]
        public void Setup()
        {
            this.errorDetails = new ErrorDetails();
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

        [TestCase(1002, 401)]
        [TestCase(2006, 401)]
        public void TheseApiCodes_MapTo401Unauthorized(int apiCode, int expectedHttpCode)
        {
            this.errorMapper = new StandardErrorMapper();

            int actual = this.errorMapper.MapApiErrorCode(apiCode);

            Assert.AreEqual(expectedHttpCode, actual);
        }

        [TestCase(1003, 400)]
        [TestCase(1005, 400)]
        [TestCase(1006, 400)]
        [TestCase(9999, 400)]
        public void TheseApiCodes_MapTo400BadRequest(int apiCode, int expectedHttpCode)
        {
            this.errorMapper = new StandardErrorMapper();

            int actual = this.errorMapper.MapApiErrorCode(apiCode);

            Assert.AreEqual(expectedHttpCode, actual);
        }

        [TestCase(2007, 403)]
        [TestCase(2008, 403)]
        public void TheseApiCodes_MapTo403Forbidden(int apiCode, int expectedHttpCode)
        {
            this.errorMapper = new StandardErrorMapper();

            int actual = this.errorMapper.MapApiErrorCode(apiCode);

            Assert.AreEqual(expectedHttpCode, actual);
        }


        [TestCase(1234, 500)]
        [TestCase(5, 500)]
        [TestCase(0, 500)]
        public void ApiCodesMapAsExpected(int apiCode, int expectedHttpCode)
        {
            this.errorDetails.error = new Error
        {
                apiCode = apiCode
            };

            string content = JsonConvert.SerializeObject(this.errorDetails);

            this.errorMapper = new StandardErrorMapper();

            ErrorDetails actual = this.errorMapper.MapErrorDetails(content,"resource");

            Assert.AreEqual(expectedHttpCode, actual.error.HttpStatusCode);
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