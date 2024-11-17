import type { FC } from "react"


const FormCard  = ({title, date, time, members, description}: FormCardProps) => {

  return (
    <>
      <div>
        <h2>{title}</h2>
        <div>
          <span>{date}</span>
          <span>{time}</span>
        </div>
        <div>
          <span>{members}<img src="" alt="" /></span>
          <span>уже записались</span>
        </div>
        <p>{description}</p>
      </div>
    </>
  )
}

export default FormCard