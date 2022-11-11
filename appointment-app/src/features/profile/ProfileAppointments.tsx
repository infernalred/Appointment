import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Table } from "semantic-ui-react";
import { useStore } from "../../app/store/store";

export default observer(function ProfileAppointments() {
  const { appointmentStore } = useStore();
  const { appointments, loadMyAppointmentsByDate } = appointmentStore;

  useEffect(() => {
    loadMyAppointmentsByDate();
  }, [loadMyAppointmentsByDate]);

  return (
    <>
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
            </Table.Row>
          ))}
        </Table.Body>
      </Table>
    </>
  );
});
