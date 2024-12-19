export  interface IInput {
  type: "text" | "textarea" | "date" | "time",
  label?: string | undefined,
  onClick?: () => void,
}

export interface IPeriods {
  startTime: string,
  bookedPlaces: string,
  endTime?: string,
  totalPlaces?: number
}

export interface ICreateForm {
  id?: string,
  meetName: string,
  creatorEmail?: string,
  description: string,
  periods: Array<IPeriods>,
  fields?: Array<IInput>
}

export interface IauthForm {
  email: string
}

export interface IauthCodeForm {
  authCode: string
}

