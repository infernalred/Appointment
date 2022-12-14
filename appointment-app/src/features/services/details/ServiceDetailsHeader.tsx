import React from "react";
import { Service } from "../../../app/models/Service";
import { Header, Segment, Icon, Item, Image } from "semantic-ui-react";
import ServiceDetailsMasters from "./ServiceDetailsMasters";

interface Props {
  service: Service;
}

export default function ServiceDetailsHeader({ service }: Props) {
  return (
    <Segment.Group>
      <Segment
        textAlign="center"
        attached="top"
        inverted
        color="teal"
        style={{ border: "none" }}
      >
        <Header>{service.title}</Header>
      </Segment>
      <Segment>
        <Item.Description>{service.description}</Item.Description>
      </Segment>
      <Segment>
        <Image src={service.image || "/assets/service.png"} fluid />
      </Segment>
      <Segment secondary>
        <span>
          <Icon name="clock" />
          Продолжительность сеанса: {service.durationMinutes} минут
        </span>
      </Segment>
      <Segment>
        <span>
          <Icon name="money" />
          Стомость: {service.price}
        </span>
      </Segment>
      <Segment
        textAlign="center"
        attached="top"
        inverted
        color="teal"
        style={{ border: "none" }}
      >
        <Header>Мастера:</Header>
      </Segment>
      <Segment secondary>
        <ServiceDetailsMasters masters={service.masters} />
      </Segment>
    </Segment.Group>
  );
}
