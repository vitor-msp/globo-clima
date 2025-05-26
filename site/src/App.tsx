import "./App.css";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Layout } from "./pages/Layout";
import { LoginPage } from "./pages/LoginPage";
import { CountriesPage } from "./pages/CountriesPage";
import { FavoriteCountriesPage } from "./pages/FavoriteCountriesPage";
import { FavoriteLocationsPage } from "./pages/FavoriteLocationsPage";
import { SignupPage } from "./pages/SignupPage";
import { LocationsPage } from "./pages/LocationsPage";
import { FavoriteCountriesProvider } from "./context/FavoriteCountriesContext";
import { FavoriteLocationsProvider } from "./context/FavoriteLocationsContext";

export const App = () => {
  return (
    <FavoriteCountriesProvider>
      <FavoriteLocationsProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<Layout child={<LoginPage />} />} />
            <Route
              path="/countries"
              element={<Layout child={<CountriesPage />} />}
            />
            <Route
              path="/favorite-countries"
              element={<Layout child={<FavoriteCountriesPage />} />}
            />
            <Route
              path="/locations"
              element={<Layout child={<LocationsPage />} />}
            />
            <Route
              path="/favorite-locations"
              element={<Layout child={<FavoriteLocationsPage />} />}
            />
            <Route path="/login" element={<Layout child={<LoginPage />} />} />
            <Route
              path="/sign-up"
              element={<Layout child={<SignupPage />} />}
            />
          </Routes>
        </BrowserRouter>
      </FavoriteLocationsProvider>
    </FavoriteCountriesProvider>
  );
};
