import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { useDispatch } from 'react-redux';
import axios from 'axios';
import { message } from 'antd';
import { useNavigate } from 'react-router-dom';

import Header from '../features/Form/Header';
import FirstName from '../features/Form/FirstName';
import LastName from '../features/Form/LastName';
import Email from '../features/Form/Email';
import Password from '../features/Form/Password';
import Button from '../features/Form/Button.tsx';
import ClearAllButton from '../features/Form/CleanAllButton'; // Updated import
import { updateUserName } from '../features/user/authSlice';
import CleanAllButton from "../features/Form/CleanAllButton";

const BASE_URL = import.meta.env.VITE_BASE_URL;

interface UserProfileData {
  email: string;
  firstName: string;
  lastName: string;
}

interface ChangePasswordRequest {
  currentPassword: string;
  password: string;
  confirmPassword: string;
}

export const UserProfile: React.FC = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const {
    register: registerProfile,
    handleSubmit: handleProfileSubmit,
    reset: resetProfile,
    formState: { errors: profileErrors },
    getValues
  } = useForm<UserProfileData>();

  const {
    register: registerPassword,
    handleSubmit: handlePasswordSubmit,
    reset: resetPassword,
    formState: { errors: passwordErrors }
  } = useForm<ChangePasswordRequest>();

  const [userData, setUserData] = useState<UserProfileData | null>(null);
  const [serverErrors, setServerErrors] = useState<{ [key: string]: string[] }>({});
  const [showClearButton, setShowClearButton] = useState<boolean>(false); // State for hidden button

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await axios.get<UserProfileData>(`${BASE_URL}/user/profile`);
        setUserData(response.data);
        resetProfile(response.data);
      } catch (error) {
        message.error('Failed to load user data.');
      }
    };

    fetchUserData();
  }, [resetProfile]);

  const onProfileFinish = async (values: UserProfileData) => {
    try {
      const response = await axios.put(`${BASE_URL}/user/UpdateProfile`, values);
      if (response.data.isSucceed) {
        message.success('Profile updated successfully!');
        dispatch(updateUserName(values.email));
      } else if (response.data.messages) {
        const newServerErrors: { [key: string]: string[] } = {};

        Object.keys(response.data.messages).forEach(field => {
          newServerErrors[field.toLowerCase()] = response.data.messages[field];
        });

        setServerErrors(newServerErrors);
        message.error('Failed to update profile. Check the fields for details.');
      } else {
        message.error('Unexpected error occurred, please try again later.');
      }
    } catch (error) {
      message.error('Unexpected error occurred, please try again later.');
    }
  };

  const onPasswordFinish = async (values: ChangePasswordRequest) => {
    if (values.password !== values.confirmPassword) {
      message.error('New password and confirm password do not match.');
      return;
    }

    try {
      const response = await axios.post(`${BASE_URL}/user/changePassword`, {currentPassword: values.currentPassword, newPassword: values.password, confirmPassword: values.confirmPassword});
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
      <div className="form-container" style={{ minHeight: 'calc(78vh)' }}>
        <Header
            mainTitle="Update Profile"
            subTitle="Manage your personal information"
            linkText="Back to Dashboard"
            linkUrl="/"
        />

        <div className="inner-form-container mp-0 mt-0">
          {/* User Profile Form */}
          <form onSubmit={handleProfileSubmit(onProfileFinish)} autoComplete="off" className="pb-0">
            <FirstName register={registerProfile} errors={profileErrors}/>
            <LastName register={registerProfile} errors={profileErrors}/>
            <Email register={registerProfile} errors={profileErrors} serverErrors={serverErrors}/>

            <div className="flex justify-center">
              <Button label="Save Changes" width="70%"/>
            </div>

          </form>
        </div>

        {/* Password Change Form */}
       
      </div>
  );
};

export default UserProfile;
