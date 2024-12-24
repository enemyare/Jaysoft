import { useLocation, useNavigate, useParams } from "react-router-dom"
import Input from "../components/input/Input"
import type { ICreateForm, IPeriods } from "../model/types"
import useFormattedDate from "../hooks/useFormattedDate"
import { useForm } from "react-hook-form"


const Form = () => {
  const location = useLocation()
  const {id} = useParams()
  const navigate = useNavigate()
  const mero: ICreateForm = location.state
  const fields: any = mero.fields
  const {register, handleSubmit, formState} = useForm(
    {

    }
  )
  const periods: IPeriods = mero?.periods[0]
  const {date, time} = useFormattedDate(periods?.startTime)
  return (
    <>
      <div className={"main-container flex flex-col gap-8"}>
        <div className={"flex justify-between"}>
          <h1 className={"font-semibold text-[32px]"}>{mero.meetName}</h1>
        </div>
        <div className={"flex flex-col gap-4"}>
          <div className={"flex gap-1 font-medium"}>
            <span>{date} {time}</span>
          </div>
        </div>
        <p className={""}>
          {mero.description}
        </p>
        <div>
          <h3 className={"mb-3"}>Введите необходимую информацию в поля ниже:</h3>
          <div className={"flex flex-col gap-4"}>
            {fields.map((item: any) =>
              <Input
                key={mero.id}
                type={"text"}
                label={item.label}
              />
            )}
          </div>
        </div>
        <div>
          {/*<h3 className={"mb-3"}>Нажмите на время, в которое хотите посетить мероприятие:</h3>*/}
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