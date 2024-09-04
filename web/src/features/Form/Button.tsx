import React from "react";
import { buttonStyles } from "./buttonStyles"; // Путь к файлу может отличаться в зависимости от структуры проекта

interface ButtonProps {
    label: string; // Text on the button
    fontSize?: string; // Optional font size parameter
    width?: string; // Optional width parameter
    isSignUpStyle?: boolean; // Flag to apply SignUpButton style
}

const Button: React.FC<ButtonProps> = ({ label, fontSize = "25px", width = "50%", isSignUpStyle = false }) => {
    // Choose the style based on the flag
    const appliedStyle = isSignUpStyle ? buttonStyles.signUp : buttonStyles.default;

    return (
        <button
            type="submit"
            className="transition-all duration-300 ease-in-out rounded-2xl mt-4 font-bold"
            style={{
                fontSize,
                lineHeight: "57px",
                height: "57px",
                letterSpacing: "2px",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                width,
                background: appliedStyle.background,
                color: "white",
                opacity: 0.9,
                boxShadow: appliedStyle.boxShadow,
            }}
            onMouseEnter={(e) => {
                e.currentTarget.style.background = appliedStyle.hoverBackground;
                e.currentTarget.style.boxShadow = "0px 15px 25px rgba(0, 0, 0, 0.6)";
                e.currentTarget.style.opacity = "1";
            }}
            onMouseLeave={(e) => {
                e.currentTarget.style.background = appliedStyle.background;
                e.currentTarget.style.boxShadow = appliedStyle.boxShadow;
                e.currentTarget.style.opacity = "0.9";
            }}
            onMouseDown={(e) => {
                e.currentTarget.style.background = appliedStyle.activeBackground;
                e.currentTarget.style.boxShadow = appliedStyle.activeBoxShadow;
                e.currentTarget.style.transform = "translateY(4px)";
            }}
            onMouseUp={(e) => {
                e.currentTarget.style.background = appliedStyle.hoverBackground;
                e.currentTarget.style.boxShadow = "0px 15px 25px rgba(0, 0, 0, 0.6)";
                e.currentTarget.style.transform = "translateY(0px)";
            }}
        >
            {label}
        </button>
    );
};

export default React.memo(Button);
