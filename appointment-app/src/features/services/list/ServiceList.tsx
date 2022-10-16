import React, {Fragment, useEffect} from "react";
import {observer} from "mobx-react-lite";
import {useStore} from "../../../app/store/store";
import {Card, Container, Grid, GridRow, Header, Image, Item, Segment} from "semantic-ui-react";
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
        <Grid>
            <Grid.Column className={"centered column"} width={8}>
                <Fragment>
                    <Card>
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
                    {services.map(service => (
                        <ServiceItem key={service.id} service={service} />
                    ))}
                </Fragment>
            </Grid.Column>
        </Grid>
    )
})
