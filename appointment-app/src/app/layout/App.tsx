import React, {useEffect} from "react";
import "./App.css";
import {Navigate, Route, Routes} from "react-router-dom";
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
import DashboardPage from "../../features/dashboard/DashboardPage";
import LoadingComponent from "./LoadingComponent";
import ModalContainer from "../common/modal/ModalContainer";

function App() {
    const {userStore} = useStore();
    const {tryLogin} = userStore;

    useEffect(() => {
        tryLogin();
    }, [userStore])

    if (!userStore.appLoaded) return <LoadingComponent content={"Loading app"}/>

    return (
        <>
            <ModalContainer />
            <Container fluid style={{marginTop: "2em"}}>
                <Navbar/>
                <Routes>
                    <Route path="/" element={<Navigate to="/services" replace/>}/>
                    <Route path="/services" element={<ServiceList/>}/>
                    <Route path="/services/:id" element={<ServiceDetails/>} />
                    <Route path="/masters" element={<MasterList/>}/>
                    <Route path="/masters/:id" element={<MasterDetails/>}/>
                    <Route path="/masters/:id/confirm" element={<MasterConfirm/>}/>
                    <Route path="/auth" element={<AuthPage/>}/>
                    <Route path="/dashboard" element={<DashboardPage/>}/>
                </Routes>
                <footer style={{textAlign: 'center'}}>Appointment ©2022 Created by infernalred</footer>
            </Container>
        </>
    );
}

export default observer(App);
