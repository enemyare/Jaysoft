import type { FC} from "react";
import logout from "../assets/logout.svg"
import FormCard from "../components/FormCard"
import addEventCard from "../assets/addEventCard.svg"
import { Link, useNavigate } from "react-router-dom"
import type { SubmitHandler} from "react-hook-form";
import { useForm } from "react-hook-form"
import useSWR from "swr"
import type { ICreateForm } from "../model/types"
import { useAppSelector } from "../app/hooks"
import { getRequest } from "../api/api"
import useFormattedDate from "../hooks/useFormattedDate"


const Profile: FC = () => {
  const state = useAppSelector((state) => state.user)
  const navigate = useNavigate()
  // Форма
  const {register, handleSubmit, formState} = useForm<any>({
    defaultValues: {
      email: localStorage.getItem("userEmail")
    }
  })
  // Параметры для запроса и запрос
  let params = new URLSearchParams({
    startIndex: "0",
    count: "8"
  })

  const {data, error, isLoading} = useSWR(
    `/api/Mero/list-meros/for-creator?${params.toString()}&userId=${localStorage.getItem("userId")}`,
    getRequest
  )

  const {data: statisticData, error: statisitcError} = useSWR(
    `/api/User/statistic?userId=${localStorage.getItem("userId")}`,
    getRequest)

  const { date, time } = data?.periods?.[0]
    ? useFormattedDate(data.periods[0].startTime)
    : { date: "", time: "" };

  const onSubmit: SubmitHandler<any> = (data) => {

  }

  const onLogout = () => {
    localStorage.clear()
    navigate('/')
    window.location.reload()
  }

  if (error) return <>ошибка</>

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
              disabled={true}
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
        <button className={"danger-responsiveness base-btn w-[248px] bg-danger"} onClick={() => {
          onLogout()
        }}>
          <img src={logout} alt="" className={"inline-block mr-2.5 pb-1"} />
          Выйти из аккаунта
        </button>
      </div>
      <div className={"main-container flex flex-col gap-8"}>
        <div className={"flex flex-col gap-3"}>
          <h1 className={"font-semibold text-[32px]"}>Формы бронирования</h1>
          <p className={"mt-3 text-secondary-text"}>
            Здесь вы можете просматривать, редактировать
            и удалять созданные вами мероприятия, а также собирать данные о посетителях..
          </p>
        </div>
        <div className={"flex gap-8 flex-wrap"}>
          {
            data?.map((card: ICreateForm) =>
              (
                <Link to={`/detailedmero/${card.id}`} key={card.id}>
                  <div key={card.id}>
                    <FormCard
                      cardData={card}
                    />
                  </div>
                </Link>
              )
            )
          }
          <Link to={"/createform"}>
            <button className={"hover:bg-[#D9D9D9] bg-secondary-bg size-[266px] rounded-2xl"}>
              <img src={addEventCard} alt="" className={"m-auto"} />
            </button>
          </Link>
        </div>
      </div>
      <div className={"main-container flex flex-col gap-8"}>
        <div className={"flex flex-col gap-3"}>
          <h1 className={"font-semibold text-[32px]"}>Статистика</h1>
          <p className={"text-secondary-text"}>
            Отслеживайте ваш вклад в организацию мероприятий.
          </p>
        </div>
        <div className={"flex gap-16"}>
          <div className={"flex flex-col gap-1"}>
            <h3 className={"leading-[44px] text-[40px] text-primary"}>{statisticData?.createdMerosCount} мероприятий</h3>
            <p className={"leading-[22px] text-[16px]"}> вы создали за последние пол года.</p>
          </div>
          <div className={"flex flex-col gap-1"}>
            <h3 className={"leading-[44px] text-[40px] text-primary"}>{statisticData?.participantsCount} человек</h3>
            <p className={"leading-[22px] text-[16px]"}> посетили эти мероприятия.</p>
          </div>
        </div>
      </div>
    </>
  )
}

export default Profile