import React, { useState } from "react";
import { EyeIcon, EyeOffIcon } from "@heroicons/react/solid";
import { FieldErrors, UseFormRegister, UseFormGetValues } from "react-hook-form";
import ErrorMessage from "./ErrorMessage.tsx";
import InnerFormWrapper from "./InnerFormWrapper.tsx";

interface PasswordInputFieldProps {
    label: string;
    id: string;
    showPassword: boolean;
    setShowPassword: React.Dispatch<React.SetStateAction<boolean>>;
    register: ReturnType<UseFormRegister<any>>;
    error: string | string[] | undefined;
    serverError: string[] | undefined;
    validate?: (value: string) => boolean | string;
}

const PasswordInputField: React.FC<PasswordInputFieldProps> = ({
                                                                   label,
                                                                   id,
                                                                   showPassword,
                                                                   setShowPassword,
                                                                   register,
                                                                   error,
                                                                   serverError,
                                                                   validate,
                                                               }) => {
    const errColor = !error && !serverError ? "icon-normal" : "icon-error";

    return (
        <>
            <ErrorMessage message={error || serverError} className="" />
            <InnerFormWrapper>
                <label htmlFor={id} className="relative block">
                    <span className="text-white">{label}</span>
                </label>
                <input
                    {...register}
                    type={showPassword ? "text" : "password"}
                    id={id}
                    className="w-full"
                />
                <div className="absolute right-4 top-1/2 transform -translate-y-1/2 cursor-pointer">
                    {showPassword ? (
                        <EyeOffIcon onClick={() => setShowPassword(false)} className={`h-6 w-6 ${errColor}`} />
                    ) : (
                        <EyeIcon onClick={() => setShowPassword(true)} className={`h-6 w-6 ${errColor}`} />
                    )}
                </div>
            </InnerFormWrapper>
        </>
    );
};

interface PasswordProps {
    register: UseFormRegister<any>;
    errors: FieldErrors<any>;
    getValues: UseFormGetValues<any>;
    showConfirmPasswordField?: boolean;
    showCurrentPasswordField?: boolean;
    serverErrors: { [key: string]: string[] };
}

const Password: React.FC<PasswordProps> = ({
                                               register,
                                               errors,
                                               getValues,
                                               showConfirmPasswordField = true,
                                               showCurrentPasswordField = false,
                                               serverErrors,
                                           }) => {
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);
    const [showCurrentPassword, setShowCurrentPassword] = useState(false);

    return (
        <div className="col-span-2">
            {showCurrentPasswordField && (
                <PasswordInputField
                    label="Current Password"
                    id="currentPassword"
                    showPassword={showCurrentPassword}
                    setShowPassword={setShowCurrentPassword}
                    register={register("currentPassword", { required: "Current password is required" })}
                    error={errors.currentPassword?.message as string | string[]}
                    serverError={serverErrors.currentPassword}
                />
            )}

            <PasswordInputField
                label="Password"
                id="password"
                showPassword={showPassword}
                setShowPassword={setShowPassword}
                register={register("password", { required: "Password is required" })}
                error={errors.password?.message as string | string[]}
                serverError={serverErrors.password}
            />

            {showConfirmPasswordField && (
                <PasswordInputField
                    label="Confirm Password"
                    id="confirmPassword"
                    showPassword={showConfirmPassword}
                    setShowPassword={setShowConfirmPassword}
                    register={register("confirmPassword", {
                        validate: value => {
                            const password = getValues("password");
                            return !password || value === password || "Passwords do not match";
                        },
                    })}
                    error={errors.confirmPassword?.message as string | string[]}
                    serverError={serverErrors.confirmPassword}
                />
            )}
        </div>
    );
};

export default React.memo(Password);
