import type { FC} from "react";
import { useState } from "react"
import logo from '../assets/logo.svg'
import arrow from '../assets/arrow.svg'
const Auth: FC = () => {
  const [isLogin, setIsLogin] = useState<boolean>(false)
  return (
    <div className={"h-screen flex justify-center items-center"}>
      <div className={"main-container max-w-[440px] flex flex-col gap-6 items-center c"}>
        <img src={logo} className={"object-cover"} />
        <div>
          {isLogin ? (
              <>
                <h1 className={"font-semibold text-[26px] text-center"}>Вход в аккаунт</h1>
                <div className={"mt-1"}>
                  <span>Впервые тут ?</span>
                  <button className={"ml-1.5 text-primary"} onClick={() => setIsLogin(!isLogin)}>Создать аккаунт</button>
                </div>
              </>
            ) :
            <>
              <h1 className={"font-semibold text-[26px] text-center"}>Создать аккаунт организатора</h1>
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
          <input type="text" className={"meta-input px-4 py-[9px]"} placeholder={"Введите почту"} />
          <button className={"arrow-btn px-2.5 py-3.5 rounded-[12px] ml-2"}><img src={arrow} alt="" /></button>
        </div>
      </div>
    </div>
  )
}

export default Auth