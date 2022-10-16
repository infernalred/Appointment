import React from "react";
import {observer} from "mobx-react-lite";
import {NavLink} from "react-router-dom";
import {Container, Menu} from "semantic-ui-react";

export default observer(function Navbar() {
    return (
        <Menu inverted fixed={"top"}>
            <Container>
                <Menu.Item as={NavLink} to={"/services"} name={"Услуги"} />
                <Menu.Item as={NavLink} to={"/masters"} name={"Мастера"} />
            </Container>
        </Menu>
    );
})
