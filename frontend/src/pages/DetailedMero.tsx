import { useState } from "react"
import QrModal from "../components/QrModal"
import { useNavigate, useParams } from "react-router-dom"
import useSWR from "swr"
import { getRequest } from "../api/api"
import type { ICreateForm, IPeriods } from "../model/types"
import useFormattedDate from "../hooks/useFormattedDate"


const DetailedMero = () => {
  const [isOpen, setIsOpen] = useState(false)
  const navigate = useNavigate()
  const {id} = useParams()
  const {data, error} = useSWR(`/api/Mero/by-id/${id}`, getRequest)
  const mero: ICreateForm  = data
  const periods:  IPeriods = mero?.periods[0]
  const {date, time} = useFormattedDate(periods?.startTime)

  return (
    <>
      <QrModal url={`http://localhost:5173/form/${mero?.id}`} isOpen={isOpen} isClose={()=>setIsOpen(false)} />
      <div className={"main-container flex flex-col gap-8"}>
        <div className={"flex justify-between"}>
          <h1 className={"font-semibold text-[32px]"}>{ mero?.meetName }</h1>
          <span className={"block px-4 py-1 bg-primary rounded-[66px] text-white"}>
            {  periods?.totalPlaces }
            <svg className={"inline-block"} width="20" height="20" viewBox="0 0 20 20" fill="#F36E24" xmlns="http://www.w3.org/2000/svg">
                <path
                  d="M2.5 17.5V15.8333C2.5 14.9493 2.85119 14.1014 3.47631 13.4763C4.10143 12.8512 4.94928 12.5 5.83333 12.5H9.16667C10.0507 12.5 10.8986 12.8512 11.5237 13.4763C12.1488 14.1014 12.5 14.9493 12.5 15.8333V17.5M13.3333 2.60824C14.0503 2.79182 14.6859 3.20882 15.1397 3.79349C15.5935 4.37817 15.8399 5.09726 15.8399 5.8374C15.8399 6.57754 15.5935 7.29664 15.1397 7.88131C14.6859 8.46598 14.0503 8.88298 13.3333 9.06657M17.5 17.4999V15.8333C17.4958 15.0976 17.2483 14.3839 16.7961 13.8036C16.3439 13.2233 15.7124 12.8088 15 12.6249M4.16667 5.83333C4.16667 6.71739 4.51786 7.56523 5.14298 8.19036C5.7681 8.81548 6.61594 9.16667 7.5 9.16667C8.38405 9.16667 9.2319 8.81548 9.85702 8.19036C10.4821 7.56523 10.8333 6.71739 10.8333 5.83333C10.8333 4.94928 10.4821 4.10143 9.85702 3.47631C9.2319 2.85119 8.38405 2.5 7.5 2.5C6.61594 2.5 5.7681 2.85119 5.14298 3.47631C4.51786 4.10143 4.16667 4.94928 4.16667 5.83333Z"
                  stroke="white" strokeWidth="1.25" strokeLinecap="round" strokeLinejoin="round" />
            </svg>
          </span>
        </div>
        <div className={"flex flex-col gap-4"}>
          <div className={"flex gap-1 font-medium"}>
            <span>{ date } { time }</span>
            <span>{ }</span>
          </div>
        </div>
        <p className={""}>
          { mero?.description }
        </p>
        <div className={"flex flex-col gap-4"}>
          <button className={"base-btn"} onClick={()=> setIsOpen(true)} >Поделиться мероприятием</button>
          <div className={"flex gap-4"}>
            <button className={"border border-primary-text base-btn text-black bg-background max-w-[424px]"}
                    onClick={() => {navigate('/editMero')}}
            >
              Редактировать форму
            </button>
            <button className={"border border-primary-text bg-primary-text text-white rounded-xl max-w-[424px] w-full"}>
              Получить данные о посетителях
            </button>
          </div>
        </div>
      </div>
    </>
  )
}

export default DetailedMero