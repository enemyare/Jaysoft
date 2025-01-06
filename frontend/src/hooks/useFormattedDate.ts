
const useFormattedDate = (propsDate: string) => {
    const objectDate = new Date(propsDate)

    const date = objectDate.toLocaleDateString()
    const time = objectDate.toLocaleTimeString([], {timeStyle: 'short'})

    return {date, time}
}

export default  useFormattedDate