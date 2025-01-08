import type { IInput } from "../../../model/types"


const BaseInput  =  (({type }: IInput) => {
  return (
    <>
      <input className={"base-input meta-input"} type={type}  />
    </>
  )
})

export default BaseInput