import React from 'react';
import { useForm } from 'react-hook-form';
import axios from 'axios';
import { message } from 'antd';
import Password from './Password';
import Button from './Button';

const BASE_URL = import.meta.env.VITE_BASE_URL;

interface ChangePasswordRequest {
    currentPassword: string;
    password: string;
    confirmPassword: string;
}

const ChangePasswordForm: React.FC = () => {
    const {
        register: registerPassword,
        handleSubmit: handlePasswordSubmit,
        getValues,
        formState: { errors: passwordErrors },
    } = useForm<ChangePasswordRequest>();

    const onPasswordFinish = async (values: ChangePasswordRequest) => {
        if (values.password !== values.confirmPassword) {
            message.error('New password and confirm password do not match.');
            return;
        }

        try {
            const response = await axios.post(`${BASE_URL}/user/changePassword`, {
                currentPassword: values.currentPassword,
                newPassword: values.password,
                confirmPassword: values.confirmPassword,
            });
            if (response.data.isSucceed) {
                message.success('Password updated successfully!');
            } else {
                message.error('Failed to update password.');
            }
        } catch (error) {
            message.error('An error occurred while updating password.');
        }
    };

    return (
        <div className="inner-form-container mt-0 mp-0">
            <form onSubmit={handlePasswordSubmit(onPasswordFinish)} autoComplete="off" className="pb-0">
                <Password
                    register={registerPassword}
                    errors={passwordErrors}
                    getValues={getValues}
                    showConfirmPasswordField
                    showCurrentPasswordField
                    serverErrors={{}} // Pass any server errors related to password here if needed
                />

                <div className="flex justify-center">
                    <Button label="Change Password" width="70%"/>
                </div>

            </form>
        </div>
    );
};

export default ChangePasswordForm;
