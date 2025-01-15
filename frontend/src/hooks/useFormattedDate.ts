
const useFormattedDate = (propsDate: string) => {
    const localTime: string = propsDate?.slice(0, -1)
    const objectDate = new Date(localTime)

    const date = objectDate.toLocaleDateString()
    const time = objectDate.toLocaleTimeString("ru-RU", {timeStyle: 'short'})
    const dayOfWeek = objectDate.toLocaleDateString('ru-RU', { weekday: 'long' })

    return {date, time, dayOfWeek}
}

export default  useFormattedDate