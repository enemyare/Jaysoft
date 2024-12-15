import type { FC} from "react";
import { useState } from "react"
import arrow from '../assets/arrow.svg'
import useSWRMutation from "swr/mutation"
import { useNavigate } from "react-router-dom"

const inviteCodeRequest = async (url: string) => {
  const response = await fetch(url);
  if (!response.ok) {
    throw new Error("Ошибка получения данных");
  }
  return response.json();
  };


const Home: FC = () => {
  const [inviteCode, setInviteCode] = useState<string>('')
  const {trigger} = useSWRMutation(`http://localhost:5000/api/Mero/by-invite-code/${inviteCode}`, inviteCodeRequest)
  const navigate = useNavigate()

  const handleSubmit = async () => {
    try {
      const result = await trigger()
      console.log('success', result)
    }
    catch (err){
      console.log('Error', err)
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
        <button className={"arrow-btn px-[18px]"} onClick={()=>{setTimeout(()=>{navigate("form/FXCABLQA")}, 1000)}}>
          <img className={"h-6 w-6"} src={arrow}/>
        </button>
      </div>
    </div>
  )
}

export default Home