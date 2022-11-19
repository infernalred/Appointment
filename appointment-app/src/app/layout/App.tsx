import React, {useEffect, useState} from "react";
import "./App.css";
import {Navigate, Route, Routes, useNavigate} from "react-router-dom";
import ServiceList from "../../features/services/list/ServiceList";
import MasterList from "../../features/masters/list/MasterList";
import {observer} from "mobx-react-lite";
import Navbar from "./Navbar";
import {Container} from "semantic-ui-react";
import ServiceDetails from "../../features/services/details/ServiceDetails";
import MasterDetails from "../../features/masters/details/MasterDetails";
import MasterConfirm from "../../features/masters/confirm/MasterConfirm";
import AuthPage from "../../features/auth/AuthPage";
import {useStore} from "../store/store";
import ProfilePage from "../../features/profile/ProfilePage";
import LoadingComponent from "./LoadingComponent";
import ModalContainer from "../common/modal/ModalContainer";
import { ToastContainer } from "react-toastify";
import ServerError from "../../features/errors/ServerError";
import PrivateRoute from "./PrivateRoute";
import AxiosInterceptorsSetup from "../api/setupInterceptors"
import NotFound from "../../features/errors/NotFound";
import { registerLocale } from "react-datepicker";

const language = Intl.DateTimeFormat().resolvedOptions().locale;
const getLocale = (language: string) => require(`date-fns/locale/${language}`)
const locale = getLocale(language);
registerLocale(language, locale.default);


function AxiosInterceptorNavigate() {
    const navigate = useNavigate();
    const [ran, setRan] = useState(false);

    if (!ran) {
        console.log("interceptors");
        AxiosInterceptorsSetup(navigate);
        setRan(true);
    }
    
    return <></>;
}

function App() {
    const {userStore} = useStore();
    const {tryLogin} = userStore;

    useEffect(() => {
        tryLogin();
    }, [userStore])

    if (!userStore.appLoaded) return <LoadingComponent content={"Loading app"}/>

    return (
        <>
            <ToastContainer position="bottom-right" hideProgressBar />
            <ModalContainer />
            <Container fluid style={{marginTop: "2em"}}>
                <Navbar/>
                <AxiosInterceptorNavigate/>
                <Routes>
                    <Route path="/profile" element={<PrivateRoute><ProfilePage/></PrivateRoute>}/>
                    <Route path="/" element={<Navigate to="/services" replace/>}/>
                    <Route path="/services" element={<ServiceList/>}/>
                    <Route path="/services/:id" element={<ServiceDetails/>} />
                    <Route path="/masters" element={<MasterList/>}/>
                    <Route path="/masters/:id" element={<MasterDetails/>}/>
                    <Route path="/masters/:id/confirm" element={<MasterConfirm/>}/>
                    <Route path="/auth" element={<AuthPage/>}/>
                    <Route path="/server-error" element={<ServerError/>} />
                    <Route path="*" element={<NotFound/>} />
                </Routes>
                <footer style={{textAlign: 'center'}}>Appointment ©2022 Created by infernalred</footer>
            </Container>
        </>
    );
}

export default observer(App);