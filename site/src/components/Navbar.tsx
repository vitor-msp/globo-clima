import { useContext } from "react";
import { NavLink } from "react-router-dom";
import { LoginContext } from "../context/LoginContext";

export const Navbar = () => {
  const loginContext = useContext(LoginContext);

  const logout = () => loginContext.logout();

  return (
    <nav>
      <NavLink to={"/countries"}>Countries Demographic Information</NavLink>
      <NavLink to={"/favorite-countries"}>Favorite Countries</NavLink>
      <NavLink to={"/locations"}>Locations Weather Information</NavLink>
      <NavLink to={"/favorite-locations"}>Favorite Locations</NavLink>
      {(loginContext.isLogged && (
        <NavLink to={"/login"} onClick={logout}>
          Logout
        </NavLink>
      )) || (
        <>
          <NavLink to={"/login"}>Login</NavLink>
          <NavLink to={"/sign-up"}>Sign Up</NavLink>
        </>
      )}
    </nav>
  );
};
