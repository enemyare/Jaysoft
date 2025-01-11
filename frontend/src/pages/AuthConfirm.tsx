import type { FC} from "react";
import logo from '../assets/logo.svg'
import arrow from '../assets/arrow.svg'
import type { SubmitHandler} from "react-hook-form";
import { useForm } from "react-hook-form"
import type { IauthCodeForm } from "../model/types"
import useSWRMutation from "swr/mutation"
import { useLocation, useNavigate } from "react-router-dom"
import { sendRequest } from "../api/api"
import { useAppDispatch, useAppSelector } from "../app/hooks"
import { userAuth } from "../app/slices/slices"


const AuthConfirm: FC = () => {
  const state = useAppSelector((state) => state.user)
  const dispatch = useAppDispatch()
  const location = useLocation()
  const {email} = location.state || {}
  const navigate = useNavigate()
  const {register, handleSubmit, formState} = useForm<IauthCodeForm>()
  const  {data, trigger, isMutating, error} = useSWRMutation(
    '/api/User/log-in',
    sendRequest
  )

  // обработчик для отправки кода
  const onSumbit: SubmitHandler<IauthCodeForm> = async (data) => {
    try {
      const response = await trigger(data)
      if (response.ok ) {
        response.json().then(res => {
          localStorage.setItem("userId", res.id)
          localStorage.setItem("userEmail", res.email)
        })
        navigate('/')
      } else {
        console.error('Ошибка:', response);
      }
    }
    catch (err){
        console.error('Произошла ошибка:', err);
      }
  }

  return (
    <div className={"h-screen flex justify-center items-center"}>
      <div className={"main-container max-w-[440px] flex flex-col gap-6 items-center c"}>
        <img src={logo} className={"object-cover"} />
        <div>
          <h1 className={"font-semibold text-[26px] text-center spread "}>Почти всё</h1>
        </div>
        <h3 className={"text-center"}>
          Мы отправили код на почту {state.userEmail}
        </h3>
        <div>
          <form onSubmit={handleSubmit(onSumbit)}>
            <input
              type="text" className={"meta-input px-4 py-[9px]"}
              placeholder={"Введите код"}
              {...register('authCode', {
                required: 'Это поле является обязательным'
              })} />
            <button disabled={isMutating} className={"primary-responsiveness arrow-btn px-2.5 py-3.5 rounded-[12px] ml-2"}><img src={arrow} alt="" /></button>
          </form>
        </div>
      </div>
    </div>
  )
}

export default AuthConfirm