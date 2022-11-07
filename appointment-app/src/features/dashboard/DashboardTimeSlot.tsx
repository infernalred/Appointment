import { observer } from "mobx-react-lite";
import React, { Fragment } from "react";
import { Button, List } from "semantic-ui-react";
import { TimeSlot } from "../../app/models/TimeSlot";
import { useStore } from "../../app/store/store";
import SlotForm from "./form/SlotForm";

interface Props {
  slots: TimeSlot[];
  day: number;
}

export default observer(function DashboardTimeSlot({ slots, day }: Props) {
  const { timeSlotStore, modalStore } = useStore();
  const { delSlot, loading, getDay } = timeSlotStore;

  function time(slot: TimeSlot) {
    const startTime = slot.start.toLocaleTimeString([], {
      hour: "2-digit",
      minute: "2-digit",
    });

    const endTime = slot.end.toLocaleTimeString([], {
      hour: "2-digit",
      minute: "2-digit",
    });

    return `${startTime} - ${endTime}`;
  }

  function getDayName(dayNumber: number) {
    const day = getDay(dayNumber);

    return day?.toLocaleDateString([], { weekday: "short" });
  }

  function handleDelete(id: string) {
    delSlot(id);
  }

  return (
    <Fragment>
      <List.Item style={{ width: 150 }}>
        <Button fluid color="facebook" content={getDayName(day)} />
        {slots.map((slot) => (
          <List.Item key={slot.id}>
            <Button.Group size="mini" fluid style={{ marginBottom: 5 }}>
              <Button
                size="mini"
                content={time(slot)}
                style={{ width: 90, color: {} }}
              />
              <Button
                size="mini"
                icon="edit"
                primary
                disabled={loading}
                onClick={() =>
                  modalStore.openModal(<SlotForm id={slot.id} day={day} />)
                }
              />
              <Button
                size="mini"
                icon="remove"
                color="red"
                disabled={loading}
                onClick={() => handleDelete(slot.id)}
              />
            </Button.Group>
          </List.Item>
        ))}
        <Button
          positive
          fluid
          content="Добавить"
          onClick={() => modalStore.openModal(<SlotForm id="" day={day} />)}
        />
      </List.Item>
    </Fragment>
  );
});
