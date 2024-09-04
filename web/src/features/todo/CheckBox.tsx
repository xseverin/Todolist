import React, { Component } from 'react';

interface CheckboxProps {
    isChecked: boolean;
    isDisabled: boolean;
}

class Checkbox extends Component<CheckboxProps> {
    render() {
        const { isChecked, isDisabled } = this.props;

        return (
            <span
                className={`w-5 h-5 border border-gray-500 flex items-center justify-center cursor-pointer rounded transition-opacity duration-300 ${isDisabled ? 'cursor-not-allowed' : ''}`}
            >
                <svg
                    className={`w-4 h-4 text-gray-50 transition-opacity duration-300 ${isChecked ? 'opacity-100' : 'opacity-0'}`}
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                    xmlns="http://www.w3.org/2000/svg"
                >
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M5 13l4 4L19 7" />
                </svg>
            </span>
        );
    }
}

export default Checkbox;
