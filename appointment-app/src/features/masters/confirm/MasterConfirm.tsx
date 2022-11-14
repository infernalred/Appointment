import React, {useEffect, useState} from "react";
import {observer} from "mobx-react-lite";
import {Button, Grid, Header, Icon, Item, Segment, Step} from "semantic-ui-react";
import {useStore} from "../../../app/store/store";
import {useNavigate} from "react-router-dom";
import {Form, Formik} from "formik";
import MyTextInput from "../../../app/common/form/MyTextInput";
import {AppointmentSlot} from "../../../app/models/AppointmentSlot";
import * as Yup from "yup";

export default observer(function MasterConfirm() {
    const {masterStore} = useStore();
    const {selected, saveAppointment} = masterStore;
    const navigate = useNavigate();
    const [confirmed, setConfirmed] = useState(false);
    const [appointment] = useState<AppointmentSlot>(new AppointmentSlot());

    const validationSchema = Yup.object({
        phone: Yup.string().trim().required("Номер телефона обязателен").length(11, "Введите корректный номер"),
        userName: Yup.string().trim().required("Имя обязательно").max(25, "Максимальное число знаков 25")
    })

    function time() {
        if (!selected) return "";

        const date = selected.start.toLocaleDateString();
        const day = selected.start.toLocaleDateString([], {
            weekday: "long",
        });
        const startTime = selected.start.toLocaleTimeString([], {
            hour: "2-digit",
            minute: "2-digit",
        });
        const endTime = selected.end.toLocaleTimeString([], {
            hour: "2-digit",
            minute: "2-digit",
        });
        return `${date}, ${day}, ${startTime}-${endTime}`;
    }

    async function handleSubmit(appointment: AppointmentSlot) {
        const updated = {...appointment, ...selected};
        await saveAppointment(updated);
        setConfirmed(true);
    }

    function isNumber(evt: React.KeyboardEvent<HTMLInputElement>): void {
        const keysAllowed: string[] = [
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
        ];
        const keyPressed: string = evt.key;

        if (!keysAllowed.includes(keyPressed)) {
            evt.preventDefault();
        }
    }

    useEffect(() => {
        if (!selected) {
            navigate("/masters/");
        }

    }, [])

    return (
        <Grid centered>
            <Grid.Column mobile={16} computer={4}>
                {confirmed && !selected ? (
                    <Step.Group ordered>
                        <Step completed>
                            <Step.Content>
                                <Step.Title>Бронирование</Step.Title>
                                <Step.Description>успешно подтверждено. Спасибо за использование онлайн
                                    сервиса</Step.Description>
                            </Step.Content>
                        </Step>
                    </Step.Group>
                ) : (
                    <Segment.Group>
                        <Segment textAlign="center"
                                 attached="top"
                                 inverted
                                 color="teal"
                                 style={{border: "none"}}>
                            <Header as={"h2"}><Icon name={"info"}/>Подтвердите расписание сеанса</Header>
                        </Segment>
                        <Segment>
                            <Item.Description>
                            <span>
                                <Icon name={"clock"}/>
                                {time()}
                            </span>
                            </Item.Description>
                        </Segment>
                        <Segment clearing>
                            <Formik
                                validationSchema={validationSchema}
                                enableReinitialize
                                initialValues={appointment}
                                validateOnMount={true}
                                onSubmit={values => handleSubmit(values)}>
                                {({handleSubmit, isValid, isSubmitting, dirty}) => (
                                    <Form className={"ui form"} onSubmit={handleSubmit} autoComplete={"off"}>
                                        <MyTextInput
                                            placeholder={"Введите ФИО"}
                                            name={"userName"}
                                            label="Как вас зовут?"
                                            maxLength={25}
                                        />
                                        <MyTextInput
                                            onKeyPress={isNumber}
                                            placeholder={"Введите номер телефона"}
                                            name={"phone"}
                                            label="Ваш номер телефона"
                                            maxLength={11}
                                        />
                                        <Button disabled={isSubmitting || !dirty || !isValid}
                                                loading={isSubmitting} floated={"right"} type="submit" color={"teal"}
                                                content={"Забронировать"}/>
                                    </Form>
                                )}
                            </Formik>
                        </Segment>
                    </Segment.Group>
                )}
            </Grid.Column>
        </Grid>
    )
})
