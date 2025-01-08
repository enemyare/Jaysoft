import { createSlice } from "@reduxjs/toolkit"

interface Iuser {
  userEmail: string,
  userId: string
}

export const initialState: Iuser  = {
  userId: "",
  userEmail: "",
}

export const userSlice = createSlice({
  name: "user",
  initialState,
  reducers: {
    userAuth: (state, action) => {
      state.userId = action.payload.id
      state.userEmail = action.payload.email
    }
  }
})

export const { userAuth} = userSlice.actions;
export default userSlice.reducer;