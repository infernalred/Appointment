import React, {useEffect} from "react";
import {observer} from "mobx-react-lite";
import {useStore} from "../../../app/store/store";
import * as Yup from "yup";
import {Formik, Form, ErrorMessage} from "formik";
import {Button, Header} from "semantic-ui-react";
import MyTextInput from "../../../app/common/form/MyTextInput";
import ValidationErrors from "../../errors/ValidationErrors";
import MySelectOptions from "../../../app/common/form/MySelectOptions";

export default observer(function MasterForm() {
    const {masterStore, modalStore, serviceStore} = useStore();
    const {serviceRegistry, loadServices, servicesSet, loading} = serviceStore;

    const validationSchema = Yup.object({
        displayName: Yup.string().required(),
        email: Yup.string().required().email(),
        password: Yup.string().required(),
        userName: Yup.string().required(),
        serviceId: Yup.string().required(),
    })

    useEffect(() => {
        if (serviceRegistry.size <= 1) {
            loadServices();
        }
    }, [serviceRegistry.size, loadServices])

    return (
        <Formik
            validationSchema={validationSchema}
            enableReinitialize
            initialValues={{displayName: "", email: "", password: "", userName: "", serviceId: "", error: null}}
            onSubmit={(values, {setErrors}) => masterStore.createMaster(values)
                .then(() => modalStore.closeModal())
                .catch(error => setErrors({error}))}>
            {({handleSubmit, isValid, dirty, isSubmitting, errors}) => (
                <Form className={"ui form"} onSubmit={handleSubmit} autoComplete={"off"}>
                    <Header as={"h2"} content={"Новая мастер"} color={"teal"} textAlign={"center"}></Header>
                    <MyTextInput
                        maxLength={15}
                        name={"displayName"}
                        placeholder="Введите имя"
                        label={"Отображаемое имя"}
                    />
                    <MyTextInput
                        name={"userName"}
                        placeholder="Введите логин"
                        label={"Логин"}
                    />
                    <MyTextInput
                        name={"email"}
                        placeholder="Введите почтовый адрес"
                        label={"Почтовый адрес"}
                    />
                    <MyTextInput
                        type={"password"}
                        name={"password"}
                        placeholder="Введите пароль"
                        label={"Пароль"}
                    />
                    <MySelectOptions
                        placeholder="Выберите услугу"
                        name={"serviceId"}
                        options={servicesSet}
                        loading={loading}
                    />
                    <ErrorMessage
                        name="error" render={() =>
                        <ValidationErrors errors={errors.error} />}
                    />
                    <Button
                        disabled={isSubmitting || !dirty || !isValid}
                        loading={isSubmitting} floated={"right"}
                        positive type={"submit"} content={"Сохранить"}/>
                    <Button  style={{ marginBottom: 10 }} type="button" onClick={() => modalStore.closeModal()} floated="right" content="Отмена" />
                </Form>
            )}

        </Formik>
    )
})
