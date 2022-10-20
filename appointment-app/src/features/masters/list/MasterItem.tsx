import React from "react";
import Master from "../../../app/models/Master";
import {Button, Card, Header, Image, Segment} from "semantic-ui-react";
import {NavLink} from "react-router-dom";

interface Props {
    master: Master
}

export default function MasterItem({master}: Props) {
    return (
        <Card fluid>
            <Segment textAlign="center"
                     attached="top"
                     inverted
                     color="teal"
                     style={{border: 'none'}}
            >
                <Header>{master.displayName}</Header>
            </Segment>
            <Image src={master.image || "/assets/user.png"}/>
            <Card.Content extra>
                <Button floated={"right"} as={NavLink} to={`/masters/${master.id}`} positive>Подробнее</Button>
            </Card.Content>
        </Card>
    )
}
