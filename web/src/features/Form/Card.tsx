import React from 'react';

interface CardProps {
    imageUrl: string;
    title: string;
    children: React.ReactNode;
}

const Card: React.FC<CardProps> = ({ imageUrl, title, children }) => {
    return (
        <>
            <div className="flex justify-center mt-4">
                <h1 className="title-header">
                    <span>{title}</span>
                    <span className="dot">.</span>
                </h1>
            </div>
            <div className="card -mt-100">
                <img src={imageUrl} alt={title} className="card-image"/>
                <div className="card-content">
                    <div className="card-text">{children}</div>
                </div>
            </div>
        </>
    );
};

export default Card;
