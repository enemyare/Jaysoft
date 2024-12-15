import type { FC} from "react";
import { useState } from "react"
import { Link } from "react-router-dom"
import copy from '../assets/copy.svg'
import qrcode from '../assets/qrcode.svg'
import QrModal from "../components/QrModal"
const FormCreated:FC = () => {
  const [isOpen, setIsOpen] = useState(false)
  return (
    <>
      <div className={"main-container flex flex-col gap-8"}>
        <h2 className={"text-secondary-text"}>Шаг 2 из 2</h2>
        <div className={""}>
          <h1 className={"font-semibold text-[32px]"}>Форма бронирования создана</h1>
          <p className={"mt-3"}>
            Чтобы отправить форму посетителям, сделайте скриншот QR-кода
            и отправьте его сотрудникам. Отсканировав код, они смогут перейти к записи на мероприятие.
          </p>
        </div>
        <div className={"flex flex-col gap-4"}>
          <button onClick={()=> setIsOpen(true)} className={" meta-input base-input rounded-[32px] text-xl text-center text-white bg-[url('assets/qrcodebg.svg')]"}>
            <img src={qrcode} alt="qrcode" className={"inline-block"} />
            Открыть QR-код
          </button>
          <QrModal isOpen={isOpen} isClose={()=>setIsOpen(false)} />
          <span className={"before:content-[''] before:border-black before:mr-4 before:w-full block text-center text-secondary-text"}>
            Или поделитесь текстовым кодом
          </span>
          <button className={"meta-input base-input rounded-[32px] text-xl text-center select-none"}
               onClick={() =>  navigator.clipboard.writeText('RWNIBNEK')}>
            RWNIBNEK
            <img src={copy} alt="" className={"inline-block ml-2.5"}/>
          </button>
        </div>

        <div>
          <Link to={"/profile"}>
            <button className={"base-btn"}>
              Перейти в личный кабинет
            </button>
          </Link>
          <Link to={"/"}>
            <button className={"border border-primary-text base-btn text-black mt-4 bg-background"}>
              На главную
            </button>
          </Link>
        </div>
      </div>
    </>
  )
}

export default FormCreated