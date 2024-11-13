import type IInput from "../../../model/types"


const DateInput  = ({ label, onClick}: IInput) => {
  return (
    <>
      <input className={"base-input meta-input"} type={"text"} placeholder={label}
             onClick={onClick} />
    </>
  )
}

export default DateInput