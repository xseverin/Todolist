import React from "react";

interface ErrorMessageProps {
    message: string | string[] | undefined;
    className?: string; // Allow passing custom classes
}

const ErrorMessage: React.FC<ErrorMessageProps> = ({ message, className = "" }) => {
    const containerHeight = message ? 'auto' : '20px';  // Adjust the height based on the message
    const visibility = message ? 'visible' : 'hidden';  // Keep the space but hide the content

    return (
        <div
            className={`error-messages ${className}`}  // Apply the passed className
            style={{ minHeight: containerHeight, visibility: visibility }}
        >
            {Array.isArray(message)
                ? message.map((error, index) => (
                    <React.Fragment key={index}>
                        <small className="text-red-600 text-xs">*{error}</small> <br />
                    </React.Fragment>
                ))
                : (
                    <small className="text-red-600 text-xs">*{message}</small>
                )
            }
        </div>
    );
}

export default React.memo(ErrorMessage);
