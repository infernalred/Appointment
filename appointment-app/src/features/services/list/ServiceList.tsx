import React, {Fragment, useEffect} from "react";
import {observer} from "mobx-react-lite";
import {useStore} from "../../../app/store/store";
import {Card, Grid, Header, Image, Segment} from "semantic-ui-react";
import ServiceItem from "./ServiceItem";

export default observer(function ServiceList() {
    const {serviceStore} = useStore();
    const {loadServices, serviceRegistry, services, loading} = serviceStore;

    useEffect(() => {
        if (serviceRegistry.size <= 1) {
            loadServices();
        }
    }, [serviceRegistry.size, loadServices])

    if (loading) return <h1>Загрузка</h1>

    return (
        <Grid centered>
            <Grid.Column mobile={16} computer={4}>
                <Card fluid>
                    <Segment textAlign="center"
                             attached="top"
                             inverted
                             color="teal"
                             style={{border: "none"}}
                    >
                        <Header>Салон красоты</Header>
                    </Segment>
                    <Image src={"/assets/logo.png"}/>
                </Card>
                <Header textAlign="center" as={"h1"}>
                    Наши услуги:
                </Header>
                <Fragment>
                    {services.map(service => (
                        <ServiceItem key={service.id} service={service}/>
                    ))}
                </Fragment>
            </Grid.Column>
        </Grid>
    )
})
