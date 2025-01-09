import type { FC} from "react";
import { useContext } from "react" 
import { Link, useLocation, useParams } from "react-router-dom"
import FormCard from "../components/FormCard"
import addEventCard from "../assets/addEventCard.svg"
import { mock } from "../mock"

const FormFilledSuccess: FC = () => {
  const {id} = useParams()
  const location = useLocation()
  const mero = location.state
  return (
    <>
      <div className={"main-container flex flex-col gap-8"}>
        <h1 className={"font-semibold text-[32px]"}>Регистрация прошла успешно</h1>
        <p className={"mt-3"}>
          Вы успешно зарегистрировались на мероприятие «{mero?.title}»,
          которое пройдёт 18.11.2024 в 10:00. Подробнее о мероприятии по карточке ниже.
          Нажмите, чтобы перейти на страницу события.
        </p>
        <div className={"flex flex-col gap-8 flex-wrap"}>
          {
            <Link to={`/detailedmero/${mero?.meroId}`} key={mero?.id}>
                <FormCard
                  cardData={mero}
                  styleList = {"w-full"}
                />
            </Link>
          }

          <Link to={"/"}>
            <button className={"base-btn"}>
              На главную
            </button>
          </Link>
        </div>
      </div>
    </>
  )
}

export default FormFilledSuccess