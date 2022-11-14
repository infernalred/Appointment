import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import {
  Button,
  Header,
  Icon,
  Segment,
  Item,
  Grid,
} from "semantic-ui-react";
import { useStore } from "../../../app/store/store";
import MasterDetailsSlots from "./MasterDetailsSlots";
import { SlotParams } from "../../../app/models/SlotParams";
import { useNavigate } from "react-router-dom";

const columnStyle = {
  margin: 0,
  padding: 0,
  borderTop: "4px solid rgb(59, 179, 189)",
  background: "rgb(243, 250, 251)",
  textAlign: "center",
};

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
        Время приема:
      </Header>
      <Header textAlign="center" as={"h1"} style={{ marginTop: -20 }}>
        {weekLabelDate}
      </Header>
      <Item.Group>
        <Item>
          <Button.Group fluid>
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
            {selected && (
              <Button
                positive
                onClick={() => navigate(`/masters/${id}/confirm`)}
              >
                <Icon name={"check"} />
                Подтвердите выбор
              </Button>
            )}
          </Button.Group>
        </Item>
      </Item.Group>
      <Segment secondary>
        <Grid columns={7} divided>
          {days.map((day) => (
            <Grid.Column key={day.getDate()} style={columnStyle}>
              <Header
                as={"h4"}
                textAlign="center"
                attached="top"
              >
                {day.toLocaleDateString([], { weekday: "short" })}
                <br></br>
                {day.getDate()}
              </Header>
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
            </Grid.Column>
          ))}
        </Grid>
      </Segment>
    </Segment.Group>
  );
});
