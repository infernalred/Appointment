import React from "react";
import {observer} from "mobx-react-lite";
import {NavLink} from "react-router-dom";
import {Container, Menu} from "semantic-ui-react";
import {useStore} from "../store/store";

export default observer(function Navbar() {
    const {userStore} = useStore();
    const {isLoggedIn} = userStore;

    return (
        <Menu inverted fixed={"top"}>
            <Container>
                <Menu.Item as={NavLink} to={"/services"} name={"Услуги"} />
                <Menu.Item as={NavLink} to={"/masters"} name={"Мастера"} />
                {isLoggedIn ? (<Menu.Item position={"right"} as={NavLink} to={"/dashboard"} name={"Профиль"} />) : (
                    <Menu.Item position={"right"} as={NavLink} to={"/auth"} name={"Войти"}/>
                )}
            </Container>
        </Menu>
    );
})
