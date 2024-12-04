import imgMembers from "../assets/members.svg"

const FormCard  = ({title, date, time, members, description}: FormCardProps) => {

  return (
    <>
      <div className={"flex flex-col gap-3 p-6 bg-secondary-bg size-[266px] rounded-2xl text-base"}>
        <div>
          <h2 className={"text-xl overflow-hidden whitespace-break-spaces text-ellipsis font-semibold h-[60px] "}>{title}</h2>
        </div>
        <div className={"flex gap-2"}>
          <span>{date}</span>
          <span>{time}</span>
        </div>
        <div className={"flex gap-2"}>
          <span className={"text-primary"}>{members}<img src={imgMembers} alt="" className={"inline-block pb-1"} /></span>
          <span>уже записались</span>
        </div>
        <p className={"text-base overflow-hidden whitespace-break-spaces text-ellipsis"}>{description}...</p>
      </div>
    </>
  )
}

export default FormCard

