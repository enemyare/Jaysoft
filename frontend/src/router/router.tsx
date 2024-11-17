import { createBrowserRouter } from "react-router-dom"
import Auth from "../pages/Auth"
import Layout from "../pages/Layout"
import Home from "../pages/Home"
import CreateForm from "../pages/CreateForm"
import SuccessForm from "../pages/SuccessForm"
import Profile from "../pages/Profile"

export const router= createBrowserRouter([
  {
    path: "/",
    element: <Layout/>,
    children: [
      {
        index: true,
        element: <Home/>
      },
      {
        path: "/createform",
        element: <CreateForm/>
      },
      {
        path: "/successform",
        element: <SuccessForm/>
      },
      {
        path: "/profile",
        element: <Profile/>
      },
    ]
  },

  {
    path: "/auth",
    element: <Auth />
  },

])