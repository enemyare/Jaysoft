import type { Action, ThunkAction } from "@reduxjs/toolkit"
import {  configureStore } from "@reduxjs/toolkit"
import userReducer from "../app/slices/slices"

//const rootReducer = combineSlices()

//export type RootState = ReturnType<typeof rootReducer>

//export const makeStore = (preloadedState?: Partial<RootState>) => {
//  const store = configureStore({
//    reducer: rootReducer,

//  })
//  setupListeners(store.dispatch)
//  return store
//}

export const store = configureStore({
  reducer: {
    user: userReducer
  }
})

export type AppDispatch =typeof store.dispatch
export type RootState = ReturnType<typeof store.getState>
export type AppThunk<ThunkReturnType = void> = ThunkAction<
  ThunkReturnType,
  RootState,
  unknown,
  Action<string>
>
