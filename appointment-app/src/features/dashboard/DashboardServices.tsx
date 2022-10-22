import React, {useEffect} from "react";
import {observer} from "mobx-react-lite";
import {Grid, Tab, Card, Segment, Header, Image, Button} from "semantic-ui-react";
import {useStore} from "../../app/store/store";
import {NavLink} from "react-router-dom";

export default observer(function DashboardServices() {
    const {serviceStore} = useStore();
    const {serviceRegistry, services, loadServices} = serviceStore;

    useEffect(() => {
        if (serviceRegistry.size <= 1) {
            loadServices();
        }
    }, [serviceRegistry.size, loadServices])

    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                    <Button size={"small"} positive content={"+"}/>
                    <Card.Group itemsPerRow={2}>
                        {services.map(service => (
                            <Card key={service.id}>
                                <Card.Header>{service.title}</Card.Header>
                                <Card.Description>
                                    {service.description}
                                </Card.Description>
                                <Button size={"small"} positive>Изменить</Button>
                            </Card>
                        ))}
                    </Card.Group>
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    )
})
