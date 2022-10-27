import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Button, Header, Icon, Segment, Item, List } from "semantic-ui-react";
import { useStore } from "../../../app/store/store";
import MasterDetailsSlots from "./MasterDetailsSlots";
import { SlotParams } from "../../../app/models/SlotParams";
import { useNavigate } from "react-router-dom";

interface Props {
  id: string;
}

export default observer(function MasterDetailsTime({ id }: Props) {
  const { masterStore } = useStore();
  const {
    masterSlotsRegistry,
    slotLoading,
    loadMasterSlots,
    slotParams,
    setSlotParams,
    weekLabelDate,
    days,
    selected,
  } = masterStore;
  const navigate = useNavigate();

  function handleWeek(cntDays: number) {
    const updatedSlotParams = new SlotParams(
      new Date(slotParams.start.setDate(slotParams.start.getDate() + cntDays)),
      slotParams.quantityDaysNumber
    );
    setSlotParams(updatedSlotParams);
    loadMasterSlots(id);
  }
  useEffect(() => {
    if (id) {
      loadMasterSlots(id);
    }
  }, [id, loadMasterSlots]);

  return (
    <Segment.Group>
      <Header textAlign="center" as={"h1"}>
        Часы приема
      </Header>
      <Item.Group>
        <Item>
          <Button.Group>
            <Button
              primary
              icon
              onClick={() => handleWeek(-7)}
              disabled={new Date() >= slotParams.start || slotLoading}
            >
              <Icon name="angle left" />
            </Button>
            <Button
              primary
              icon
              onClick={() => handleWeek(7)}
              disabled={slotLoading}
            >
              <Icon name="angle right" />
            </Button>
          </Button.Group>
          <Item.Description>
            <h1>{weekLabelDate}</h1>
          </Item.Description>
          {selected && (
            <span>
              <span className={"selectedDay"}>Подтвердите выбор</span>
              <Button
                positive
                icon
                onClick={() => navigate(`/masters/${id}/confirm`)}
              >
                {" "}
                <Icon name={"check"} />
              </Button>
            </span>
          )}
        </Item>
      </Item.Group>
      <Segment secondary>
        <List className={"days"}>
          {days.map((day) => (
            <ul className={"ulSlots"} key={day.getDate()}>
              <Segment>
                <Header as={"h4"}>
                  {day.toLocaleDateString([], { weekday: "short" })}{" "}
                  {day.getDate()}
                </Header>
              </Segment>
              {slotLoading ? (
                <button disabled className={"buttonSlot"}>
                  Загрузка слотов
                </button>
              ) : (
                <MasterDetailsSlots
                  slots={masterSlotsRegistry.get(day.getDay()) || []}
                  id={id}
                />
              )}
            </ul>
          ))}
        </List>
      </Segment>
    </Segment.Group>
  );
});
