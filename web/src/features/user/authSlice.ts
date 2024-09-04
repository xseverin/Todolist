import { PayloadAction, createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";
import { jwtDecode } from "jwt-decode";
import { logout } from "./authAPI";

export interface iUser {
  Id: string;
  RoleClaim: Array<string>;
  UserName: string;
  email: string;
  firstName: string;
  lastName: string;
  address: string;
}

export interface iAuthState {
  status: "idle" | "loading" | "failed";
  accessToken?: string;
  refreshToken?: string;
  user?: iUser;
}

const initialState: iAuthState = {
  status: "idle",
};

export const logoutAsync = createAsyncThunk("user/logout", async () => {
  const response = await logout();
  return response?.data;
});

export const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    updateToken: (
        state,
        action: PayloadAction<{ accessToken: string; refreshToken: string }>
    ) => {
      state.accessToken = action.payload.accessToken;
      state.refreshToken = action.payload.refreshToken;
      state.user = jwtDecode<iUser>(action.payload.accessToken);
    },
    resetToken: (state) => {
      state.accessToken = undefined;
      state.refreshToken = undefined;
      state.user = undefined;
    },
    setLoading: (state) => {
      state.status = "loading";
    },
    resetLoading: (state) => {
      state.status = "idle";
    },
    updateUser: (state, action: PayloadAction<Partial<iUser>>) => {
      if (state.user) {
        state.user = { ...state.user, ...action.payload };
      }
    },
    updateUserName: (state, action: PayloadAction<string>) => {

      console.log(action.payload);
      if (state.user) {
        console.log(action.payload);
        state.user.UserName = action.payload;
      }
    }
  },
  extraReducers: (builder) => {
    builder
        .addCase(logoutAsync.pending, (state) => {
          state.status = "loading";
        })
        .addCase(logoutAsync.fulfilled, (state) => {
          state.status = "idle";
          state.accessToken = undefined;
          state.refreshToken = undefined;
          state.user = undefined;
        })
        .addCase(logoutAsync.rejected, (state) => {
          state.status = "failed";
        });
  },
});

export const { updateToken, resetToken, setLoading, resetLoading, updateUserName } =
    authSlice.actions;

export const selectAuth = (state: RootState) => state.auth;

export default authSlice.reducer;