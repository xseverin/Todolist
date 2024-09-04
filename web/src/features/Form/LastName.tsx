import React from "react";
import { useState } from "react";
import { UserIcon } from "@heroicons/react/solid";
import { FieldErrors, UseFormRegister } from "react-hook-form";
import InnerFormWrapper from "./InnerFormWrapper";
import ErrorMessage from "./ErrorMessage.tsx"; // Убедитесь, что путь правильный

interface LastNameProps {
  register: UseFormRegister<any>;
  errors: FieldErrors;
}

function LastName({ register, errors }: LastNameProps) {
  const errColor = !errors.lastName ? "icon-normal" : "icon-error";

  return (
      <div className="col-span-1">
          <ErrorMessage message={errors.lastName?.message as string | string[]}/>
      <InnerFormWrapper className="ml-1">
        <label htmlFor="lastName">
          <span className="text-white">Last name</span>
          <UserIcon className={`h-6 w-6 absolute right-3 ${errColor}`} />
        </label>
        <input
            {...register("lastName", { required: "required" })}
            type="text"
            id="lastName"
        />
      </InnerFormWrapper>
          </div>
  );
}

export default React.memo(LastName);
