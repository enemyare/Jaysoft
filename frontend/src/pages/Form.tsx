import { useContext } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { MeroInfoContext } from "./Profile"
import type IInput from "../model/types"
import Input from "../components/input/Input"

const mock: Array<IInput> = [
  {
    type: "text",
    label: "Например, «Ваше ФИО»",
  },
  {
    type: "text",
    label: "Например, «Ваш возраст»",
  },
  {
    type: "text",
    label: "Например, «Ваш номер телефона»",
  },
]
const Form = () => {
  const {id} = useParams()
  const meroInfoDetails = useContext(MeroInfoContext)
  const mero = meroInfoDetails.find(event => event.meroId ===  id)
  const navigate = useNavigate()
  return (
    <>
      <div className={"main-container flex flex-col gap-8"}>
        <div className={"flex justify-between"}>
          <h1 className={"font-semibold text-[32px]"}>{mero?.title}</h1>
        </div>
        <div className={"flex flex-col gap-4"}>
          <div className={"flex gap-1 font-medium"}>
            <span>{mero?.date}</span>
            <span>{mero?.time}</span>
          </div>
        </div>
        <p className={""}>
          {mero?.description}
        </p>
        <div>
          <h3 className={"mb-3"}>Введите необходимую информацию в поля ниже:</h3>
          <div className={"flex flex-col gap-4"}>
            {mock.map((item) =>
              <Input key={item.label} type={item.type} label={item.label}></Input>
            )}
          </div>
        </div>
        <div>
          <h3 className={"mb-3"}>Нажмите на время, в которое хотите посетить мероприятие:</h3>
        </div>
        <div className={"flex flex-col gap-4"}>
          <button className={"base-btn"} onClick={()=>{navigate(`/filledSuccess/${id}`)}} >Зарегистрироваться</button>
          <button className={"border border-primary-text base-btn text-black bg-background"} >
            На главную
          </button>
        </div>
      </div>
    </>
  )
}

export default Form