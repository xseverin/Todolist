import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import { Form, Input, Button, message } from 'antd';
import axios from 'axios';

import { Outlet, useLocation, useNavigate } from "react-router";

interface Params {
    token: string;
}


const ResetPassword: React.FC = () => {
    const { token } = useParams<Params>();
    const [form] = Form.useForm();
    const navigate = useNavigate();

    const onFinish = async (values: { password: string, confirmPassword: string }) => {
        const { password, confirmPassword } = values;
        if (password !== confirmPassword) {
            message.error('Passwords do not match');
        } else {
            try {
                const response = await axios.put(`https://localhost:1002/User/resetpassword?token=${token}`, {
                    password
                });
                if (response.data.isSucceed) {
                    message.success('Password reset successfully');
                    navigate('/login');
                } 
                else if (response.data.messages?.password) {
                    form.setFields([
                        { name: 'password', errors: response.data.messages.password }
                    ]);}
                   else if ( response.data.messages?.token) {
                    message.error(response.data.messages.token);
                }
                else {
                    message.error(response.data.message || 'Error resetting password');
                }
            } catch {
                message.error('Server Error');
            }
        }
    };

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100">
            <Form
                form={form}
                name="reset-password"
                labelCol={{ span: 8 }}
                wrapperCol={{ span: 16 }}
                style={{ maxWidth: 600 }}
                initialValues={{ remember: true }}
                onFinish={onFinish}
                autoComplete="off"
                className="bg-white p-6 rounded shadow-md w-full max-w-sm"
            >
                <h2 className="text-2xl font-bold mb-4">Сброс пароля</h2>
                <Form.Item
                    name="password"
                    label="Новый пароль"
                    rules={[{ required: true, message: 'Введите новый пароль!' }]}
                >
                    <Input.Password
                        name="new-password"
                        id="new-password"
                        autoComplete="new-password"
                    />
                </Form.Item>

                <Form.Item
                    name="confirmPassword"
                    label="Подтвердите пароль"
                    rules={[{ required: true, message: 'Подтвердите новый пароль!' }]}
                >
                    <Input.Password
                        name="confirm-password"
                        id="confirm-password"
                        autoComplete="confirm-password"
                    />
                </Form.Item>

                <Form.Item wrapperCol={{ offset: 8, span: 16 }}>
                    <Button type="primary" htmlType="submit">
                        Сбросить пароль
                    </Button>
                </Form.Item>
            </Form>
        </div>
    );
};

export default ResetPassword;
