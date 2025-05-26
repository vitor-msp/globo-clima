import { Navbar } from "../components/Navbar";

export type LayoutProps = {
  child: any;
};

export const Layout: React.FC<LayoutProps> = ({ child }) => {
  return (
    <div>
      <Navbar />
      <main>{child}</main>
    </div>
  );
};
