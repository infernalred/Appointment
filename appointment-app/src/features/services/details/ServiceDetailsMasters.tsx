import React from "react";
import Master from "../../../app/models/Master";
import {Button, Card, Header, Icon, Item, List, Segment} from "semantic-ui-react";
import {NavLink} from "react-router-dom";

interface Props {
    masters: Master[]
}

export default function ServiceDetailsMasters({masters}: Props) {
    return (
        <List horizontal>
            {masters.map(master => (
                <Segment.Group key={master.id} size={"mini"}>
                    <Segment textAlign={"center"}>
                        <Item.Group >
                            <Item.Header>{master.displayName}</Item.Header>
                            <Item.Image size={"mini"} circular bordered src={master.image || "/assets/user.png"} />
                        </Item.Group>
                    </Segment>
                    <Segment>
                        <Item>
                            <Button as={NavLink} to={`/masters/${master.id}`} size={"mini"} color='teal'>Подробнее</Button>
                        </Item>
                    </Segment>
                </Segment.Group>
            ))}
        </List>
    )
}
