import type { FC } from "react"
import { useState } from "react"
import Input from "../components/input/Input"
import addField from "../assets/addField.svg"
import dump from "../assets/dump.svg"
import { Link } from "react-router-dom"
import type { Field, SubmitHandler } from "react-hook-form"
import { useFieldArray, useForm } from "react-hook-form"
import type { ICreateForm, IInput } from "../model/types"

const mock: Array<IInput> = [
  {
    type: "text",
    label: "Например, «Ваше ФИО»",
  },
  {
    type: "date",
    label: "Например, «Ваш возраст»",
  },
  {
    type: "text",
    label: "Например, «Ваш номер телефона»",
  },
]



const FormCreate = () => {
  const [isStepOne, setIsStepOne] = useState(true)
  const [amock, setMock] = useState(mock)

  const {control, register, handleSubmit, formState} = useForm<ICreateForm>()
  const {fields: periodFields, append: appendPeriods, remove: removePeriods} = useFieldArray({
    control,
    name: "periods"
  })
  const {fields, append , remove } = useFieldArray({
    control,
    name: "fields"
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
                  <Input
                    type={"text"}
                    label={"Ваша почта"}
                    {
                      ...register(
                        "creatorEmail",
                        {required: true}
                      )
                    }
                  />
                  <Input
                    type={"time"}
                    label={"Название мероприятия"}
                    {
                      ...register(
                        "meetName",
                        {required: true}
                      )
                    }
                  />
                  <Input
                    type={"textarea"}
                    label={"Описание"}
                    {
                      ...register(
                        "description",
                        {required: true}
                      )
                    }
                  />
                </div>
                <div className={"flex flex-col gap-4"}>
                  {
                    fields.map((field, index) => (
                      <Input
                        type={field.type}
                        label={field.label}
                        key={field.id}
                      />
                    ))
                  }

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
                  <p className={"mt-3"}>Вы можете указать, какую информацию хотите запрашивать у посетителей мероприятия.
                    Просто введите названия полей, которые им нужно будет заполнить.</p>
                </div>
                <div>
                  <div className={"flex flex-col gap-4"}>
                    {
                      amock.map((inputObject) =>(
                        <Input key={inputObject.label} type={inputObject.type} label={inputObject.label} />
                      ))
                    }
                  </div>
                  <div className={"flex justify-between mt-3 text-[14px] leading-5"}>
                    <button
                      onClick={() =>{ setMock(
                        amock.concat({
                          type: "text",
                          label: "йоу"
                        })
                      )
                    }}>
                      <img src={addField} alt="" className={"inline mr-1.5 pb-0.5"} />
                      Добавить ещё поле
                    </button>
                    <button className={"text-danger"} onClick={periodFields.pop}>
                      <img src={dump} alt="" className={"inline mr-1.5 pb-0.5"}/> Удалить
                      поле
                    </button>
                  </div>
                </div>

                <div>
                  <Link to={"/successform"}>
                    <button className={"base-btn"} onClick={() => {
                    }}>Создать форму
                    </button>
                  </Link>
                  <button className={"border border-primary-text base-btn text-black mt-4 bg-background "} onClick={() =>
                    setIsStepOne(!isStepOne)
                  }>Назад
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