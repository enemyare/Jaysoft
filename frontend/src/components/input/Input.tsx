import BaseInput from "./BaseInput/BaseInput"
import type { IInput } from "../../model/types"

const Input = ({type, label, onClick}: IInput)=> {
  switch (type) {
    case 'text':
      return <BaseInput type={type} label={label} onClick={onClick} />;
    case 'textarea':
      return <textarea className={"base-input meta-input min-h-20"} placeholder={label} name="" id="" cols={30}
                       rows={4}></textarea>;
    case "checkbox":
      return <></>
    case 'date':
      return <BaseInput type={type} label={label} onClick={onClick} />;
    case 'time':
      return <BaseInput type={type} label={label} onClick={onClick} />;
    default:
      return null
  }
}

export default Input