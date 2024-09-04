import React from "react";
import { FieldErrors, UseFormRegister } from "react-hook-form";
import { AtSymbolIcon } from "@heroicons/react/solid";
import ErrorMessage from "./ErrorMessage";
import InnerFormWrapper from "./InnerFormWrapper.tsx";

interface EmailProps {
    register: UseFormRegister<any>;
    errors: FieldErrors<any>;
    serverErrors: { [key: string]: string[] };
}

function Email({ register, errors, serverErrors }: EmailProps) {

    const errColor = !(errors.emailR || errors.email || serverErrors.email) ? "icon-normal" : "icon-error";

    return (
        <div className="col-span-2">
            <ErrorMessage message={errors.email?.message || serverErrors.email} className="col-span-2" />
<InnerFormWrapper>
                <label htmlFor="email" className="relative block">
                    <span className="text-white">Email</span>
                    <small className="dark:text-red-400 text-red-600 ml-2 text-xs">
                        {errors.emailR && "*" + errors?.emailR?.message}
                    </small>
                    <AtSymbolIcon
                        className={`h-6 w-6 absolute right-4 ${errColor}`}
                    />
                </label>
                <input
                    {...register("email", { required: "Email is required" })}
                    type="email"
                    id="email"
                    className="w-full border-none outline-none"
                />
</InnerFormWrapper>
        </div>
    );
}

export default React.memo(Email);
