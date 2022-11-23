import React from "react";
import { Service } from "../../../app/models/Service";
import { Button, Card, Header, Image, Segment } from "semantic-ui-react";
import { NavLink } from "react-router-dom";

interface Props {
  service: Service;
}

export default function ServiceItem({ service }: Props) {
  return (
    <Card fluid>
      <Segment
        textAlign="center"
        attached="top"
        inverted
        style={{ border: "none", background: "#2a9d8f" }}
      >
        <Header>{service.title}</Header>
        <Image src={service.image || "/assets/service.png"} />
      </Segment>

      <Card.Content>
        <Card.Description>{service.description}</Card.Description>
      </Card.Content>
      <Card.Content extra>
        <Button as={NavLink} to={`/services/${service.id}`} positive>
          Подробнее
        </Button>
      </Card.Content>
    </Card>
  );
}
