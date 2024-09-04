import { Form, Input, Button, message } from "antd";
import React from "react";
import axios from "axios";

const BASE_URL = import.meta.env.VITE_BASE_URL;

interface ForgotPasswordData {
    email: string;
}

const ForgotPassword: React.FC = () => {
    const [form] = Form.useForm();

    const onFinish = async (values: ForgotPasswordData) => {
        try {
            const response = await axios.post(`${BASE_URL}/user/forgotPassword`, values);
            if (response.data.isSucceed) {
                message.success("Password reset link sent to your email.");
            } else {
                message.error("Failed to send password reset link.");
            }
        } catch (error) {
            message.error("An error occurred while sending password reset link.");
        }
    };

    return (
        <Form
            form={form}
            name="forgotPassword"
            layout="vertical"
            style={{ maxWidth: 300 }}
            onFinish={onFinish}
            initialValues={{ email: "" }}
        >
            <Form.Item
                label="Email"
                name="email"
                rules={[{ required: true, type: "email", message: "Please enter a valid email!" }]}
            >
                <Input />
            </Form.Item>
            <Form.Item>
                <Button type="primary" htmlType="submit">
                    Send Reset Link
                </Button>
            </Form.Item>
        </Form>
    );
};

export default ForgotPassword;