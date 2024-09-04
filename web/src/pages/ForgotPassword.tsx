import React, { useState } from "react";
import { useForm } from "react-hook-form";
import { message } from "antd";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { resetLoading, setLoading } from "../features/user/authSlice.ts";
import Header from "../features/Form/Header";
import Email from "../features/Form/Email";
import Button from "../features/Form/Button.tsx";

type ForgotPasswordData = {
    email?: string;
};

const ForgotPassword: React.FC = () => {
    const navigate = useNavigate();
    const dispatch = useDispatch();
    const {
        register,
        handleSubmit,
        formState: { errors }
    } = useForm<ForgotPasswordData>();

    const [serverErrors, setServerErrors] = useState<{ [key: string]: string[] }>({});

    const onFinish = async (values: ForgotPasswordData) => {
        dispatch(setLoading());
        try {
            const response = await axios.post(`${import.meta.env.VITE_BASE_URL}/user/forgotPassword`, values);

            dispatch(resetLoading());

            if (response.data.isSucceed) {
                message.success("Password reset link sent to your email.");
                navigate("/login");
            } else {
                message.error("Failed to send password reset link.");
                setServerErrors({ email: response.data.messages?.email || ["Failed to send reset link."] });
            }
        } catch (error) {
            dispatch(resetLoading());
            message.error("An error occurred while sending password reset link.");
        }
    };

    return (
        <div className="form-container min-h-[80vh]">
            <Header
                mainTitle="Forgot Password"
                subTitle="Remember your password?"
                linkText="Log In"
                linkUrl="/login"
            />

            <div className="inner-form-container">
                <form onSubmit={handleSubmit(onFinish)} autoComplete="off">
                    <Email register={register} errors={errors} serverErrors={serverErrors}/>

                    <div className="flex items-center justify-center">
                        <Button label="Send Reset Link"  width="70%"/>
                    </div>

                </form>
            </div>
        </div>
    );
};

export default ForgotPassword;
