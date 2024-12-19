import { FC, useState } from "react"
import { useEffect } from "react"
import logo from "../assets/logo.svg"
import profile from "../assets/profile.svg"
import { NavLink } from "react-router-dom"
import Cookies from "js-cookie"

const Header: FC = () => {
  const [isAuth, setIsAuth] = useState<boolean>(true)
  useEffect(() => {
    setIsAuth(Boolean(Cookies.get("authToken")))
  })
  console.log(Boolean(Cookies.get("authToken")))
  return (
    <header className="flex items-center text-[20px]">
      <img className="pt-1.5" src={logo} />
      <nav className={"ml-6"}>
        <ul className={"flex gap-5"}>
          <li>
            <NavLink to={"/"}>Главная</NavLink>
          </li>
          <li>
            <NavLink to={"#"}>Для организаторов</NavLink>
          </li>
        </ul>
      </nav>
      <div className="ml-auto">
        {
          isAuth ? (
            <div className={"flex gap-3 items-center"}>
              <NavLink to={"/profile"}>
                Личный кабинет
              </NavLink>
              <NavLink to={"/profile"}>
                <img src={profile} />
              </NavLink>
            </div>
          ) : (
            <NavLink to={"/auth"}>
              <button className={"border border-black rounded-xl px-4 py-1.5"}>Войти как организатор</button>
            </NavLink>
          )
        }
      </div>
    </header>
  )
}
export default Header