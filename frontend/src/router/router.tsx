import { createBrowserRouter } from "react-router-dom"
import Auth from "../pages/Auth"
import Layout from "../pages/Layout"
import Home from "../pages/Home"
import FormCreate from "../pages/FormCreate"
import FormCreated from "../pages/FormCreated"
import Profile from "../pages/Profile"
import DetailedMero from "../pages/DetailedMero"
import Form from "../pages/Form"
import FormFilledSuccess from "../pages/FormFilledSuccess"

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
        element: <FormCreate/>
      },
      {
        path: "successform",
        element: <FormCreated/>
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
        element: <Form/>
      },
      {
        path: "/filledSuccess/:id",
        element: <FormFilledSuccess/>
      }
    ]
  },

  {
    path: "/auth",
    element: <Auth />
  },

])