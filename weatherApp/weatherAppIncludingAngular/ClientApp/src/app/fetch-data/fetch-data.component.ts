import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent
{
  public forecast: CurrentForecastAndAstronomySummary;
  http: HttpClient;
  baseURL: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {
    this.http = http;
    this.baseURL = baseUrl;
  }

  ngOnInit() {}
  onClickSubmit(weatherForm: WeatherFormOptions)
  {
    alert("You have entered : " + weatherForm);

    this.fetchWeatherData(weatherForm);
  }

  fetchWeatherData(weatherForm: WeatherFormOptions)
  {
    this.http.get<CurrentForecastAndAstronomySummary>(this.baseURL + 'weatherforecast/' + weatherForm.locationName + "?tempInCelcius=" + weatherForm.tempInCelcius + "&includeAstronomy=" + weatherForm.includeAstronomy).subscribe(result =>
      {
        this.forecast = result;
      }, error => console.error(error));
  }
}

interface WeatherFormOptions
{
  locationName : string;
  tempInCelcius: boolean
  includeAstronomy: boolean
}

interface CurrentForecastAndAstronomySummary
{
  currentForecastSummary: CurrentForecastSummary
  currentAstronomySummary : CurrentAstronomySummary
}

interface CurrentForecastSummary
{
  city: string;
  region: string;
  country: string;
  localTime: string;
  temperature: number;
}

interface CurrentAstronomySummary
{
  sunriseTime: string;
  sunsetTime: string;
}
