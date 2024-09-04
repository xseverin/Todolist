import React from 'react';
import { Breadcrumb, Button, Typography, Space } from 'antd';
import { HomeOutlined, ArrowLeftOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';

const { Title, Paragraph } = Typography;

export const NotFoundPage = () => {
    const navigate = useNavigate();

    return (
        <div style={{ textAlign: 'center', padding: '50px', minHeight: '70vh' }}>

            

            <Title level={1} style={{ fontSize: '48px', fontWeight: 'bold' }}>
                404
            </Title>
            <Paragraph style={{ fontSize: '18px', marginBottom: '30px' }}>
                Oops! The page you're looking for doesn't exist.
            </Paragraph>

            <Space>
                <Button
                    type="primary"
                    icon={<HomeOutlined />}
                    onClick={() => navigate('/')}
                >
                    Go to Home
                </Button>
                <Button
                    icon={<ArrowLeftOutlined />}
                    onClick={() => navigate(-1)}
                >
                    Go Back
                </Button>
            </Space>
        </div>
    );
};

export default NotFoundPage;
