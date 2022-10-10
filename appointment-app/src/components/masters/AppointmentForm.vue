<template>
  <n-form
    ref="formRef"
    inline
    :label-width="80"
    :model="formValue"
    :rules="rules"
  >
    <n-form-item label="Phone" path="phone">
      <n-input
        v-model:value.trim="formValue.phone"
        @keypress="isNumber($event)"
        placeholder="Phone Number"
      />
    </n-form-item>
    <n-form-item>
      <n-button
        round
        type="primary"
        :disabled="isValidForm"
        @click="saveAppointment"
        >Забронировать</n-button
      >
    </n-form-item>
  </n-form>
</template>

<script lang="ts">
import { defineComponent, PropType, ref } from "vue";
import { FormInst } from "naive-ui";
import AppointmentSlot from "@/models/AppointmentSlot";

export default defineComponent({
  name: "AppointmentForm",
  emits: ["save-appointment"],
  props: {
    appointment: {
      type: Object as PropType<AppointmentSlot>,
      required: true,
    },
  },
  setup(props, { emit }) {
    const formRef = ref<FormInst | null>(null);
    const formValue = ref({ phone: "" });
    return {
      formRef,
      formValue,
      rules: {
        phone: {
          required: true,
          message: "Please input your number",
          trigger: ["blur", "input"],
        },
      },
      isNumber(evt: KeyboardEvent): void {
        const keysAllowed: string[] = [
          "0",
          "1",
          "2",
          "3",
          "4",
          "5",
          "6",
          "7",
          "8",
          "9",
        ];
        const keyPressed: string = evt.key;

        if (
          formValue.value.phone.length === 11 ||
          !keysAllowed.includes(keyPressed)
        ) {
          evt.preventDefault();
        }
      },
      saveAppointment() {
        const appointment = {
          ...props.appointment,
          phone: formValue.value.phone,
        };
        emit("save-appointment", appointment);
      },
    };
  },
  computed: {
    isValidForm() {
      return this.formValue.phone.length !== 11;
    },
  },
});
</script>

<style scoped></style>
