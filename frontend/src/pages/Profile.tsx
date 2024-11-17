import type { FC } from "react"
import Input from "../components/input/Input"
import logout from "../assets/logout.svg"
import FormCard from "../components/FormCard"

const Profile: FC = () => {
  const mock: FormCardProps = {
    title: "Цифровая сила предприятия с SILA Union 2024",
    date: "12.11.2024",
    time: "10:00",
    members: 72,
    description: "Конференция «Цифровая сила предприятия с SILA Union» – крупнейшее отраслевое мероприятие в области бизнес-моделирования и цифровой трансформации, пройдет 12 ноября 2024 г. на самой инновационной площадке г. Москва. "
  }
  return (
    <>
      <div className={"main-container flex flex-col gap-8"}>
        <div className={""}>
          <h1 className={"font-semibold text-[32px]"}>Личный кабинет</h1>
          <p className={"mt-3"}>Здесь вы можете изменить свою электронную почту,
            добавить имя и фамилию или управлять настройками сервиса.</p>
        </div>
        <div className={"flex flex-col gap-4"}>
          <Input type={"text"} label={"d.ivanov@rostatom.ru"} />
          <Input type={"text"} label={"Введите имя"} />
          <Input type={"text"} label={"Введите фамилию"} />
        </div>
        <div>
          <div>
            <input type="text" />
            <p>Автоматически заполнять электронную почту при создании формы</p>
          </div>
          <div>
            <input type="text" />
            <p>Автоматически заполнять основные поля сбора данных при создании формы</p>
          </div>
        </div>
        <button className={"base-btn w-[248px] bg-danger"}>
          <img src={logout} alt="" className={"inline-block mr-2.5 pb-1"} />
          Выйти из аккаунта
        </button>
      </div>
      <FormCard
        title={mock.title}
        date={mock.date}
        time={mock.time}
        members={mock.members}
        description={mock.description}
      />
    </>
  )
}

export default Profile