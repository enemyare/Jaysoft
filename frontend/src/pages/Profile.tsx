import type { FC } from "react";
import logout from "../assets/logout.svg"
import FormCard from "../components/FormCard"
import addEventCard from "../assets/addEventCard.svg"
import { Link, useNavigate } from "react-router-dom"
import type { SubmitHandler} from "react-hook-form";
import { useForm } from "react-hook-form"
import useSWR from "swr"
import { getRequest } from "../api/api"
import Cookies from "js-cookie"
import type { ICreateForm } from "../model/types"

export interface IEmail {
  email: string
}

const Profile: FC = () => {
  const navigate = useNavigate()
  const {register, handleSubmit, formState} = useForm<IEmail>()
  const params = new URLSearchParams({
    startIndex: "0",
    count: "8",
    userId: "675f93b28f7d7b469c220918"
  })
  const {data, error, isLoading} = useSWR(
    `/api/Mero/list-meros/for-creator?${params.toString()}`,
    getRequest
  )

  const onSubmit: SubmitHandler<IEmail> = (data) => {

  }

  const onLogout = () => {
    Cookies.remove('authToken', { path: '/' })
    navigate('/')
  }

  return (
    <>
      <div className={"main-container flex flex-col gap-8"}>
        <div className={""}>
          <h1 className={"font-semibold text-[32px]"}>Личный кабинет</h1>
          <p className={"mt-3 text-secondary-text"}>Здесь вы можете изменить свою электронную почту,
            добавить имя и фамилию или управлять настройками сервиса.</p>
        </div>
        <div className={"flex flex-col gap-4"}>
          <form onSubmit={handleSubmit(onSubmit)}>
            <input
              type={"text"}
              placeholder={"sultanovMi@gmail.com"}
              className={"base-input meta-input"}
              {...register(
                "email",
                {
                  required: "Это поле обязательное"
                }
              )}
            />
          </form>
        </div>
        <button className={"base-btn w-[248px] bg-danger"} onClick={()=>{onLogout()}}>
          <img src={logout} alt="" className={"inline-block mr-2.5 pb-1"} />
          Выйти из аккаунта
        </button>
      </div>
      <div className={"main-container flex flex-col gap-8"}>
        <h1 className={"font-semibold text-[32px]"}>Формы бронирования</h1>
        <p className={"mt-3 text-secondary-text"}>
          Здесь вы можете просматривать, редактировать
          и удалять созданные вами мероприятия, а также собирать данные о посетителях..
        </p>
        <div className={"flex gap-8 flex-wrap"}>
          {
            data?.map((card: ICreateForm) =>
              (
                <Link to={`/detailedmero/${card.id}`} key={card.id} >
                  <div key={card.id}>
                    <FormCard
                      meetName = {card.meetName}
                      description = {card.description}
                      periods = {card.periods}
                    />
                  </div>
                </Link>
              )
            )
          }
          <Link to={"/createform"}>
            <button className={"bg-secondary-bg size-[266px] rounded-2xl"}>
              <img src={addEventCard} alt="" className={"m-auto"} />
            </button>
          </Link>
        </div>
      </div>
    </>
  )
}

export default Profile