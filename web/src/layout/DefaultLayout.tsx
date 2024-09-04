import { App, Layout, Menu } from "antd";
import { Content, Header } from "antd/es/layout/layout";
import { useEffect, useState } from "react";
import { Outlet, useLocation, useNavigate } from "react-router";
import { AppLogo } from "../features/AppLogo";
import { AppFooter } from "../features/Footer";
import Nav from "../features/Nav/Nav.tsx";

export const DefaultLayout = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const [current, setCurrent] = useState(
        location.pathname === "/" || location.pathname === ""
            ? "/login"
            : location.pathname
    );

    useEffect(() => {
        if (location) {
            if (current !== location.pathname) {
                setCurrent(location.pathname);

                if (location.pathname !== "/register" && location.pathname !== "/forgot-password" && location.pathname.startsWith("/token") && location.pathname !== "/profile" ) {
                    //navigate("/login");
                }
            }
        }}, [location]);

    const handleClick = (key: string) => {
        navigate(key);
    };

    return (
        <App>
            <Layout className="layout ">
                <Header style={{ display: "flex", alignItems: "center" }}>
                    <AppLogo />
                    <Menu
                        theme="dark"
                        mode="horizontal"
                        defaultSelectedKeys={["/login"]}
                        selectedKeys={[current]}
                        style={{ minWidth: "500px" }}
                        items={[
                            {
                                key: "/login",
                                label: "Login",
                                onClick: (e) => {
                                    handleClick(e.key);
                                },
                            },
                            {
                                key: "/register",
                                label: "Register",
                                onClick: (e) => {
                                    handleClick(e.key);
                                },
                            }
                        ]}
                    />
                </Header>
                <Content>
                    <Outlet />

                </Content>
                <AppFooter />
            </Layout>
        </App>
    );
};
