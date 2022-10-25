import React, {useEffect} from "react";
import {observer} from "mobx-react-lite";
import {Grid, Tab, Card, Image, Button} from "semantic-ui-react";
import {useStore} from "../../app/store/store";
import ServiceForm from "./form/ServiceForm";

export default observer(function DashboardServices() {
    const {serviceStore, modalStore} = useStore();
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
                    <Button size={"small"} positive content={"+"} style={{marginBottom: 10}} onClick={() => modalStore.openModal(<ServiceForm id={""}/>)}/>
                    <Card.Group doubling itemsPerRow={3}>
                        {services.map(service => (
                            <Card key={service.id}>
                                <Card.Content>
                                    <Card.Header as={"h2"} color={"teal"} textAlign={"center"}>{service.title}</Card.Header>
                                    <Image src={service.image || "/assets/service.png"} />
                                    <Card.Description>
                                        {service.description}
                                    </Card.Description>
                                </Card.Content>
                                <Card.Content extra>
                                    <Button fluid size={"small"} positive content={"Изменить"} onClick={() => modalStore.openModal(<ServiceForm id={service.id}/>)}/>
                                </Card.Content>
                            </Card>
                        ))}
                    </Card.Group>
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    )
})
