import { NavLink } from "react-router-dom";

export const Navbar = () => {
  return (
    <nav>
      <NavLink to={"/countries"}>Countries Demographic Information</NavLink>
      <NavLink to={"/favorite-countries"}>Favorite Countries</NavLink>
      <NavLink to={"/locations"}>Locations Weather Information</NavLink>
      <NavLink to={"/favorite-locations"}>Favorite Locations</NavLink>
      <NavLink to={"/login"}>Login</NavLink>
      <NavLink to={"/sign-up"}>Sign Up</NavLink>
    </nav>
  );
};
