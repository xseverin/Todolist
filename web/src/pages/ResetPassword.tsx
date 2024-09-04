import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { message } from 'antd';
import axios from 'axios';

import Header from '../features/Form/Header';
import Password from '../features/Form/Password';
import CleanAllButton from '../features/Form/CleanAllButton';
import Button from '../features/Form/Button.tsx';

interface Params {
    token: string;
}

interface ResetPasswordForm {
    password: string;
    confirmPassword: string;
}

const ResetPassword: React.FC = () => {
    const { token } = useParams<Params>();
    const { register, handleSubmit, setError, formState: { errors }, reset, getValues } = useForm<ResetPasswordForm>();
    const navigate = useNavigate();

    const [serverErrors, setServerErrors] = useState<{ [key: string]: string[] }>({});

    const onFinish = async (values: ResetPasswordForm) => {
        const { password, confirmPassword } = values;

        // Clear any existing server errors
        setServerErrors({});

        if (password !== confirmPassword) {
            setError('confirmPassword', { message: 'Passwords do not match' });
            message.error('Passwords do not match');
            return;
        }

        try {
            const response = await axios.put(`https://localhost:1002/User/resetpassword?token=${token}`, { password });

            if (response.data.isSucceed) {
                message.success('Password reset successfully');
                navigate('/login');
            } else if (response.data.messages) {
                const newServerErrors: { [key: string]: string[] } = {};

                // Process server-side error messages
                Object.keys(response.data.messages).forEach(field => {
                    if (field.startsWith("Password")) {
                        // Handle password-related errors
                        if (!newServerErrors.password) {
                            newServerErrors.password = [];
                        }
                        //console.log(response.data.messages[field], field);
                        newServerErrors.password.push(...response.data.messages[field]);
                    } else if (field === "Token") {
                        // Handle token errors
                        newServerErrors.token = response.data.messages[field];
                        // Display token error messages
                        response.data.messages[field].forEach(msg => message.error(msg));
                    } else {
                        // Handle other fields dynamically
                        newServerErrors[field.toLowerCase()] = response.data.messages[field];
                    }
                });
                //console.log(newServerErrors);
                // Update the serverErrors state
                setServerErrors(newServerErrors);

                // Display a general error message
                if (!newServerErrors.token) {
                    message.error("Password reset failed. Please check the fields for more details.");
                }
            } else {
                message.error(response.data.message || 'Error resetting password');
            }
        } catch (error) {
            message.error('Server Error');
        }
    };

    return (
        <div className="form-container min-h-[80vh]">
            <Header
                mainTitle="Reset Your Password"
                subTitle="Remembered your password?"
                linkText="Log In"
                linkUrl="/login"
            />

            <div className="inner-form-container">
                <form onSubmit={handleSubmit(onFinish)} autoComplete="off">
                    <Password
                        register={register}
                        errors={errors}
                        getValues={getValues}
                        showConfirmPasswordField={true}
                        serverErrors={serverErrors}
                    />

                    <div className="flex justify-center">
                        <Button label="Reset Password" width="70%"/>
                    </div>

                </form>
            </div>
        </div>
    );
};

export default ResetPassword;
