export  interface IInput {
  type: "text" | "textarea" | "date" | "time" | "checkbox",
  label?: string | undefined,
  onClick?: () => void,
}

export interface IPeriods {
  startTime: string,
  totalPlaces: number
}

export interface ICreateForm {
  meetName: string,
  creatorEmail: string,
  description: string,
  periods: Array<IPeriods>,
  fields: Array<IInput>
}

export interface IauthForm {
  email: string
}

export interface IauthCodeForm {
  authCode: string
}