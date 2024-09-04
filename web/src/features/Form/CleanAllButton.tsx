import React from "react";

interface ClearAllButtonProps {
    reset: () => void;
    fontSize?: string; // Optional font size parameter
    width?: string; // Optional width parameter
    hidden?: boolean; // Optional hidden flag
}

const ClearAllButton: React.FC<ClearAllButtonProps> = ({ reset, fontSize = "25px", width = "50%", hidden = false }) => {
    return (
        <button
            type="button"
            onClick={() => reset()}
            className={`transition-all duration-300 ease-in-out rounded-2xl mt-4 mr-4 font-bold ${hidden ? 'opacity-0' : 'opacity-90'}`}
            style={{
                fontSize,
                lineHeight: "57px",
                height: "57px",
                letterSpacing: "2px",
                width, // Apply the width dynamically
                background: "linear-gradient(135deg, rgba(50, 50, 50, 1) 0%, rgba(70, 70, 70, 1) 100%)", // Dark gradient background
                color: "white",
                boxShadow: "0px 8px 15px rgba(0, 0, 0, 0.3)", // Default shadow
            }}
            onMouseEnter={(e) => {
                e.currentTarget.style.background = "linear-gradient(135deg, rgba(70, 70, 70, 1) 0%, rgba(90, 90, 90, 1) 100%)"; // Lighter on hover
                e.currentTarget.style.boxShadow = "0px 15px 25px rgba(0, 0, 0, 0.5)";
                e.currentTarget.style.opacity = "1";
            }}
            onMouseLeave={(e) => {
                e.currentTarget.style.background = "linear-gradient(135deg, rgba(50, 50, 50, 1) 0%, rgba(70, 70, 70, 1) 100%)"; // Revert to default dark background
                e.currentTarget.style.boxShadow = "0px 8px 15px rgba(0, 0, 0, 0.3)";
                e.currentTarget.style.opacity = "0.9";
            }}
            onMouseDown={(e) => {
                e.currentTarget.style.background = "linear-gradient(135deg, rgba(90, 90, 90, 1) 0%, rgba(110, 110, 110, 1) 100%)"; // Darker on click
                e.currentTarget.style.boxShadow = "inset 0px 5px 10px rgba(0, 0, 0, 0.5)";
                e.currentTarget.style.transform = "translateY(4px)";
            }}
            onMouseUp={(e) => {
                e.currentTarget.style.background = "linear-gradient(135deg, rgba(70, 70, 70, 1) 0%, rgba(90, 90, 90, 1) 100%)"; // Revert to hover gradient
                e.currentTarget.style.boxShadow = "0px 15px 25px rgba(0, 0, 0, 0.5)";
                e.currentTarget.style.transform = "translateY(0px)";
            }}
        >
            Clear all
        </button>
    );
};

export default React.memo(ClearAllButton);
