import React from 'react';
import { Alert } from 'antd';

const InfoBanner: React.FC = () => {
    return (
        <Alert
            message="Information"
            description="You can see the todo lists that have been shared with you below. Expand the sections to view the details of each todo list."
            type="info"
            showIcon
            style={{ marginBottom: '16px' }}
        />
    );
};

export default InfoBanner;
