import axios from "axios";
import { iAppResponse } from "../../app/appResponse";

const BASE_URL = import.meta.env.VITE_BASE_URL;
export const login = async (email: string, password: string) => {
  const response = await axios.post<
      iAppResponse<{ accessToken: string; refreshToken: string }>
  >(`${BASE_URL}/user/login`, {
    email: email,
    password: password,
  }).catch((ex)=>{
    console.log(ex);
  });
  return response?.data;
};

export const refreshToken = async (data: {
  accessToken: string;
  refreshToken: string;
}) => {
  const response = await axios.post<
      iAppResponse<{ accessToken: string; refreshToken: string }>
  >(`${BASE_URL}/user/refreshToken`, data).catch((ex)=>{
    console.log(ex);
  });;
  return response?.data;
};

export const register = async (email: string, password: string, firstName: string, lastName: string) => {
  try {
    const response = await axios.post<iAppResponse<{}>>(
        `${BASE_URL}/user/register`,
        {
          email: email,
          password: password,
          firstName: firstName,
          lastName: lastName
        }
    );
    return response?.data;
  } catch (ex) {
    console.log(ex);
  }
};

export const logout = async () => {
  const response = await axios.post<iAppResponse<boolean>>(
      `${BASE_URL}/user/logout`
  ).catch((ex)=>{
    console.log(ex);
  });;
  return response?.data;
};
