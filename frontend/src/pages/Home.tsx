import type { FC} from "react";
import { useState } from "react"
import arrow from '../assets/arrow.svg'
import useSWRMutation from "swr/mutation"
import { getRequest } from "../api/api"
import { useLocation, useNavigate } from "react-router-dom"


const Home: FC = () => {
  const navigate = useNavigate()
  const [inviteCode, setInviteCode] = useState<string>('')
  const {data, error, trigger, isMutating} = useSWRMutation(
    `/api/Mero/by-invite-code/${inviteCode}`,
    getRequest,

  )

  const handleSubmit =  async () => {
    const result = await trigger()
    navigate(`/form/${data.id}`, { state: data})
    try {
      if (result.ok){
        const data = await result
      }
    } catch (e) {
      console.error(e)
    }
  }


  return (
    <div className={"main-container p-16 text-center flex-col flex items-center gap-8  text-[20px]"}>
      <div className={"flex flex-col gap-6"}>
        <h1 className={"text-[48px] leading-[52px] text-center font-semibold"}>Легко регистрируйте себя и других людей на
          мероприятия</h1>
        <h3>Введите код мероприятия, чтобы присоединиться к нему</h3>
      </div>
      <div className={"flex gap-3"}>
        <input
          className={"px-6 py-3  meta-input rounded-[18px] text-[24px] "}
          placeholder={"Код мероприятия"}
          value={inviteCode}
          onChange={(e) => setInviteCode(e.target.value)} />
        <button className={"arrow-btn px-[18px]"} disabled={isMutating} onClick={handleSubmit}>
          <img className={"h-6 w-6"} src={arrow}/>
        </button>
      </div>
    </div>
  )
}

export default Home