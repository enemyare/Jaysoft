import { useLocation, useNavigate, useParams } from "react-router-dom"
import type { Ianswers, ICreateForm, IInput, IPeriods, IPhormAnswer } from "../model/types"
import useFormattedDate from "../hooks/useFormattedDate"
import type { SubmitHandler} from "react-hook-form";
import { useFieldArray, useForm } from "react-hook-form"
import useSWRMutation from "swr/mutation"
import { sendRequest } from "../api/api"
import { useState } from "react"

const Form = () => {
  const [selectedPeriod, setSelectedPeriod] = useState<string | undefined>();
  const location = useLocation()
  const {id} = useParams()
  const navigate = useNavigate()
  const mero: ICreateForm = location.state
  const periods: IPeriods = mero?.periods[0]
  const {date, time, dayOfWeek} = useFormattedDate(periods?.startTime)
  // Форма
  const {register,
    handleSubmit,
    control,
    setValue} = useForm<IPhormAnswer>({
      defaultValues: {
        meroId: mero.id,
        timePeriodId: "",
        answers: mero.fields?.map((field) => ({ questionTitle: field.title, questionAnswer: "" }))
      }
    }
  )
  const {fields} = useFieldArray({
    control,
    name: "answers"
  })
  //Запрос
  const {trigger, isMutating} = useSWRMutation(
    '/api/Mero/phorm-answer/create',
    sendRequest)

  const onSubmit: SubmitHandler<any> = async (data) => { 
    try {
      await trigger(data)
      navigate(`/filledSuccess/${id}`, {state: mero})
    }catch (e){
      return <>хуй тебе</>
    }
  }

  const freePlaces = (totalPlaces: any , bookedPlaces: any) => {
    return totalPlaces-bookedPlaces
  }

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
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
              {fields.map((field, index) => (
                <input
                  className={"base-input meta-input"}
                  key={field.id}
                  type={"text"}
                  placeholder={field.questionTitle}
                  {...register(`answers.${index}.questionAnswer`)}
                />
              ))}
            </div>
          </div>
          <div>
            <h3 className={"mb-3"}>Нажмите на время, в которое хотите посетить мероприятие:</h3>
            <div className={"flex flex-col gap-4"}>
              {
                mero.periods.map((period, index) => (
                  <div className={`flex gap-2 font-medium cursor-pointer`} key={period.id} onClick={()=>{
                    setSelectedPeriod(period.id)
                    setValue("timePeriodId", period.id || "")
                  }}>
                    <span className={`${selectedPeriod === period.id ? "bg-primary text-white": ""} block meta-input px-4 py-[9px] text-[16px] leading-[22px] text-black`}>{index+1}</span>
                    <div className={`${selectedPeriod === period.id ? "bg-primary text-white": ""} meta-input base-input`}>{dayOfWeek}, {date} в {time}. Осталось {freePlaces(period.totalPlaces, period.bookedPlaces)}</div>
                  </div>
                ))
              }
            </div>
          </div>
          <div className={"flex flex-col gap-4"}>
            <button disabled={isMutating} className={"primary-responsiveness base-btn"} type={"submit"} >Зарегистрироваться</button>
            <button className={"white-responsiveness border border-primary-text base-btn text-black bg-background"} >
              На главную
            </button>
          </div>
        </div>
      </form>
    </>
  )
}

export default Form