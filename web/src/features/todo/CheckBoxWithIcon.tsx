import React, { useEffect, useState } from 'react';

interface CheckboxWithIconProps {
    isChecked: boolean;
    isDisabled: boolean;
    onChange: () => void;
    className?: string;
}

const CheckboxWithIcon: React.FC<CheckboxWithIconProps> = ({ isChecked, isDisabled, onChange, className }) => {
    const [isChrome, setIsChrome] = useState(false);

    useEffect(() => {
        const checkIfChrome = () => {
            const userAgent = navigator.userAgent;
            return /Chrome/.test(userAgent) && /Google Inc/.test(navigator.vendor);
        };

        setIsChrome(checkIfChrome());
    }, []);

    const labelClassName = `flex items-center space-x-2 ${className || ''}`;
    const divClassName = `w-5 h-5 border border flex items-center justify-center cursor-pointer rounded transition-opacity duration-300 ${isDisabled ? 'cursor-not-allowed opacity-50' : ''}`;
    const svgClassName = `w-4 h-4 text-gray-500 transition-opacity duration-300 ${isChecked ? 'opacity-100' : 'opacity-0'}`;

    return (
        <label className={labelClassName}>
            <input
                type="checkbox"
                className="hidden"
                checked={isChecked}
                onChange={onChange}
                disabled={isDisabled}
            />
            <div className={divClassName}>
                <svg
                    className={svgClassName}
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                    xmlns="http://www.w3.org/2000/svg"
                >
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M5 13l4 4L19 7" />
                </svg>
            </div>
        </label>
    );
};

export default CheckboxWithIcon;
