import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;
if (!API_URL) throw Error("missing API_URL environment variable");
const api = axios.create({ baseURL: API_URL });

class Output<T> {
  constructor(public error: boolean, public data: T) {}
}

export type Country = {
  commonName: string;
  officialName: string;
  cioc: string;
  population: number;
  languages: string[];
  currencies: { symbol: string; name: string }[];
};

export const getCountryDemographicInformation = async (
  cioc: string
): Promise<Output<Country>> => {
  return {
    error: false,
    data: {
      commonName: "Brazil",
      officialName: "Federative Republic of Brazil",
      cioc: "BRA",
      population: 212559409,
      languages: ["Portuguese"],
      currencies: [{ symbol: "R$", name: "Brazilian real" }],
    },
  };
};

export type Location = {
  currentUTCTimeUnix: number;
  temperatureInCelsius: number;
  feelsLikeInCelsius: number;
  pressureInhPa: number;
  humidityPercent: number;
  cloudsPercent: number;
  windSpeedInMeterPerSecond: number;
};

export const getLocationWeatherInformation = async (
  lat: number,
  lon: number
): Promise<Output<Location>> => {
  return {
    error: false,
    data: {
      currentUTCTimeUnix: 1748294616,
      temperatureInCelsius: 21.35,
      feelsLikeInCelsius: 21.26,
      pressureInhPa: 1022,
      humidityPercent: 66,
      cloudsPercent: 0,
      windSpeedInMeterPerSecond: 2.57,
    },
  };
};
