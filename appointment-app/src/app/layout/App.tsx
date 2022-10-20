import React from "react";
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

function App() {
    return (
        <>
            <Container fluid style={{marginTop: "2em"}}>
                <Navbar/>
                <Routes>
                    <Route path="/" element={<Navigate to="/services" replace/>}/>
                    <Route path="/services" element={<ServiceList/>}/>
                    <Route path="/services/:id" element={<ServiceDetails/>} />
                    <Route path="/masters" element={<MasterList/>}/>
                    <Route path="/masters/:id" element={<MasterDetails/>}/>
                    <Route path="/masters/:id/confirm" element={<MasterConfirm/>}/>
                </Routes>
                <footer style={{textAlign: 'center'}}>Appointment ©2022 Created by infernalred</footer>
            </Container>
        </>
    );
}

export default observer(App);
