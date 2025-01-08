import type { FC} from "react";
import logout from "../assets/logout.svg"
import FormCard from "../components/FormCard"
import addEventCard from "../assets/addEventCard.svg"
import { Link, useNavigate } from "react-router-dom"
import type { SubmitHandler} from "react-hook-form";
import { useForm } from "react-hook-form"
import useSWR from "swr"
import Cookies from "js-cookie"
import type { ICreateForm } from "../model/types"
import { useAppSelector } from "../app/hooks"

async function getProfileRequest(path: string) {
  const url =  "http://localhost:5000" + path;
  return await fetch(url, {
    method: 'GET',
    credentials: 'include',
    headers: { 'Content-Type': 'application/json' }
  }).then(res => res.json())
}

const Profile: FC = () => {
  const state = useAppSelector((state) => state.user)
  const navigate = useNavigate()
  // Форма
  const {register, handleSubmit, formState} = useForm<any>({
    defaultValues: {
      email: localStorage.getItem("userEmail")
    }
  })
  //
  let params = new URLSearchParams({
    startIndex: "0",
    count: "8"
  })


  // Параметры для запроса и запрос
  const {data, error, isLoading} = useSWR(
    `/api/Mero/list-meros/for-creator?${params.toString()}&userId=${localStorage.getItem("userId")}`,
    getProfileRequest
  )

  const onSubmit: SubmitHandler<any> = (data) => {

  }

  const onLogout = () => {
    Cookies.remove('authToken', { path: '/' })
    navigate('/')
  }

  if (error) return <>Ошибка</>

  if (isLoading) return  <>Загрузка...</>
  return (
    <>
      <div className={"main-container flex flex-col gap-8"}>
        <div className={""}>
          <h1 className={"font-semibold text-[32px]"}>Личный кабинет {state.userId}</h1>
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