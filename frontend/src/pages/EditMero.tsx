import type { FC} from "react";
import { useEffect } from "react"
import addField from "../assets/addField.svg"
import dump from "../assets/dump.svg"
import { useNavigate, useParams } from "react-router-dom"
import useSWR from "swr"
import { getRequest, sendRequest } from "../api/api"
import type { SubmitHandler} from "react-hook-form";
import { useFieldArray, useForm } from "react-hook-form"
import type { ICreateForm } from "../model/types"
import useSWRMutation from "swr/mutation"



const EditMero: FC = () => {
  const {id} = useParams()
  const navigate = useNavigate()
  //ЗАПРОС
  const {data, isLoading, error, isValidating} = useSWR(
    `/api/Mero/by-id/${id}`,
    getRequest,
    {
      revalidateIfStale: false,  // Не отправлять новый запрос, если есть закешированные данные
      revalidateOnFocus: false,  // Не отправлять запрос при переключении вкладок
      revalidateOnReconnect: false,  // Не отправлять запрос при восстановлении соединения
    })

  const {trigger, error: postError} = useSWRMutation(
    `/api/Mero/update/${id}`,
    sendRequest
  )

  const onSubmit: SubmitHandler<ICreateForm> = async (data) => {
    try {
      const response = await trigger(data)
      if (response.ok){
        console.log("заебись")
      }
    }catch(e){
      console.log(e)
    }
  }

  //ФОРМА
  const {
    control,
    register,
    handleSubmit,
    reset,
    getValues,
    setValue
  } = useForm<ICreateForm>({
    defaultValues: {
      meetName: "",
      description: "",
      periods: [],
      fields: [],
    },
  })

  const {fields: periodFields, append: appendPeriods, remove: removePeriods} = useFieldArray({
    control,
    name: "periods"
  })

  const {fields, append , remove } = useFieldArray({
    control,
    name: "fields",

  })

  useEffect(() => {
    if (data) {
      reset({
        meetName: data.meetName,
        description: data.description,
        creatorEmail: data.creatorEmail,
        periods: data.periods || [],
        fields: data.fields || [],
      });
    }
  }, [data, reset]);

  const handleStartTimeChange = (index: number, type: string, value: string) => {
    const periods = getValues("periods")
    const currentStartTime = periods[index]?.startTime
    const [currentDate, currentTime] = currentStartTime.split("T")
    let newDate = currentDate || ""
    let newTime = currentTime ? currentTime.split("Z")[0] : ""

    if (type === "date") newDate = value
    if (type === "time") newTime = value

    if (newDate && newTime) {
      const combinedDateTime = `${newDate}T${newTime}:00.000Z`
      setValue(`periods.${index}.startTime`, combinedDateTime)
    }
  }

  if (isLoading){
    return <>хуй</>
  }

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className={"main-container flex flex-col gap-8"}>
          <h1 className={"font-semibold text-[32px]"}>Редактирование формы</h1>
          <p className={""}>
            {/* eslint-disable-next-line no-irregular-whitespace */}
            Выберите нужные поля и измените данные мероприятия, а затем нажмите «Сохранить изменения», чтобы они
            вступили
            в силу.
          </p>
          <div className={"flex flex-col gap-3"}>
            <h2>Название мероприятия</h2>
            <input
              type={"text"}
              placeholder={"Название мероприятия"}
              className={"base-input meta-input"}
              {...register("meetName")}
            />
          </div>
          <div className={"flex flex-col gap-3"}>
            <h2>Дата, время и вместимость</h2>
            <div>
              <div className={"flex flex-col gap-8"}>
                {
                  periodFields.map((field, index) => (
                    <div className={"flex flex-col gap-4"} key={field.id}>
                      <input
                        type={"date"}
                        placeholder={""}
                        className={"base-input meta-input"}
                        onChange={(e) => handleStartTimeChange(index,"date", e.target.value)}
                      />
                      <input
                        type={"time"}
                        placeholder={""}
                        className={"base-input meta-input"}
                        onChange={(e) => handleStartTimeChange(index, "time" , e.target.value)}
                      />
                      <input
                        type={"text"}
                        placeholder={"Количество мест в этот интервал"}
                        className={"base-input meta-input"}
                        {...register(`periods.${index}.totalPlaces`, { valueAsNumber: true })}
                      />
                    </div>
                  ))
                }
              </div>
              <div className={"flex justify-between mt-3 text-[14px] leading-5"}>
                <button
                  type={"button"}
                  onClick={() => {
                    appendPeriods({
                      startTime: "",
                      totalPlaces: 10
                    })
                  }}>
                  <img
                    src={addField}
                    alt=""
                    className={"inline mr-1.5 pb-0.5"} />
                  Добавить ещё временной интервал
                </button>
                <button type={"button"} className={"text-danger"} onClick={() => {
                  removePeriods(-1)
                }}>
                  <img
                    src={dump}
                    alt=""
                    className={"inline mr-1.5 pb-0.5"}
                  />
                  Удалить временной интервал
                </button>
              </div>
            </div>
          </div>

          <div className={"flex flex-col gap-3"}>
            <h2>Описание</h2>
            <textarea
              placeholder={"Описание"}
              className={"base-input meta-input"}
              {...register("description")}
            />
          </div>

          <div className={"flex flex-col gap-3"}>
            <h2>Вопросы посетителям</h2>
            <div className={"flex flex-col gap-4"}>
              {
                fields.map((inputObject, index) => (
                  <div key={inputObject.id}>
                    <input
                      type={inputObject.type}
                      placeholder={inputObject.title}
                      className={"base-input meta-input"}
                      {...register(`fields.${index}.title`)}
                    />
                  </div>
                ))
              }
            </div>
            <div className={"flex justify-between mt-3 text-[14px] leading-5"}>
              <button
                type={"button"}
                onClick={() => {
                  append({
                    type: "text",
                    title: "Введите название поля"
                  })
                }}>
                <img src={addField} alt="" className={"inline mr-1.5 pb-0.5"} />
                Добавить ещё поле
              </button>
              <button type={"button"} className={"text-danger"} onClick={() => {
                remove(-1)
              }}>
                <img src={dump} alt="" className={"inline mr-1.5 pb-0.5"} /> Удалить
                поле
              </button>
            </div>
          </div>

          <div className={"flex flex-col gap-4"}>
            <button type={"submit"} className={"base-btn"} >
              Сохранить изменения
            </button>
            <div className={"flex gap-4"}>
              <button type={"button"} className={"border border-primary-text base-btn text-black bg-background max-w-[424px]"}
                      onClick={()=>{navigate("/profile")}}>
                Назад без изменений
              </button>
              <button
                className={"border border-primary-text bg-primary-text text-white rounded-xl max-w-[424px] w-full"}>
                Переместить в архив
              </button>
            </div>
          </div>
        </div>
      </form>
    </>
  )
}

export default EditMero