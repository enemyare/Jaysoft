import type { FC } from "react"

const EditMero: FC = () => {
  
  return (
    <>
      <div className={"main-container flex flex-col gap-8"}>
          <h1 className={"font-semibold text-[32px]"}>Редактирование формы</h1>
          <p className={""}>
            Выберите нужные поля и измените данные мероприятия, а затем нажмите «Сохранить изменения», чтобы они вступили в силу.
          </p>
        <div className={"flex flex-col gap-4"}>
          <button className={"base-btn"} >
            Сохранить изменения
          </button>
          <div className={"flex gap-4"}>
            <button className={"border border-primary-text base-btn text-black bg-background max-w-[424px]"}>
              Назад без изменений
            </button>
            <button className={"border border-primary-text bg-primary-text text-white rounded-xl max-w-[424px] w-full"}>
              Переместить в архив
            </button>
          </div>
        </div>
      </div>
    </>
  )
}

export default EditMero