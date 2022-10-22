import React from "react";
import {observer} from "mobx-react-lite";
import {Button, Grid, Form, Header, Segment, Label} from "semantic-ui-react";
import {useStore} from "../../app/store/store";
import {Formik, ErrorMessage} from "formik";
import {useNavigate} from "react-router-dom";

export default observer(function AuthPage() {
    const {userStore} = useStore();
    const navigate = useNavigate();

    return (
        <Grid textAlign="center" style={{height: "50vh"}} verticalAlign="middle">
            <Grid.Column style={{maxWidth: 450}}>
                <Formik initialValues={{email: "", password: "", error: null}}
                        onSubmit={(values, {setErrors}) => userStore.login(values)
                            .then(() => navigate("/dashboard"))
                            .catch(() => setErrors({error: "Неправильный логин или пароль"}))}>
                    {({handleSubmit, handleChange, isSubmitting, errors}) => (
                        <Form className="ui form" onSubmit={handleSubmit} autoComplete={"off"} size="large">
                            <Header as="h2" color="teal" textAlign="center">
                                Войти в свой аккаунт
                            </Header>
                            <Segment stacked>
                                <Form.Input
                                    name={"email"}
                                    fluid icon="user"
                                    iconPosition="left"
                                    placeholder="Логин"
                                    onChange={handleChange}
                                />
                                <Form.Input
                                    name={"password"}
                                    fluid
                                    icon="lock"
                                    iconPosition="left"
                                    placeholder="Пароль"
                                    type="password"
                                    onChange={handleChange}
                                />
                                <ErrorMessage name={"error"} render={() =>
                                    <Label style={{marginBottom: 10}} basic color={"red"} content={errors.error}/>}/>

                                <Button type={"submit"} loading={isSubmitting} color="teal" fluid size="large">
                                    Войти
                                </Button>
                            </Segment>
                        </Form>
                    )}
                </Formik>
            </Grid.Column>
        </Grid>
    )
})
