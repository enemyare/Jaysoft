import type { FC} from "react";
import { Link, useLocation, useParams } from "react-router-dom"
import FormCard from "../components/FormCard"
import useSWR from "swr"
import { getRequest } from "../api/api"
import type { ICreateForm } from "../model/types"
import useFormattedDate from "../hooks/useFormattedDate"

async function getProfileRequest(path: string) {
  const url =  "http://localhost:5000" + path;
  return await fetch(url, {
    method: 'GET',
    credentials: 'include',
    headers: { 'Content-Type': 'application/json' }
  }).then(res => res.json())
}

const FormFilledSuccess: FC = () => {
  const {id} = useParams()
  const {data, error, isLoading} = useSWR(
    `/api/Mero/by-id/${id}`,
    getProfileRequest
  )
  console.log(data)
  const mero: ICreateForm = data

  if (isLoading) return <>Загрузка...</>
  return (
    <>
      <div className={"main-container flex flex-col gap-8"}>
        <h1 className={"font-semibold text-[32px]"}>Регистрация прошла успешно</h1>
        <p className={"mt-3"}>
          Вы успешно зарегистрировались на мероприятие «{mero.meetName}»,
          которое пройдёт 18.11.2024 в 10:00. Подробнее о мероприятии по карточке ниже.
          Нажмите, чтобы перейти на страницу события.
        </p>
        <div className={"flex flex-col gap-8 flex-wrap"}>
           {
             <FormCard
              cardData={mero}
              styleList = {"w-full"}
             />
          }
          <Link to={"/"}>
            <button className={"base-btn primary-responsiveness"}>
              На главную
            </button>
          </Link>
        </div>
      </div>
    </>
  )
}

export default FormFilledSuccess