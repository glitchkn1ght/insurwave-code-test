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
  onClickSubmit(result)
  {
    alert("You have entered : " + result.locationName);

    this.fetchWeatherData(result.locationName);
  }

  fetchWeatherData(location: string)
  {
    this.http.get<CurrentForecastAndAstronomySummary>(this.baseURL + 'weatherforecast/' + location).subscribe(result =>
      {
        this.forecast = result;
      }, error => console.error(error));
  }
}

interface CurrentForecastAndAstronomySummary
{
  CurrentForecastSummary: CurrentForecastSummary
  CurrentAstronomySummary : CurrentAstronomySummary
}

interface CurrentForecastSummary
{
  city: string;
  region: number;
  country: string;
  localTime: string;
  temperature: string;
}

interface CurrentAstronomySummary
{
  sunriseTime: string;
  sunsetTime: string;
}
