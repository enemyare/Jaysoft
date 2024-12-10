import type { FC } from "react";
import { createContext } from "react"
import Input from "../components/input/Input"
import logout from "../assets/logout.svg"
import FormCard from "../components/FormCard"
import addEventCard from "../assets/addEventCard.svg"
import { Link, Outlet } from "react-router-dom"

const mock: Array<FormCardProps> = [
  {
    id: 1,
    title: "Цифровая сила предприятия с SILA Union 2024",
    date: "12.11.2024",
    time: "10:00",
    members: 72,
    description: "Конференция «Цифровая сила предприятия с SILA Union» – крупнейшее отраслевое мероприятие в области бизнес-моделирования и цифровой трансформации, пройдет 12 ноября 2024 г. на самой инновационной площадке г. Москва. "
  },
  {
    id: 2,
    title: "Enterprise Agile Russia",
    date: "12.11.2024",
    time: "10:00",
    members: 72,
    description: "Ежегодно конференция собирает успешные примеры Agile-трансформаций крупных организаций России и зарубежья из различных отраслей с использованием всех распространённых фреймворков Enterprise Agility: SAFe, LeSS, OKR, Nexus и прочих.\n" +
      "Полная картина Enterprise Agility в России: кейсы трансформаций только крупных компаний с контурами изменений от сотни до тысяч человек, опыт из различных отраслей: ИТ, финансы, телекоммуникации, ритейл, промышленность и другие, опыт цифровизации государственных сервисов, практики и эксперты.\n" +
      "Конференция особенно полезна владельцам бизнеса и менеджерам любого уровня из крупных организаций — всем, кто управляет портфелями, программами и проектами со стороны бизнеса или ИТ."
  },
]
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
                  <Link to={`/profile/detailedmero/${card.id}`} key={card.id} >
                    <div key={card.id}>
                      <FormCard
                        id={card.id}
                        title={card.title}
                        date={card.date}
                        time={card.time}
                        members={card.members}
                        description={card.description}
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