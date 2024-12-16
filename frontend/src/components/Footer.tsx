import type { FC } from "react"
import { Link } from "react-router-dom"


const Footer: FC = () => {
  return (
    <footer className={"text-[20px] flex justify-between"}>
      <div className={"flex flex-col justify-between"}>
        <h2 className={"font-semibold mb-5 text-2xl"}>Мероприятия</h2>
        <Link className={""} to={"/createForm"}>
          <button className={"border border-primary-text bg-primary-text text-white rounded-xl px-4 py-1.5"}>Создать форму бронирования</button>
        </Link>
      </div>
      <div>
        <h2 className={"font-semibold mb-5 text-2xl"}>Разделы</h2>
        <nav className={"text-secondary-text"}>
          <ul className={"flex flex-col gap-2"}>
            <li>
              <Link to={"/"}>Главная</Link>
            </li>
            <li>
              <Link to={"#"}>Для организаторов</Link>
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