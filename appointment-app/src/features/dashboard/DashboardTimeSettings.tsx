import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { Button, List, Segment } from "semantic-ui-react";
import { useStore } from "../../app/store/store";
import DashboardTimeSlot from "./DashboardTimeSlot";

export default observer(function DashboardTimeSettings() {
  const { timeSlotStore } = useStore();
  const { loadMySlots, slots } = timeSlotStore;
  const [daysOfWeek] = useState<number[]>([1, 2, 3, 4, 5, 6, 0]);

  useEffect(() => {
    loadMySlots();
  }, [loadMySlots]);

  return (
    <Segment>
      <Button fluid color="teal" content="Рабочее время:"/>
      
      <List horizontal celled>
        {daysOfWeek.map((day) => (
          <List.Item
            key={day}
            style={{ verticalAlign: "top"}}
          >
            <DashboardTimeSlot
              slots={slots.filter((x) => x.dayOfWeek === day)}
              day={day}
            />
          </List.Item>
        ))}
      </List>
    </Segment>
  );
});
