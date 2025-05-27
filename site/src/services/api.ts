import axios, { AxiosResponse } from "axios";

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

const handleSuccess = (res: AxiosResponse<any, any>) => ({
  error: false,
  data: res.data,
});

const handleError = (error: any) => ({
  error: true,
  data: error?.response?.data?.apiErrorMessage ?? "Unknown error.",
});

const getHeaders = (accessToken: string) => ({
  Authorization: `Bearer ${accessToken}`,
});

const handleRequest = (request: Promise<AxiosResponse<any, any>>) =>
  request.then(handleSuccess).catch(handleError);

export const api = {
  getCountryDemographicInformation: async (
    cioc: string
  ): Promise<Output<Country>> => {
    return await handleRequest(httpClient.get(`/countries/${cioc}`));
  },

  getLocationWeatherInformation: async (
    lat: number,
    lon: number
  ): Promise<Output<Location>> => {
    return await handleRequest(
      httpClient.get(`/locations/lat/${lat}/lon/${lon}`)
    );
  },

  getFavoriteCountries: async (
    accessToken: string
  ): Promise<Output<FavoriteCountry[]>> => {
    return await handleRequest(
      httpClient.get(`/favorite-countries`, {
        headers: getHeaders(accessToken),
      })
    );
  },

  getFavoriteLocations: async (
    accessToken: string
  ): Promise<Output<FavoriteLocation[]>> => {
    return await handleRequest(
      httpClient.get(`/favorite-locations`, {
        headers: getHeaders(accessToken),
      })
    );
  },

  createFavoriteCountry: async (
    input: {
      cioc: string;
    },
    accessToken: string
  ): Promise<Output<{ favoriteCountryId: string }>> => {
    return await handleRequest(
      httpClient.post(`/favorite-countries`, input, {
        headers: getHeaders(accessToken),
      })
    );
  },

  removeFavoriteCountry: async (
    id: string,
    accessToken: string
  ): Promise<Output<void>> => {
    return await handleRequest(
      httpClient.delete(`/favorite-countries/${id}`, {
        headers: getHeaders(accessToken),
      })
    );
  },

  createFavoriteLocation: async (
    input: {
      lat: number;
      lon: number;
    },
    accessToken: string
  ): Promise<Output<{ favoriteLocationId: string }>> => {
    return await handleRequest(
      httpClient.post(`/favorite-locations`, input, {
        headers: getHeaders(accessToken),
      })
    );
  },

  removeFavoriteLocation: async (
    id: string,
    accessToken: string
  ): Promise<Output<void>> => {
    return await handleRequest(
      httpClient.delete(`/favorite-locations/${id}`, {
        headers: getHeaders(accessToken),
      })
    );
  },

  signup: async (input: SignupInput): Promise<Output<void>> => {
    return await handleRequest(httpClient.post(`/users`, input));
  },

  login: async (
    input: LoginInput
  ): Promise<Output<{ accessToken: string }>> => {
    return await handleRequest(httpClient.post(`/login`, input));
  },
};
