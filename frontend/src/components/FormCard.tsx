import imgMembers from "../assets/members.svg"
import type { IPeriods } from "../model/types";
import useFormattedDate from "../hooks/useFormattedDate"

type Props = {
  cardData: {
    meetName: string,
    periods: Array<IPeriods>,
    description: string
  },
  styleList?: string
}

const FormCard  = ({cardData, styleList}: Props) => {
  const {meetName, periods, description} = cardData
  const {date, time} = useFormattedDate(periods[0].startTime)

  return (
    <>
      <div className={`${styleList} flex flex-col gap-3 p-6 bg-secondary-bg size-[266px] rounded-2xl text-base`}>
        <div>
          <h2 className={"text-xl overflow-hidden whitespace-break-spaces text-ellipsis font-semibold max-h-[60px] "}>{meetName}</h2>
        </div>
        <div className={"flex gap-2"}>
          <span>{date} {time}</span>
        </div>
        <div className={"flex gap-2"}>
          <span className={"text-primary"}>{periods[0].bookedPlaces}<img src={imgMembers} alt="" className={"inline-block pb-1"} /></span>
          <span>уже записались</span>
        </div>
        <p className={"text-base overflow-hidden whitespace-break-spaces text-ellipsis"}>{description}...</p>
      </div>
    </>
  )
}

export default FormCard

