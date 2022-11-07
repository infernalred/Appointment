import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../app/store/store";
import { ServiceFormValues } from "../../../app/models/Service";
import * as Yup from "yup";
import { v4 as uuid } from "uuid";
import { Formik, Form } from "formik";
import { Button, Header } from "semantic-ui-react";
import MyTextInput from "../../../app/common/form/MyTextInput";
import MyTextArea from "../../../app/common/form/MyTextArea";

interface Props {
  id: string;
}

export default observer(function ServiceForm({ id }: Props) {
  const { serviceStore, modalStore } = useStore();
  const { createService, updateService, loadService } = serviceStore;
  const [service, setService] = useState<ServiceFormValues>(
    new ServiceFormValues()
  );

  const validationSchema = Yup.object({
    title: Yup.string()
      .required("Название обязательно")
      .max(25, "Длина превышена"),
    durationMinutes: Yup.number()
      .required("Продолжительность обязательна")
      .moreThan(0)
      .when("title", (title, schema) => {
        return schema.test({
          test: (durationMinutes: number) => durationMinutes % 30 === 0,
          message: "Значение кратно 30",
        });
      }),
  });

  useEffect(() => {
    if (id)
      loadService(id).then((service) =>
        setService(new ServiceFormValues(service))
      );
  }, [id, loadService]);

  function handleSubmit(service: ServiceFormValues) {
    if (!service.id) {
      const newService = {
        ...service,
        id: uuid(),
      };
      createService(newService).then(() => modalStore.closeModal());
    } else {
      updateService(service).then(() => modalStore.closeModal());
    }
  }

  return (
    <Formik
      validationSchema={validationSchema}
      enableReinitialize
      initialValues={service}
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
            content={service.id ? "Редактирование услуги" : "Новая услуга"}
            color={"teal"}
            textAlign={"center"}
          ></Header>
          <MyTextInput
            maxLength={25}
            name={"title"}
            placeholder="Введите название"
            label={"Название"}
          />
          <MyTextArea
            rows={2}
            name={"description"}
            placeholder="Введите описание услуги"
            label={"Описание"}
          />
          <MyTextInput
            type={"number"}
            name={"durationMinutes"}
            placeholder="Продолжительность"
            label={"Продолжительность"}
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
