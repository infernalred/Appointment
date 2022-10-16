import React from "react";
import Service from "../../../app/models/Service";
import {Button, Card, Header, Image, Segment, Item} from "semantic-ui-react";
import {NavLink} from "react-router-dom";

interface Props {
    service: Service
}

export default function ServiceItem({service}: Props) {
    return (
        <Card fluid>
            <Segment textAlign='center'
                     attached='top'
                     inverted
                     color='teal'
                     style={{border: 'none'}}
            >
                <Header>{service.title}</Header>
            </Segment>
            <Image src={service.image || "/assets/service.png"}/>
            <Card.Content>
                <Card.Description>
                    {service.description}
                </Card.Description>
            </Card.Content>
            <Card.Content extra>
                <Button as={NavLink} to={`/services/${service.id}`} positive>Подробнее</Button>
            </Card.Content>
        </Card>
    )
}
