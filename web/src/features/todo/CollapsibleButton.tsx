import React from 'react';

interface CollapsibleButtonProps {
    id: string;
    isCollapsed: boolean;
    hasChildren: boolean;
    onToggle: (id: string) => void;
}

const CollapsibleButton: React.FC<CollapsibleButtonProps> = ({ id, isCollapsed, hasChildren, onToggle }) => {
    const handleClick = () => {
        onToggle(id);
    };

    return (
        <button
            onClick={handleClick}
            className="flex items-center text-black hover:text-gray-800 focus:outline-none"
        >
            <svg
                className={`w-4 h-4 transition-transform ${isCollapsed ? 'transform -rotate-90' : ''} ${hasChildren ? 'opacity-100' : 'opacity-0'}`}
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
            >
                <path d="M6 9l6 6 6-6" />
            </svg>
        </button>
    );
};

export default CollapsibleButton;
