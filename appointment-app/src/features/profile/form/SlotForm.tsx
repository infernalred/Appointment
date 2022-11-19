import { observer } from "mobx-react-lite";
import React, { useEffect, useState } from "react";
import { useStore } from "../../../app/store/store";
import * as Yup from "yup";
import { v4 as uuid } from "uuid";
import { TimeSlotFormValues } from "../../../app/models/TimeSlot";
import { Form, Formik } from "formik";
import { Header, Button } from "semantic-ui-react";
import MyDateInput from "../../../app/common/form/MyDateInput";

const language = Intl.DateTimeFormat().resolvedOptions().locale;

interface Props {
  id: string;
  day: number;
}

export default observer(function SlotForm({ id, day }: Props) {
  const { timeSlotStore, modalStore } = useStore();
  const { createSlot, updateSlot, loadSlot, slots } = timeSlotStore;
  const [slot, setSlot] = useState<TimeSlotFormValues>(
    new TimeSlotFormValues()
  );

  const validationSchema = Yup.object({
    start: Yup.date()
      .required()
      .nullable()
      .when("end", (end, schema) => {
        return schema.test({
          test: (start: Date) => start < end,
          message: "Начало должно быть раньше окончания",
        });
      }),
    end: Yup.date()
      .required()
      .nullable()
      .test("end", "Пересекается с существующим слотом", function (value) {
        return !(isUsedTime(this.parent.start, value) !== undefined);
      }),
  });

  function isUsedTime(
    start: Date | null | undefined,
    end: Date | null | undefined
  ) {
    if (!start || !end) {
      return undefined;
    }

    const today = new Date();

    const startDate = new Date(
      today.getUTCFullYear(),
      today.getUTCMonth(),
      today.getUTCDate(),
      start.getUTCHours(),
      start.getUTCMinutes()
    );
    const endDate = new Date(
      today.getUTCFullYear(),
      today.getUTCMonth(),
      today.getUTCDate(),
      end.getUTCHours(),
      end.getUTCMinutes()
    );

    const existSlots = slots.filter(
      (x) =>
        x.dayOfWeek === day &&
        x.id !== id &&
        new Date(
          today.getUTCFullYear(),
          today.getUTCMonth(),
          today.getUTCDate(),
          x.start.getUTCHours(),
          x.start.getUTCMinutes()
        ) < endDate &&
        new Date(
          today.getUTCFullYear(),
          today.getUTCMonth(),
          today.getUTCDate(),
          x.end.getUTCHours(),
          x.end.getUTCMinutes()
        ) > startDate
    );
    return existSlots[0];
  }

  useEffect(() => {
    if (id) {
      loadSlot(id).then((slot) => setSlot(new TimeSlotFormValues(slot)));
    }
  }, [id, loadSlot]);

  function handleSubmit(slot: TimeSlotFormValues) {
    if (!slot.id) {
      const newSlot = {
        ...slot,
        id: uuid(),
        dayOfWeek: day,
      };
      createSlot(newSlot).then(() => modalStore.closeModal());
    } else {
      updateSlot(slot).then(() => modalStore.closeModal());
    }
  }

  return (
    <Formik
      validationSchema={validationSchema}
      enableReinitialize
      initialValues={slot}
      onSubmit={(values) => handleSubmit(values)}
    >
      {({ handleSubmit, isSubmitting }) => (
        <Form
          className={"ui form"}
          onSubmit={handleSubmit}
          autoComplete={"off"}
        >
          <Header
            as={"h2"}
            content={slot.id ? "Редактирование слота" : "Новый слот"}
            color={"teal"}
            textAlign={"center"}
          ></Header>
          <label>Начало работы</label>
          <MyDateInput
            name={"start"}
            placeholderText="Начало работы"
            showTimeSelect
            showTimeSelectOnly
            timeIntervals={15}
            timeCaption="Время"
            dateFormat="p"
            locale={language}
          />
          <label>Окончание работы</label>
          <MyDateInput
            name={"end"}
            placeholderText="Окончание работы"
            showTimeSelect
            showTimeSelectOnly
            timeIntervals={15}
            timeCaption="Время"
            dateFormat="p"
            locale={language}
          />
          <Button
            disabled={isSubmitting}
            loading={isSubmitting}
            floated={"right"}
            positive
            type={"submit"}
            content={"Сохранить"}
          />
          <Button
            style={{ marginBottom: 10 }}
            type="button"
            onClick={() => modalStore.closeModal()}
            floated="right"
            content="Отмена"
          />
        </Form>
      )}
    </Formik>
  );
});
