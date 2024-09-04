import "bootstrap/dist/css/bootstrap-reboot.min.css";
import "bootstrap/dist/css/bootstrap-utilities.min.css";
import { Route, Routes } from "react-router";
import { DefaultLayout } from "./layout/DefaultLayout";
import { HomePage } from "./pages/HomePage";
import { RegisterPage } from "./pages/RegisterPage";
import { LoginPage } from "./pages/LoginPage";
import { NotFoundPage } from "./pages/NotFoundPage";
import { useAppSelector } from "./app/hooks";
import { selectAuth } from "./features/user/authSlice";
import { UserLayout } from "./layout/UserLayout";
import { Spin } from "antd";
import { useEffect } from "react";

import "./index.css";
import SharedTodoList from "./pages/SharedTodoList";
import UserProfile from "./pages/UserProfile";
import ForgotPassword from "./pages/ForgotPassword";
import ResetPassword from "./pages/ResetPassword";
import ChangePasswordPage from "./pages/ChangePasswordPage.tsx";

// Function to clear cookies and localStorage only for the current site
const clearSiteData = () => {
  localStorage.clear();
};

export const App = () => {
  const auth = useAppSelector(selectAuth);

  useEffect(() => {
    const persistedAuth = localStorage.getItem("persist:auth");

    if (persistedAuth) {
      try {
        const parsedAuth = JSON.parse(persistedAuth);
        const status = parsedAuth && parsedAuth.status ? JSON.parse(parsedAuth.status) : null;

        if (status === "loading") {
          clearSiteData();
          console.log("Cleared site data due to loading status.");
          //window.location.reload(); // Reload the page after clearing the data
        }
      } catch (error) {
        console.error("Error parsing auth status from storage:", error);
      }
    }
  }, [auth.status]);

  if (!auth.user) {
    return (
        <Spin spinning={auth.status === "loading"}>
          <Routes>
            <Route path="/" element={<DefaultLayout />}>
              <Route path="register" element={<RegisterPage />} />
              <Route path="login" element={<LoginPage />} />
              <Route path="/forgot-password" element={<ForgotPassword />} />
              <Route path="/resetpassword/:token" element={<ResetPassword />} />
              <Route index element={<LoginPage />} />
              <Route path="*" element={<NotFoundPage />} />
            </Route>
          </Routes>
        </Spin>
    );
  } else {
    return (
        <Spin spinning={auth.status === "loading"}>
          <Routes>
            <Route path="/" element={<UserLayout {...auth.user} />}>
              <Route index element={<HomePage {...auth.user} />} />
              <Route path="shared-todos" element={<SharedTodoList />} />
              <Route path="profile" element={<UserProfile {...auth.user} />} />
              <Route path="change-password" element={<ChangePasswordPage />} />
              <Route path="*" element={<NotFoundPage />} />
            </Route>
          </Routes>
        </Spin>
    );
  }
};

export default App;
