import React, { useState } from 'react';
import logoWhitePng from '../img/todolist-high-resolution-logo-removebg-preview.png';

interface AppLogoProps {
    onClick: () => void; // Add onClick prop
}

export const AppLogo: React.FC<AppLogoProps> = ({ onClick }) => {
    const [rotateDirection, setRotateDirection] = useState(true);

    const getRandomDirection = () => Math.random() >= 0.5;

    return (
        <div className="text-white me-5" onClick={onClick} style={{ cursor: 'pointer' }}> {/* Add onClick handler */}
            <img
                src={logoWhitePng}
                alt="Logo White"
                className={`h-10 transition-transform duration-300 ease-in-out transform ${rotateDirection ? 'hover:rotate-[360deg]' : 'hover:-rotate-[360deg]'}`}
                onMouseEnter={() => setRotateDirection(getRandomDirection())}
            />
        </div>
    );
};
