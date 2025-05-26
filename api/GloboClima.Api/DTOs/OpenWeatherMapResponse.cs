using System.Text.Json.Serialization;
using GloboClima.Api.Inputs;

namespace GloboClima.Api.DTOs;

public class OpenWeatherMapResponse
{
    [JsonPropertyName("current")]
    public Current CurrentWeather { get; set; } = new();

    public class Current
    {
        [JsonPropertyName("dt")]
        public long CurrentUTCTimeUnix { get; set; }

        [JsonPropertyName("temp")]
        public double TemperatureInCelsius { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLikeInCelsius { get; set; }

        [JsonPropertyName("pressure")]
        public int PressureInhPa { get; set; }

        [JsonPropertyName("humidity")]
        public double HumidityPercent { get; set; }

        [JsonPropertyName("clouds")]
        public double CloudsPercent { get; set; }

        [JsonPropertyName("wind_speed")]
        public double WindSpeedInMeterPerSecond { get; set; }
    }

    public GetLocationOutput ConvertOutput()
        => new()
        {
            CurrentUTCTimeUnix = CurrentWeather.CurrentUTCTimeUnix,
            TemperatureInCelsius = CurrentWeather.TemperatureInCelsius,
            FeelsLikeInCelsius = CurrentWeather.FeelsLikeInCelsius,
            PressureInhPa = CurrentWeather.PressureInhPa,
            HumidityPercent = CurrentWeather.HumidityPercent,
            CloudsPercent = CurrentWeather.CloudsPercent,
            WindSpeedInMeterPerSecond = CurrentWeather.WindSpeedInMeterPerSecond,
        };
}