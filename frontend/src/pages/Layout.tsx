import type { FC } from "react"
import Header from "../components/Header"
import Footer from "../components/Footer"
import PrivateRoute from "../utils/PrivateRoute"

const Layout: FC = () => {
  return (
    <div
      className="font-[inter] text-xl min-h-screen	 max-w-screen-xl mx-auto px-40 pt-6 pb-12 flex flex-col gap-12 justify-between">
      <Header />
        <PrivateRoute/>
      <Footer />
    </div>
  )
}

export default Layout