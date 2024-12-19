import type { FC} from "react";
import logo from '../assets/logo.svg'
import arrow from '../assets/arrow.svg'
import type { SubmitHandler} from "react-hook-form";
import { useForm } from "react-hook-form"
import type { IauthForm } from "../model/types"
import useSWRMutation from "swr/mutation"
import { useNavigate } from "react-router-dom"
import { sendRequest } from "../api/api"

const Auth: FC = () => {
  const navigate = useNavigate()
  const {register, handleSubmit, formState, getValues} = useForm<IauthForm>()
  const  {data, trigger, isMutating, error} = useSWRMutation(
      '/api/User/send-authcode',
      sendRequest,
      {}
    )
  if (data?.ok === true){
    navigate('/authConfirm', {state: {email: getValues('email')}})
  }


  const onSumbit: SubmitHandler<IauthForm> = (data) => {
    trigger(data)
  }

  return (
    <div className={"h-screen flex justify-center items-center"}>
      <div className={"main-container max-w-[440px] flex flex-col gap-6 items-center c"}>
        <img src={logo} className={"object-cover"} />
        <div>
          <h1 className={"font-semibold text-[26px] text-center spread "}>Вход в аккаунт</h1>
          <div className={"mt-1"}>
            <span>Впервые тут ?</span>
            <button className={"ml-1.5 text-primary"}>Создать аккаунт</button>
          </div>
        </div>
        <div>
          <form onSubmit={handleSubmit(onSumbit)}>
            <input
              type="text" className={"meta-input px-4 py-[9px]"}
              placeholder={"Введите почту"}
              {...register('email', {
                required: 'Это поле является обязательным'
              })} />
            <button disabled={isMutating} className={"arrow-btn px-2.5 py-3.5 rounded-[12px] ml-2"}><img src={arrow} alt="" /></button>
          </form>
        </div>
      </div>
    </div>
  )
}

export default Auth