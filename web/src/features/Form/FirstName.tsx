import React, { useState } from "react";
import { UserIcon } from "@heroicons/react/solid";
import {FieldErrors, UseFormRegister} from "react-hook-form";
import InnerFormWrapper from "./InnerFormWrapper.tsx";
import ErrorMessage from "./ErrorMessage.tsx";

interface FirstNameProps {
  register: UseFormRegister<any>;
  errors: FieldErrors<any>;
}

function FirstName({ register, errors }: FirstNameProps) {
  const errColor = !errors.firstName ? "icon-normal" : "icon-error";

  return (
      <div className="col-span-1">
      <ErrorMessage message={errors.firstName?.message as string | string[]} className=""/>
      <InnerFormWrapper className="mr-">
        <label htmlFor="firstName">
          <span className="text-white">First name</span>
          <UserIcon className={`h-6 w-6 absolute right-3 ${errColor}`} />
        </label>
        <input
            {...register("firstName", { required: "required" })}
            type="text"
            id="firstName"
        />
      </InnerFormWrapper>
      </div>
  );
}

export default React.memo(FirstName);

