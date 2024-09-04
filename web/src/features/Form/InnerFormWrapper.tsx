import React, { useState } from "react";

interface InnerFormWrapperProps {
    children: React.ReactNode;
    className?: string;
    hasError?: boolean; // Новый пропс для указания ошибки
}

const InnerFormWrapper: React.FC<InnerFormWrapperProps> = ({ children, className, hasError }) => {
    const [active, setActive] = useState(false);

    return (
        <div
            tabIndex={0} // Make the div focusable
            onFocus={() => setActive(true)}
            onBlur={() => setActive(false)}
            className={`form__inputs transition-transform duration-75 ease-in-out transform hover:scale-105 ${active ? "input__active" : ""} ${hasError ? "input__error" : ""} ${className || ""}`}
        >
            {children}
        </div>
    );
};

export default React.memo(InnerFormWrapper);