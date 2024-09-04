// src/pages/ChangePasswordPage.tsx

import React from 'react';
import ChangePasswordForm from '../features/Form/ChangePasswordForm'; // Update the import path if necessary
import Header from '../features/Form/Header';

const ChangePasswordPage: React.FC = () => {
    return (
        <div style={{ minHeight: 'calc(78vh)' }}>
        <div className="form-container" >
        <Header
            mainTitle="Change Password"
    subTitle="Update your account password"
    linkText="Back to Profile"
    linkUrl="/profile" // Assuming this is the path to the user profile page
        />

        <ChangePasswordForm />
        </div>
        </div>
);
};

export default ChangePasswordPage;
