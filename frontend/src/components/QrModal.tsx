import { createPortal } from "react-dom"
import QRCode from "react-qr-code"
interface ModalProps{
  isOpen: boolean,
  url: string,
  isClose: () => void
}
const QrModal = ({isOpen, isClose, url}: ModalProps) => {

  if (!isOpen) return null

  return createPortal(
    <div onClick={isClose} className={"absolute top-0 left-0 flex justify-center items-center w-full min-h-full bg-[#0000004D]"}>
      <div className={"flex flex-col max-w-[340px] items-center gap-8 p-12 bg-white rounded-[32px]"}>
        <h1 className={"text-[32px] font-semibold text-center leading-[34px]"}>QR-код вашего
          мероприятия</h1>
          <QRCode
            size={240}
            style={{ height: "auto", maxWidth: "", width: "" }}
            value={url}
            viewBox={`0 0 240 240`}
          />
        <button className={"base-btn"} onClick={isClose}>Закрыть</button>
      </div>
    </div>,
    document.body
)
}

export default QrModal