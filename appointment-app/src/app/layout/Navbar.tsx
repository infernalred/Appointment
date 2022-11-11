import React from "react";
import { observer } from "mobx-react-lite";
import { Link, NavLink } from "react-router-dom";
import { Container, Menu, Image, Dropdown } from "semantic-ui-react";
import { useStore } from "../store/store";

export default observer(function Navbar() {
  const { userStore } = useStore();
  const { isLoggedIn, user, logout } = userStore;

  return (
    <Menu inverted fixed={"top"}>
      <Container text>
        <Menu.Item header>
          <img
            src="/assets/logo.png"
            alt="logo"
          />
        </Menu.Item>
        <Menu.Item as={NavLink} to={"/services"} name={"Услуги"} />
        <Menu.Item as={NavLink} to={"/masters"} name={"Мастера"} />

        {isLoggedIn ? (
          <Menu.Item position="right">
            <Image
              src={user?.image || "/assets/user.png"}
              avatar
              spaced="right"
            />
            <Dropdown text={user?.displayName}>
              <Dropdown.Menu direction="left">
                <Dropdown.Item
                  as={Link}
                  to={"/profile"}
                  text="Мой профиль"
                  icon="user"
                />
                <Dropdown.Item onClick={logout} text="Выйти" icon="power" />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Item>
        ) : (
          <Menu.Item position={"right"} as={Link} to={"/auth"} name={"Войти"} />
        )}
      </Container>
    </Menu>
  );
});
