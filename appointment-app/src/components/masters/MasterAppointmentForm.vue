<template>
  <form @submit.prevent="saveAppointment">
    <div class="form-control" :class="{ invalid: !phone.isValid }">
      <label for="phone">Телефон</label>
      <input
        type="text"
        id="phone"
        v-model.trim="phone.val"
        @keypress="isNumber($event)"
      />
      <p v-if="!phone.isValid">Телефон должен быть заполнен.</p>
    </div>
    <button class="finish" :disabled="isValidForm">Завершить</button>
  </form>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import AppointmentSlot from "@/models/AppointmentSlot";

export default defineComponent({
  name: "MasterAppointmentForm",
  emits: ["save-appointment"],
  props: {
    appointment: {
      type: Object as PropType<AppointmentSlot>,
      required: true,
    },
  },
  data() {
    return {
      phone: {
        val: "",
        isValid: true,
      },
    };
  },
  computed: {
    isValidForm() {
      return this.phone.val.length !== 11;
    },
  },
  methods: {
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

      if (this.phone.val.length === 11 || !keysAllowed.includes(keyPressed)) {
        evt.preventDefault();
      }
    },
    saveAppointment() {
      const appointment = { ...this.appointment, phone: this.phone.val };
      this.$emit("save-appointment", appointment);
    },
  },
});
</script>

<style scoped>
.form-control {
  margin: 0.5rem 0;
}

label {
  font-weight: bold;
  display: block;
  margin-bottom: 0.5rem;
}

input[type="checkbox"] + label {
  font-weight: normal;
  display: inline;
  margin: 0 0 0 0.5rem;
}

input,
textarea {
  display: block;
  width: 100%;
  border: 1px solid #ccc;
  font: inherit;
}

.invalid label {
  color: red;
}

.invalid input,
.invalid textarea {
  border: 1px solid red;
}

.finish {
  position: relative;
  display: inline-flex;
  justify-content: center;
  border: 0;
  align-items: center;
  min-height: 32px;
  font-size: 14px;
  line-height: 1;
  text-decoration: none;
  font-weight: 500;
  border-radius: 2px;
  text-align: center;
  text-overflow: ellipsis;
  overflow: hidden;
  max-width: 100%;
  cursor: pointer;
  outline: 0;
  transition: background-color 50ms;
  color: #fff;
  background-color: #3bb3bd;
  padding: 8px 16px;
}

button:disabled {
  background-color: rgba(19, 1, 1, 0.3);
  border-color: rgba(195, 195, 195, 0.3);
  color: rgba(16, 16, 16, 0.3);
}
</style>
