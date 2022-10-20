import React from "react";
import {Header, Segment, Item} from "semantic-ui-react";
import Master from "../../../app/models/Master";

interface Props {
    master: Master
}

export default function MasterDetailsHeader({master}: Props) {
    return (
        <Segment.Group>
            <Header textAlign="center" as={"h1"}>
                {master.displayName}
            </Header>
            <Segment>
                <Item.Image size={"large"} src={master.image || "/assets/user.png"}/>
            </Segment>
        </Segment.Group>
    )
}
