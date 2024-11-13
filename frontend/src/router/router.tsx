import { createBrowserRouter } from "react-router-dom"
import Auth from "../pages/Auth"
import Layout from "../pages/Layout"
import Home from "../pages/Home"
import CreateForm from "../pages/CreateForm"

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
        path: "/profile",
        element: <div>Личный профайл</div>
      },
    ]
  },

  {
    path: "/auth",
    element: <Auth />
  },

])