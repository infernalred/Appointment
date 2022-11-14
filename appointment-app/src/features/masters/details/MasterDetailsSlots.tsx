import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import SlotModel from "../../../app/models/SlotModel";
import { useStore } from "../../../app/store/store";
import { AppointmentSlot } from "../../../app/models/AppointmentSlot";
import { Button } from "semantic-ui-react";

const disabledButton = {
  color: "rgb(174, 181, 188)",
  border: "1px solid transparent",
  cursor: "pointer",
  borderRadius: "2px",
  pointerEvents: "none",
  touchAction: "none",
  paddingLeft: "10%",
  paddingRight: "10%",
};

interface Props {
  slots: SlotModel[];
  id: string;
}

export default observer(function MasterDetailsSlots({ slots, id }: Props) {
  const { masterStore } = useStore();
  const { setSelected, clearSelected, selected } = masterStore;

  function handleSelected(slot: SlotModel) {
    if (selected?.id === slot.id) {
      clearSelected();
    } else {
      const newSelected = {
        id: slot.id,
        masterId: id,
        start: slot.start,
        end: slot.end,
      } as AppointmentSlot;
      setSelected(newSelected);
    }
  }

  return (
    <>
      {slots.length > 0 ? (
        <Fragment>
          {slots.map((slot) => (
            <Button
              fluid
              className={selected?.id === slot.id ? "selected" : "slot"}
              size="mini"
              key={slot.id}
              disabled={selected && selected.id !== slot.id}
              onClick={() => handleSelected(slot)}
            >
              {slot.start.toLocaleTimeString([], {
                hour: "2-digit",
                minute: "2-digit",
              })}
            </Button>
          ))}
        </Fragment>
      ) : (
        <Button fluid content="Слотов нет" size="mini" style={disabledButton} />
      )}
    </>
  );
});
