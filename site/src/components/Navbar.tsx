import { useContext } from "react";
import { NavLink } from "react-router-dom";
import { LoginContext } from "../context/LoginContext";

export const Navbar = () => {
  const context = useContext(LoginContext);

  const logout = () => context.setAccessToken(null);

  return (
    <nav>
      <NavLink to={"/countries"}>Countries Demographic Information</NavLink>
      <NavLink to={"/favorite-countries"}>Favorite Countries</NavLink>
      <NavLink to={"/locations"}>Locations Weather Information</NavLink>
      <NavLink to={"/favorite-locations"}>Favorite Locations</NavLink>
      <NavLink to={"/login"}>Login</NavLink>
      <NavLink to={"/sign-up"}>Sign Up</NavLink>
      <NavLink to={"/login"} onClick={logout}>
        Logout
      </NavLink>
    </nav>
  );
};
