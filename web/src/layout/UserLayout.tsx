import { App, Layout, Menu, Dropdown } from "antd";
import { Content, Header } from "antd/es/layout/layout";
import { useEffect, useState } from "react";
import { Outlet, useLocation, useNavigate } from "react-router";
import { iUser, logoutAsync } from "../features/user/authSlice";
import { useAppDispatch } from "../app/hooks";
import { AppLogo } from "../features/AppLogo";
import { AppFooter } from "../features/Footer";

export const UserLayout = ({ UserName }: iUser) => {
    const navigate = useNavigate();
    const location = useLocation();
    const dispatch = useAppDispatch();
    const [current, setCurrent] = useState(
        location.pathname === "/" || location.pathname === ""
            ? "/"
            : location.pathname
    );

    useEffect(() => {
        if (location) {
            if (current !== location.pathname) {
                setCurrent(location.pathname);

                if (location.pathname !== "/register" && location.pathname !== "/forgot-password" && location.pathname.startsWith("/token") && location.pathname !== "/profile") {
                    //navigate("/");
                }
            }
        }
    }, [location]);

    const handleClick = (key: string) => {
        navigate(key);
        setCurrent(key); // Update the current state when a menu item is clicked
    };

    const handleLogoClick = () => {
        navigate("/"); // Navigate to the home page
    };

    const menu = (
        <Menu
            theme="dark"
            items={[
                {
                    key: "/profile",
                    label: <a onClick={() => handleClick("/profile")}>Profile</a>,
                },
                {
                    key: "/change-password",
                    label: <a onClick={() => handleClick("/change-password")}>Change Password</a>,
                },
                {
                    key: "/logout",
                    label: <a onClick={() => {
                        dispatch(logoutAsync());
                        handleClick("/");
                    }}>Logout</a>,
                },
            ]}
        />
    );

    return (
        <App>
            <Layout className="layout">
                <Header style={{ display: "flex", alignItems: "center", justifyContent: "space-between" }}>
                    <AppLogo onClick={handleLogoClick} /> {/* Pass handleLogoClick to AppLogo */}
                    <div style={{ display: "flex", flex: 1 }}>
                        <Menu
                            theme="dark"
                            mode="horizontal"
                            defaultSelectedKeys={[current]}
                            selectedKeys={[current]}
                            style={{ flex: 1, justifyContent: "flex-start" }}
                            items={[
                                {
                                    key: "/shared-todos",
                                    label: "Shared Todo",
                                    onClick: (e) => {
                                        handleClick(e.key);
                                    },
                                },
                            ]}
                        />
                        <div style={{ marginLeft: "auto" }}>
                            <Dropdown overlay={menu} trigger={['click']}>
                                <a
                                    onClick={(e) => e.preventDefault()}
                                    style={{ color: '#FFFFFFA6', fontSize: '14px' }}
                                >
                                    {UserName}
                                </a>
                            </Dropdown>
                        </div>
                    </div>
                </Header>

                <Content>
                    <Outlet />
                </Content>
                <AppFooter />
            </Layout>
        </App>
    );
};
