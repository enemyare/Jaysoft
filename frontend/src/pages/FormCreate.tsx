import type { FC } from "react"
import { useState } from "react"
import addField from "../assets/addField.svg"
import dump from "../assets/dump.svg"
import type { SubmitHandler } from "react-hook-form"
import { useFieldArray, useForm } from "react-hook-form"
import type { ICreateForm } from "../model/types"
import { useNavigate } from "react-router-dom"
import useSWRMutation from "swr/mutation"
import { sendRequest } from "../api/api"


const FormCreate: FC = () => {
  const navigate = useNavigate()
  const [isStepOne, setIsStepOne] = useState(true)
  // Форма
  const {
    control,
    register,
    handleSubmit,
    setValue,
    getValues
    } = useForm<ICreateForm>({
    defaultValues: {
      periods: [
        {
          startTime: "2026-01-29T21:05:21.370Z",
        },
      ],
      fields: [
        {
          title: "Например, «Ваше ФИО»",
          type: "text",
        },
        {
          title: "Например, «Ваш возраст»",
          type: "text",
        },
        {
          title: "Например, «Ваш номер телефона»",
          type: "text",
        },
      ],
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
  // POST-запрос
  const {data, trigger, isMutating} = useSWRMutation(
    '/api/Mero',
    sendRequest
  )


  // Обработчик для отправки формы
  const onSubmit:SubmitHandler<ICreateForm> = async (data) => {
    try {
      const response = await trigger(data)
      navigate('/successForm', {state: response})
    }catch (e){
      console.log(e)
    }
  }

  //Фукнция для объединения даты и времни
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

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
        {(isStepOne ?
            (
              <>
                <div className={"main-container flex flex-col gap-8"}>
                  <h2 className={"text-secondary-text"}>Шаг 1 из 2</h2>
                  <div className={""}>
                    <h1 className={"font-semibold text-[32px]"}>Создание формы бронирования</h1>
                    <p className={"mt-3"}>Введите основную информацию о мероприятии, а затем укажите его временные
                      интервалы и максимальное количество посетителей.</p>
                  </div>
                  <div className={"flex flex-col gap-4"}>
                    <input
                      type={"text"}
                      placeholder={"Ваша почта"}
                      className={"base-input meta-input"}
                      {...register("creatorEmail")}
                    />
                    <input
                      type={"text"}
                      placeholder={"Название мероприятия"}
                      className={"base-input meta-input"}
                      {...register("meetName")}
                    />
                    <input
                      type={"textarea"}
                      placeholder={"Описание"}
                      className={"base-input meta-input"}
                      {...register("description")}
                    />
                  </div>
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
                        type="button"
                        onClick={() => {
                          appendPeriods({
                            startTime: ""
                          })
                        }}>
                        <img
                          src={addField}
                          alt=""
                          className={"inline mr-1.5 pb-0.5"} />
                        Добавить ещё временной интервал
                      </button>
                      <button  type="button" className={"text-danger"} onClick={() => {
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
                  <button  type="button" className={"base-btn primary-responsiveness"} onClick={(e) => {
                    e.preventDefault();
                    setIsStepOne(!isStepOne)
                  }}>Далее
                  </button>
                </div>
              </>
            ) :
            (
              <>
                <div className={"main-container flex flex-col gap-8"}>
                  <h2 className={"text-secondary-text"}>Шаг 2 из 2</h2>
                  <div className={""}>
                    <h1 className={"font-semibold text-[32px]"}>Информация о посетителях</h1>
                    <p className={"mt-3"}>Вы можете указать, какую информацию хотите запрашивать у посетителей
                      мероприятия.
                      Просто введите названия полей, которые им нужно будет заполнить.</p>
                  </div>
                  <div>
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
                        type="button"
                        onClick={() => {
                          append({
                            type: "text",
                            title: "Введите название поля"
                          })
                        }}>
                        <img src={addField} alt="" className={"inline mr-1.5 pb-0.5"} />
                        Добавить ещё поле
                      </button>
                      <button  type="button" className={"text-danger"} onClick={() => {
                        remove(-1)
                      }}>
                        <img src={dump} alt="" className={"inline mr-1.5 pb-0.5"} /> Удалить
                        поле
                      </button>
                    </div>
                  </div>

                  <div className={"flex flex-col gap-4"}>
                    <button type="button"
                            className={"white-responsiveness border border-primary-text base-btn text-black mt-4 bg-background "}
                            onClick={() =>
                              setIsStepOne(!isStepOne)
                            }>Назад
                    </button>
                    <button type={"submit"} className={"base-btn primary-responsiveness"}>Создать форму
                    </button>
                  </div>

                </div>
              </>
            )
        )}
      </form>
    </>
  )
}

export default FormCreate