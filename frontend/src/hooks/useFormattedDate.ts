
const useFormattedDate = (propsDate: string) => {
    const objectDate = new Date(propsDate)

    const date = objectDate.toLocaleDateString()
    const time = objectDate.toLocaleTimeString([], {timeStyle: 'short'})
    const dayOfWeek = objectDate.toLocaleDateString('ru-RU', { weekday: 'long' })

    return {date, time, dayOfWeek}
}

export default  useFormattedDate