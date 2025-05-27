import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;
if (!API_URL) throw Error("missing API_URL environment variable");
const httpClient = axios.create({ baseURL: API_URL });

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

export type Location = {
  currentUTCTimeUnix: number;
  temperatureInCelsius: number;
  feelsLikeInCelsius: number;
  pressureInhPa: number;
  humidityPercent: number;
  cloudsPercent: number;
  windSpeedInMeterPerSecond: number;
};

export type FavoriteCountry = {
  id: string;
  cioc: string;
};

export type FavoriteLocation = {
  id: string;
  lat: number;
  lon: number;
};

export type SignupInput = {
  name: string;
  username: string;
  password: string;
  passwordConfirmation: string;
};

export type LoginInput = {
  username: string;
  password: string;
};

export const api = {
  getCountryDemographicInformation: async (
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
  },

  getLocationWeatherInformation: async (
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
  },

  getFavoriteCountries: async (): Promise<Output<FavoriteCountry[]>> => {
    return {
      error: false,
      data: [
        {
          id: "568d91ac-7cfd-4584-85bb-bb4cdad365f4",
          cioc: "GBR",
        },
      ],
    };
  },

  getFavoriteLocations: async (): Promise<Output<FavoriteLocation[]>> => {
    return {
      error: false,
      data: [
        {
          id: "f41a2d76-ebd4-4e94-baa9-00fe033a2801",
          lat: 90,
          lon: -107,
        },
      ],
    };
  },

  createFavoriteCountry: async (input: {
    cioc: string;
  }): Promise<Output<{ favoriteCountryId: string }>> => {
    return {
      error: false,
      data: {
        favoriteCountryId: "f41a2d76-ebd4-4e94-baa9-00fe033a2801",
      },
    };
  },

  removeFavoriteCountry: async (id: string): Promise<Output<void>> => {
    return {
      error: false,
      data: undefined,
    };
  },

  createFavoriteLocation: async (input: {
    lat: number;
    lon: number;
  }): Promise<Output<{ favoriteLocationId: string }>> => {
    return {
      error: false,
      data: {
        favoriteLocationId: "f41a2d76-ebd4-4e94-baa9-00fe033a2802",
      },
    };
  },

  removeFavoriteLocation: async (id: string): Promise<Output<void>> => {
    return {
      error: false,
      data: undefined,
    };
  },

  signup: async (input: SignupInput): Promise<Output<void>> => {
    return {
      error: false,
      data: undefined,
    };
  },

  login: async (
    input: LoginInput
  ): Promise<Output<{ accessToken: string }>> => {
    return {
      error: false,
      data: { accessToken: "any-jwt" },
    };
  },
};
