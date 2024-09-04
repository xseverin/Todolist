import React, { useState } from "react";
import Header from "./Header";
import FirstName from "./FirstName";
import LastName from "./LastName";
import Email from "./Email";
import Password from "./Password";
// redux
import { useDispatch } from "react-redux";
import { useForm } from "react-hook-form";
import { resetLoading, setLoading } from "../user/authSlice.ts";
import { register as req } from "../user/authAPI.ts";
import { message } from 'antd'; // For Ant Design messages
import { useNavigate } from 'react-router-dom';
import Button from "./Button.tsx";
import CleanAllButton from "./CleanAllButton.tsx";
import SignUpButton from "./Sing up.tsx"; // For navigation

type FieldType = {
  email?: string;
  password?: string;
  confirmPassword?: string;
  firstName?: string;
  lastName?: string;
};

export const RegisterForm = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const {
    register,
    handleSubmit,
    getValues,
    setError,
    formState: { errors },
    reset,
  } = useForm<FieldType>();

  const [serverErrors, setServerErrors] = useState<{ [key: string]: string[] }>({});

  // Function to handle form submission
  const onFinish = async (values: FieldType) => {
    dispatch(setLoading());

    try {
      const data = await req(
          values.email as string,
          values.password as string,
          values.firstName as string,
          values.lastName as string
      );

      dispatch(resetLoading());

      if (data?.isSucceed) {
        message.success("Registration is successful, Please login.");
        navigate("/login");
      } else if (data?.messages) {
        const newServerErrors: { [key: string]: string[] } = {};

        Object.keys(data.messages).forEach(field => {
          if (field.startsWith("Password")) {
            // If the field starts with "Password", treat it as a password-related error
            if (!newServerErrors.password) {
              newServerErrors.password = [];
            }
            newServerErrors.password.push(...data.messages[field]);
          } else if (field === "DuplicateUserName") {
            // For email errors
            newServerErrors.email = data.messages[field];
          } else {
            // Handle other fields dynamically
            newServerErrors[field.toLowerCase()] = data.messages[field];
          }
        });

        // Update the serverErrors state
        setServerErrors(newServerErrors);

        // Display a general error message
        message.error("Registration failed. Please check the fields for more details.");
      } else {
        message.error("Unexpected error occurred, please try again later.");
      }
    } catch (error) {
      dispatch(resetLoading());
      message.error("Unexpected error occurred, please try again later.");
    }
  };

  return (
      <div className="form-container min-h-[70vh] ">
        <Header
            mainTitle="Create new account"
            subTitle="Already a member?"
            linkText="Log In"
            linkUrl="/login"
        />

        <div className="inner-form-container">
          <form onSubmit={handleSubmit(onFinish)} autoComplete="off">
            <FirstName register={register} errors={errors}/>
            <LastName register={register} errors={errors}/>
            <Email register={register} errors={errors} serverErrors={serverErrors}/>
            <Password register={register} errors={errors} getValues={getValues} showConfirmPasswordField
                      serverErrors={serverErrors}/>

            <div className="flex justify-between">
              <CleanAllButton reset={reset} width="40%"/>
              <Button label="Create account"  width="60%"/>
            </div>
            
          </form>
        </div>
      </div>
  );
};

export default RegisterForm;
