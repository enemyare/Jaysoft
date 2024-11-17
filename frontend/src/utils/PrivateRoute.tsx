import { Navigate, Outlet } from "react-router-dom"


const PrivateRoute = () => {
  let auth: boolean = true
  return auth ? <Outlet/>: <Navigate to={'/auth'}/>
}

export default PrivateRoute