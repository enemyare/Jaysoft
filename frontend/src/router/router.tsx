import { createBrowserRouter } from "react-router-dom"
import Auth from "../pages/Auth"
import Layout from "../pages/Layout"
import Home from "../pages/Home"
import CreateForm from "../pages/CreateForm"
import SuccessForm from "../pages/SuccessForm"
import Profile from "../pages/Profile"
import DetailedMero from "../pages/DetailedMero"
import FormPage from "../pages/FormPage"

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
        path: "createform",
        element: <CreateForm/>
      },
      {
        path: "successform",
        element: <SuccessForm/>
      },
      {
        path: "profile",
        element:
          <Profile/>,

      },
      {
        path: "detailedmero/:id",
        element: <DetailedMero/>
      },
      {
        path: "form/:id",
        element: <FormPage/>
      },
    ]
  },

  {
    path: "/auth",
    element: <Auth />
  },

])