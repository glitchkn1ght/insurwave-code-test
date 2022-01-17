//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 16/01/2022 Ticket3 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherAppTests
{
    using NUnit.Framework;
    using System;
    using weatherApp.Models.Weather;

    [TestFixture]
    public class CommonTestData
    {

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

        public Astronomy GetValidAstronomy()
        {
            return new Astronomy
            {
                astro = new Astro
                {
                    SunriseTime = "08:00AM",
                    SunsetTime = "04:19PM",
                    MoonRiseTime = "01:10 PM",
                    MoonSetTime = "05:22 AM",
                    MoonPhase = "Waxing Gibbous",
                    MoonIllumination = "77"
                }
            };
        }

        public CurrentAstronomy GetValidCurrentAstronomy()
        {
            return new CurrentAstronomy
            {
                location = GetValidLocation(),
                astronomy = GetValidAstronomy()
            };
        }
    }
}