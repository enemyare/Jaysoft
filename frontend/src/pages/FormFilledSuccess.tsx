import type { FC} from "react";
import { useContext } from "react" 
import { Link, useParams } from "react-router-dom"
import FormCard from "../components/FormCard"
import addEventCard from "../assets/addEventCard.svg"
import { MeroInfoContext } from "./Profile"
import { mock } from "../mock"

const FormFilledSuccess: FC = () => {
  const {id} = useParams()
  const meroInfoDetails = useContext(MeroInfoContext)
  const mero = mock.find(event => event.meroId === id)
  const styleList = 'w-full'
  return (
    <>
      <div className={"main-container flex flex-col gap-8"}>
        <h1 className={"font-semibold text-[32px]"}>Регистрация прошла успешно</h1>
        <p className={"mt-3"}>
          Вы успешно зарегистрировались на мероприятие «{mero?.title}»,
          которое пройдёт 18.11.2024 в 10:00. Подробнее о мероприятии по карточке ниже.
          Нажмите, чтобы перейти на страницу события.
        </p>
        <div className={"flex flex-col gap-8 flex-wrap"}>
          {
            <Link to={`/detailedmero/${mero.meroId}`} key={mero.id}>
                <FormCard
                  id={mero.id}
                  title={mero.title}
                  date={mero.date}
                  time={mero.time}
                  members={1}
                  description={mero.description}
                  meroId={mero.meroId}
                  styleList = {styleList}
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