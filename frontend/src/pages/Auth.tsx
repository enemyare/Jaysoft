import type { FC} from "react";
import { useState } from "react"
import logo from '../assets/logo.svg'
import arrow from '../assets/arrow.svg'
import { usePostRequest } from "../api/usePostRequest"
import { SubmitHandler, useForm } from "react-hook-form"

interface IauthForm {
  email: string
}

const Auth: FC = () => {
  const [isLogin, setIsLogin] = useState<boolean>(false)

  // const {data, error, isLoading} = usePostRequest("api/User/send-authcode", {})

  const {register, handleSubmit, formState} = useForm<IauthForm>()

  const onSumbit: SubmitHandler<IauthForm> = (data) => {
    console.log(data)
  }

  return (
    <div className={"h-screen flex justify-center items-center"}>
      <div className={"main-container max-w-[440px] flex flex-col gap-6 items-center c"}>
        <img src={logo} className={"object-cover"} />
        <div>
          {isLogin ? (
              <>
                <h1 className={"font-semibold text-[26px] text-center spread "}>Вход в аккаунт</h1>
                <div className={"mt-1"}>
                  <span>Впервые тут ?</span>
                  <button className={"ml-1.5 text-primary"} onClick={() => setIsLogin(!isLogin)}>Создать аккаунт</button>
                </div>
              </>
            ) :
            <>
              <h1 className={"font-semibold text-[26px] text-center leading-[32px]"}>Создать аккаунт организатора</h1>
              <div className={"mt-1 text-center"}>
                <span>Уже есть аккаунт?</span>
                <button className={"ml-1.5 text-primary"} onClick={() => setIsLogin(!isLogin)}>Войти</button>
              </div>
            </>
          }
        </div>
        {
          isLogin ? <span className={"block"}>Отправим вам код для входа</span> :
            <span className={"block"}>Отправим вам код для регистрации</span>
        }
        <div>
          <form onSubmit={handleSubmit(onSumbit)}>
            <input
              type="text" className={"meta-input px-4 py-[9px]"}
              placeholder={"Введите почту"}
              {...register('email', {
                required: 'Это поле является обязательным'
              })} />
            <button className={"arrow-btn px-2.5 py-3.5 rounded-[12px] ml-2"}><img src={arrow} alt="" /></button>
          </form>
        </div>
      </div>
    </div>
  )
}

export default Auth