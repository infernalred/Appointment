import React from "react";
import { Header, Segment, Image, Grid } from "semantic-ui-react";
import Master from "../../../app/models/Master";

interface Props {
  master: Master;
}

export default function MasterDetailsHeader({ master }: Props) {
  return (
    <Grid centered>
      <Grid.Column width={8}>
        <Segment.Group>
          <Segment
            textAlign="center"
            attached="top"
            inverted
            color="teal"
            style={{ border: "none" }}
          >
            <Header>{master.displayName}</Header>
          </Segment>
          <Segment>
            <Image src={master.image || "/assets/user.png"} fluid />
          </Segment>
        </Segment.Group>
      </Grid.Column>
    </Grid>
  );
}
