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
import AuthConfirm from "../pages/AuthConfirm"
import { Navigate } from "react-router-dom";
import type { FC, ReactNode } from "react"
import Cookies from "js-cookie"

interface PrivateRouteProps {
  children: ReactNode;
}

const isAuthenticated = (): boolean => {
  return true;
};

const PrivateRoute: FC<PrivateRouteProps> = ({ children }) => {
  return isAuthenticated() ? children : <Navigate to="/auth" replace />;
};

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
        element:
          <PrivateRoute>
            <FormCreate/>
          </PrivateRoute>
      },
      {
        path: "successForm",
        element:
          <PrivateRoute>
            <FormCreated/>
          </PrivateRoute>
      },
      {
        path: "profile",
        element:
          <PrivateRoute>
            <Profile/>
          </PrivateRoute>
      },
      {
        path: "detailedMero/:id",
        element:
            <DetailedMero/>
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
  {
    path: "/authConfirm",
    element: <AuthConfirm/>
  }

])