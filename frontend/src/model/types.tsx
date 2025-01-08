export  interface IInput {
  title?: string;
  type: "text" | "textarea" | "date" | "time",
}

export interface IPeriods {
  startTime: string,
  bookedPlaces?: string,
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
  uniqueInviteCode?: string
}

export interface IauthForm {
  email: string
}

export interface IauthCodeForm {
  authCode: string
}
