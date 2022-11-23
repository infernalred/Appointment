import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { Button, Header, Icon, Item, Segment, Table } from "semantic-ui-react";
import { useStore } from "../../app/store/store";
import { AppointmentsOnDateParams } from "../../app/models/AppointmentsOnDateParams";
import { AppointmentSlot } from "../../app/models/AppointmentSlot";

export default observer(function ProfileAppointments() {
  const { appointmentStore } = useStore();
  const { appointments, loadAppointmentsByDate } = appointmentStore;
  const [date, setDate] = useState(new AppointmentsOnDateParams());

  function handleDay(cnt: number) {
    setDate(
      new AppointmentsOnDateParams(
        new Date(date.onDate.setDate(date.onDate.getDate() + cnt))
      )
    );
  }

  function time(appointment: AppointmentSlot) {
    const startTime = appointment.start.toLocaleTimeString([], {
      hour: "2-digit",
      minute: "2-digit",
    });
    const endTime = appointment.end.toLocaleTimeString([], {
      hour: "2-digit",
      minute: "2-digit",
    });
    return `${startTime}-${endTime}`;
  }

  useEffect(() => {
    loadAppointmentsByDate(date);
  }, [loadAppointmentsByDate, date]);

  return (
    <>
      <Segment>
        <Header textAlign="center" as={"h3"}>
          Посетители на: {date.onDate.toLocaleDateString()}
        </Header>
        <Item>
          <Button.Group fluid>
            <Button primary icon onClick={() => handleDay(-1)}>
              <Icon name="angle left" />
            </Button>
            <Button primary icon onClick={() => handleDay(1)}>
              <Icon name="angle right" />
            </Button>
          </Button.Group>
        </Item>
      </Segment>
      <Table celled>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>Имя</Table.HeaderCell>
            <Table.HeaderCell>Время</Table.HeaderCell>
            <Table.HeaderCell>Действия</Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {appointments.map((ap) => (
            <Table.Row key={ap.id}>
              <Table.Cell>{ap.userName}</Table.Cell>
              <Table.Cell>{time(ap)}</Table.Cell>
              <Table.Cell><Button primary content="Действие"/></Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>
      </Table>
    </>
  );
});
