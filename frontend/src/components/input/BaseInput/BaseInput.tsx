import type IInput from "../../../model/types"


const BaseInput  = ({type, label, onClick}: IInput) => {
  return (
    <>
      <input className={"base-input meta-input"} type={type} placeholder={label} onClick={onClick} />
    </>
  )
}

export default BaseInput