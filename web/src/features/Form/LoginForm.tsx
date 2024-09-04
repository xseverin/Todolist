import { useState } from "react";
import { useForm } from "react-hook-form";
import { useNavigate } from 'react-router-dom';
import { useAppDispatch } from "../../app/hooks";
import { resetLoading, setLoading, updateToken } from "../user/authSlice";
import { login } from "../user/authAPI";
import { message } from 'antd';
import Header from "./Header";
import Email from "./Email";
import Password from "./Password";
import Button from "./Button";


import Card from "./Card";
import image from '../../img/todo-example.png';
import SignUpButton from "./Sing up";

type FieldType = {
  email?: string;
  password?: string;
};

export const LoginForm = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const {
    register,
    handleSubmit,
    getValues,
    formState: { errors }
  } = useForm<FieldType>();

  const [serverErrors, setServerErrors] = useState<{ [key: string]: string[] }>({});

  const onFinish = async (values: FieldType) => {
    dispatch(setLoading());
    try {
      const data = await login(values.email as string, values.password as string);

      dispatch(resetLoading());

      if (data?.isSucceed && data?.data) {
        message.success("Login is successful.");
        dispatch(updateToken(data.data));
        navigate("/");
      } else if (data?.messages) {
        const newServerErrors: { [key: string]: string[] } = {};

        Object.keys(data.messages).forEach(field => {
          if (field === "DuplicateUserName") {
            newServerErrors.email = data.messages[field];
          } else if (field.startsWith("Password")) {
            if (!newServerErrors.password) {
              newServerErrors.password = [];
            }
            newServerErrors.password.push(...data.messages[field]);
          } else {
            newServerErrors[field.toLowerCase()] = data.messages[field];
          }
        });

        setServerErrors(newServerErrors);

        message.error("Login failed. Please check the fields for more details.");
      } else {
        message.error("Unexpected error occurred, please try again later.");
      }
    } catch (error) {
      dispatch(resetLoading());
      message.error("Unexpected error occurred, please try again later.");
    }
  };

  return (
      <>
        <div>
            <Card
                imageUrl={image}
                title="Welcome to Todolist"
            >
                <div
                    className="p-6 rounded-xl max-w-md w-full shadow transition-colors duration-300 ease-in-out hover:bg-gray-200">
                    <p className="text-lg font-medium leading-relaxed mb-4">
                        Welcome to Todolist, the solution for managing tasks and collaborating with others.
                    </p>
                    <p className="text-lg font-medium leading-relaxed">
                        Whether you're managing personal tasks or working with a team, TodoApp is designed to make your
                        life easier and more productive.
                    </p>
                </div>


            </Card>
        </div>

          <div className="form-container mt-0 mb-0 pb-0">
              <Header
                  mainTitle="Log in"
                  subTitle="Forgot your password?"
                  linkUrl="/forgot-password"
                  linkText="Reset it"
              />

              <div className="inner-form-container mb-0 flex items-center justify-center">
                  <form
                      onSubmit={handleSubmit(onFinish)}
                      autoComplete="off"
                  >
                  <Email register={register} errors={errors} serverErrors={serverErrors}/>
                      <Password register={register} errors={errors} getValues={getValues} showConfirmPasswordField={false} serverErrors={serverErrors} />

              <div className="flex justify-between">
                <SignUpButton width="35%" />
                <Button label="Sign in" width="70%" isSignUpStyle={true}/>
              </div>
            </form>
          </div>
        </div>
      </>
  );
};

export default LoginForm;