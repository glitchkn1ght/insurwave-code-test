# The Insurwave code test

James To Do
- Handling front end errors
- Make form prettier
- Input Validation

## Introduction

Let's imagine a ship operating company having vessels traveling across the globe. The operators located in one of the headquarters are responsible for communication with vessel captains during their journey.  
To make their work easier they need to understand what is the local time at the ports where vessels stay as well as local weather conditions.

With this exercise, you will build an ASP.NET Core Web Api allowing the system to provide that information.

The produced solution should:
* build with `dotnet build` command,
* run with `dotnet run` command and start the local, console instance of the webapi,
* contain all the necessary tests (unit tests and integration tests) runnable with `dotnet test` command,
* have a documented api showing available operation and the requests/response models.

During writing the service, please include all the good practices that would be normally used during the development of the service.

For obtaining the weather/time information, please integrate with https://www.weatherapi.com/ using a free account.

For the implementation, please follow the tickets written below.  
For every ticket, please prepare a separate commit/pull-request showing incremental work.  
Assuming every ticket is implemented and released independently, the changes should be implemented in a non-breaking manner.

During the interview session, all of the tickets will be demoed, reviewed and discussed.

## Ticket-1

As an operator,  
I want to know what is the local time and temperature in the specified city,  
So that I can efficiently coordinate the vessel crew staying in the port.

Please design and create an endpoint returning the current weather conditions for the `city name` specified in the request, like: `Liverpool`, `Rotterdam` or `Busan, South Korea`.

The endpoint should use http://api.weatherapi.com/v1/current.json for obtaining the details and return response including the city name, region, country, local time and temperature in Celsius:

```
{
  "city": "Rotterdam",
  "region": "South Holland",
  "country": "Netherlands",
  "localTime": "2020-04-05 21:54",
  "temperature": 15.0
}
```

## Ticket-2

As an operator,  
I want to be able to specify Celsius or Fahrenheit for temperature measurement,  
So that I can communicate more efficiently with the vessel crew.

Assuming that Ticket-1 is implemented and released to production, please extend the `existing endpoint` with an option to return the temperature in Celsius or Fahrenheit.

## Ticket-3

As an operator,  
I want to know what are the times of sunrise and sunset,  
So that I can better plan and coordinate the operations on the vessel.

Assuming previous changes are implemented and released to production, please extend the `existing endpoint` with information of sunrise and sunset using the http://api.weatherapi.com/v1/astronomy.json api call.

## Ticket-4 (optional to verify the front-end skills)

As a operator,
I want to see the current weather details for typed in the city name,
So that I can better handle the various vessel crews.

As a part of this ticket, please create a simple react page which allows to display the weather details for the typed in city. Using typescript is highly appreciated.
