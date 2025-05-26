namespace GloboClima.Api.Inputs;

public class GetLocationOutput
{
    public long CurrentUTCTimeUnix { get; set; }

    public double TemperatureInCelsius { get; set; }

    public double FeelsLikeInCelsius { get; set; }

    public int PressureInhPa { get; set; }

    public double HumidityPercent { get; set; }

    public double CloudsPercent { get; set; }

    public double WindSpeedInMeterPerSecond { get; set; }
}