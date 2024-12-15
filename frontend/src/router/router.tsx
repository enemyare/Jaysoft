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
import EditMero from "../pages/EditMero"

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
        path: "createForm",
        element: <FormCreate/>
      },
      {
        path: "successForm",
        element: <FormCreated/>
      },
      {
        path: "profile",
        element:
          <Profile/>,

      },
      {
        path: "detailedMero/:id",
        element: <DetailedMero/>
      },
      {
        path: "form/:id",
        element: <Form/>
      },
      {
        path: "/filledSuccess/:id",
        element: <FormFilledSuccess/>
      },
      {
        path: "editMero",
        element: <EditMero/>
      }
    ]
  },

  {
    path: "/auth",
    element: <Auth />
  },

])