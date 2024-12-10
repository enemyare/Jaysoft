import type { FC } from "react";
import { createContext } from "react"
import Input from "../components/input/Input"
import logout from "../assets/logout.svg"
import FormCard from "../components/FormCard"
import addEventCard from "../assets/addEventCard.svg"
import { Link, Outlet } from "react-router-dom"
import { mock } from "../mock"


export const MeroInfoContext = createContext(mock)
const Profile: FC = () => {

  return (
    <>
      <MeroInfoContext.Provider value={mock}>
        <div className={"main-container flex flex-col gap-8"}>
          <div className={""}>
            <h1 className={"font-semibold text-[32px]"}>Личный кабинет</h1>
            <p className={"mt-3 text-secondary-text"}>Здесь вы можете изменить свою электронную почту,
              добавить имя и фамилию или управлять настройками сервиса.</p>
          </div>
          <div className={"flex flex-col gap-4"}>
            <Input type={"text"} label={"d.ivanov@rostatom.ru"} />
            <Input type={"text"} label={"Введите имя"} />
            <Input type={"text"} label={"Введите фамилию"} />
          </div>
          <button className={"base-btn w-[248px] bg-danger"}>
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
              mock.map((card) =>
                (
                  <Link to={`/detailedmero/${card.meroId}`} key={card.id} >
                    <div key={card.id}>
                      <FormCard
                        id={card.id}
                        title={card.title}
                        date={card.date}
                        time={card.time}
                        members={card.members}
                        description={card.description}
                        meroId={card.meroId}
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
        <Outlet />
      </MeroInfoContext.Provider>
    </>
  )
}

export default Profile