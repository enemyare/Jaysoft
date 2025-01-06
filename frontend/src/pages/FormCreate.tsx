import type { FC } from "react"
import { useState } from "react"
import addField from "../assets/addField.svg"
import dump from "../assets/dump.svg"
import type { SubmitHandler } from "react-hook-form"
import { useFieldArray, useForm } from "react-hook-form"
import type { ICreateForm } from "../model/types"
import { Link } from "react-router-dom"


const FormCreate: FC = () => {
  const [isStepOne, setIsStepOne] = useState(true)

  const {
    control,
    register,
    handleSubmit,
    } = useForm<ICreateForm>({
    defaultValues: {
      periods: [
        {
          startTime: "2024-12-29T21:05:21.370Z",
          endTime: "2025-12-19T21:05:21.370Z",
          totalPlaces: 0,
        },
      ],
      fields: [
        {
          text: "Например, «Ваше ФИО»",
          type: "text",
        },
        {
          text: "Например, «Ваш возраст»",
          type: "text",
        },
        {
          text: "Например, «Ваш номер телефона»",
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


  const onSubmit:SubmitHandler<ICreateForm> = (data) => {
    console.log(data)
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
                              {...register(`periods.${index}.startTime`)}
                            />
                            <input
                              type={"time"}
                              placeholder={""}
                              className={"base-input meta-input"}
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
                        onClick={() => {
                          appendPeriods({
                            startTime: "",
                            totalPlaces: 40
                          })
                        }}>
                        <img
                          src={addField}
                          alt=""
                          className={"inline mr-1.5 pb-0.5"} />
                        Добавить ещё временной интервал
                      </button>
                      <button className={"text-danger"} onClick={() => {
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
                  <button className={"base-btn"} onClick={() => {
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
                              placeholder={inputObject.text}
                              className={"base-input meta-input"}
                              {...register(`fields.${index}.text`)}
                            />
                          </div>
                        ))
                      }
                    </div>
                    <div className={"flex justify-between mt-3 text-[14px] leading-5"}>
                      <button
                        onClick={() => {
                          append({
                            type: "text",
                            text: "Введите название поля"
                          })
                        }}>
                        <img src={addField} alt="" className={"inline mr-1.5 pb-0.5"} />
                        Добавить ещё поле
                      </button>
                      <button className={"text-danger"} onClick={() => {
                        remove(-1)
                      }}>
                        <img src={dump} alt="" className={"inline mr-1.5 pb-0.5"} /> Удалить
                        поле
                      </button>
                    </div>
                  </div>

                  <div>
                    <button className={"border border-primary-text base-btn text-black mt-4 bg-background "}
                            onClick={() =>
                              setIsStepOne(!isStepOne)
                            }>Назад
                    </button>
                  </div>
                  <Link to={"/successForm"}>
                    <button className={"base-btn"} type={"submit"} onClick={() => {
                      console.log(register.arguments)
                    }}>Создать форму
                    </button>
                  </Link>
                </div>
              </>
            )
        )}


      </form>
    </>
  )
}

export default FormCreate