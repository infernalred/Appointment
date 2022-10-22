import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import SlotModel from "../../../app/models/SlotModel";
import { useStore } from "../../../app/store/store";
import { AppointmentSlot } from "../../../app/models/AppointmentSlot";

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
    <div>
      {slots.length > 0 ? (
        <Fragment>
          {slots.map((slot) => (
            <button
              className={selected?.id === slot.id ? "selected" : "buttonSlot"}
              disabled={selected && selected.id !== slot.id}
              onClick={() => handleSelected(slot)}
              key={slot.id}
            >
              {slot.start.toLocaleTimeString([], {
                hour: "2-digit",
                minute: "2-digit",
              })}
            </button>
          ))}
        </Fragment>
      ) : (
        <Fragment>
          <button disabled className={"buttonSlot"}>
            Слотов нет
          </button>
        </Fragment>
      )}
    </div>
  );
});
