import type { FC} from "react";
import { useState } from "react"
import Input from "../components/input/Input"
import addField from "../assets/addField.svg"
import dump from "../assets/dump.svg"
import { Link, Navigate } from "react-router-dom"

const CreateForm: FC = () => {
  const [isStepOne, setIsStepOne] = useState(true)
  return (
    isStepOne ?
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
              <Input type={"text"} label={"Ваша почта"} />
              <Input type={"text"} label={"Название мероприятия"} />
              <Input type={"textarea"} label={"Описание"} />
            </div>
            <div className={"flex flex-col gap-4"}>
              <Input type={"date"} />
              <Input type={"time"} label={"Время"} />
              <Input type={"text"} label={"Количество мест в этот интервал"} />
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
                <Input type={"text"} label={"Например, «Ваше ФИО»"} />
                <Input type={"text"} label={"Например, «Ваш возраст»"} />
                <Input type={"text"} label={"Например, «Ваш номер телефона»"} />
              </div>
              <div className={"flex justify-between mt-3 text-[14px] leading-5"}>
                <button><img src={addField} alt="" className={"inline mr-1.5 pb-0.5"} /> Добавить ещё поле</button>
                <button className={"text-danger"}><img src={dump} alt="" className={"inline mr-1.5 pb-0.5"} /> Удалить
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

  )
}

export default CreateForm