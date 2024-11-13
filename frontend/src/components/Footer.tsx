import type { FC } from "react"
import { Link, NavLink } from "react-router-dom"


const Footer: FC = () => {
  return (
    <footer className={"text-[20px] flex justify-between"}>
      <div className={"flex flex-col justify-between"}>
        <h2 className={"font-semibold mb-5 text-2xl"}>Мероприятия</h2>
        <NavLink className={""} to={"/createform"}>
          <button className={"border border-primary-text bg-primary-text text-white rounded-xl px-4 py-1.5"}>Создать форму бронирования</button>
        </NavLink>
      </div>
      <div>
        <h2 className={"font-semibold mb-5 text-2xl"}>Разделы</h2>
        <nav className={"text-secondary-text"}>
          <ul className={"flex flex-col gap-2"}>
            <li>
              <NavLink to={"/"}>Главная</NavLink>
            </li>
            <li>
              <NavLink to={"/heq"}>Для организаторов</NavLink>
            </li>
          </ul>
        </nav>
      </div>
      <div>
        <h2 className={"font-semibold mb-5 text-2xl"}>Обратная связь</h2>
        <Link className={"text-secondary-text"} to={'https://forms.yandex.ru/admin/'}>Яндекс формы</Link>
      </div>
    </footer>
  )
}

export default Footer