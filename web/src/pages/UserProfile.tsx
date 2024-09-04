import React, { useEffect, useState } from 'react';
import { Form, Input, Button, message } from 'antd';
import axios from 'axios';
import { useAppSelector, useAppDispatch } from "../app/hooks.ts";
import { iUser, selectAuth, updateUser, updateUserName } from "../features/user/authSlice.ts";

const BASE_URL = import.meta.env.VITE_BASE_URL;

interface UserProfileData {
  email: string;
  firstName: string;
  lastName: string;
  address: string;
}

interface PasswordChangeData {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

const UserProfile: React.FC = () => {
  const [form] = Form.useForm();
  const [passwordForm] = Form.useForm();
  const dispatch = useAppDispatch();

  const [userData, setUserData] = useState<UserProfileData | null>(null);

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await axios.get<UserProfileData>(`${BASE_URL}/user/profile`);
        console.log(response.data);
        setUserData(response.data);
        form.setFieldsValue(response.data);
      } catch (error) {
        message.error('Failed to load user data.');
      }
    };

    fetchUserData();
  }, [form]);

  const onProfileFinish = async (values: UserProfileData) => {
    try {
      const response = await axios.put(`${BASE_URL}/user/UpdateProfile`, values);
      if (response.data.isSucceed) {
        message.success('Profile updated successfully!');
        dispatch(updateUserName(values.email));
      } else {
        message.error('Failed to update profile.');
      }
    } catch (error) {
      message.error('An error occurred while updating profile.');
    }
  };

  const onPasswordFinish = async (values: PasswordChangeData) => {
    if (values.newPassword !== values.confirmPassword) {
      message.error('New password and confirm password do not match.');
      return;
    }

    try {
      const response = await axios.post(`${BASE_URL}/user/changePassword`, values);
      if (response.data.isSucceed) {
        message.success('Password updated successfully!');
      } else {
        message.error('Failed to update password.');
      }
    } catch (error) {
      message.error('An error occurred while updating password.');
    }
  };

  if (!userData) {
    return <div>Loading...</div>;
  }

  return (
      <>
        <Form
            form={form}
            name="userProfile"
            layout="vertical"
            onFinish={onProfileFinish}
            initialValues={userData}
        >
          <Form.Item
              label="Email"
              name="email"
              rules={[{ required: true, type: 'email', message: 'Please enter a valid email!' }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
              label="First Name"
              name="firstName"
              rules={[{ required: true, message: 'Please enter your first name!' }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
              label="Last Name"
              name="lastName"
              rules={[{ required: true, message: 'Please enter your last name!' }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
              label="Address"
              name="address"
              rules={[{ required: true, message: 'Please enter your address!' }]}
          >
            <Input />
          </Form.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit">
              Save Changes
            </Button>
          </Form.Item>
        </Form>

        <Form
            form={passwordForm}
            name="passwordChange"
            layout="vertical"
            onFinish={onPasswordFinish}
            initialValues={{ currentPassword: '', newPassword: '', confirmPassword: '' }}
        >
          <Form.Item
              label="Current Password"
              name="currentPassword"
              rules={[{ required: true, message: 'Please enter your current password!' }]}
          >
            <Input.Password />
          </Form.Item>
          <Form.Item
              label="New Password"
              name="newPassword"
              rules={[{ required: true, message: 'Please enter your new password!' }]}
          >
            <Input.Password />
          </Form.Item>
          <Form.Item
              label="Confirm New Password"
              name="confirmPassword"
              rules={[{ required: true, message: 'Please confirm your new password!' }]}
          >
            <Input.Password />
          </Form.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit">
              Change Password
            </Button>
          </Form.Item>
        </Form>
      </>
  );
};

export default UserProfile;