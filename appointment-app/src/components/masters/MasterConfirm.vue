<template>
  <section>
    <header>Подтвердите расписание сеанса</header>
    <div class="content">
      <ul>
        <li>
          <div class="scheduleItem">
            <div class="scheduleItemRow">
              <div class="iconWrap">
                <svg
                  height="24"
                  viewBox="0 0 24 24"
                  xmlns="http://www.w3.org/2000/svg"
                  class="iconSuccess"
                >
                  <path
                    d="M12 24C5.373 24 0 18.627 0 12S5.373 0 12 0s12 5.373 12 12-5.373 12-12 12zm5.078-16.7L9.72 14.499l-2.797-2.736-1.637 1.6L9.719 17.7l8.996-8.798z"
                  ></path>
                </svg>
              </div>
              <div class="scheduleTime">{{ time }}</div>
              <button class="closeButton">x</button>
            </div>
          </div>
        </li>
      </ul>
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
      </form>
      <button class="finish" :disabled="isValidForm" @click="saveAppointment">
        Завершить
      </button>
    </div>
  </section>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import AppointmentSlot from "@/models/AppointmentSlot";

export default defineComponent({
  name: "MasterConfirm",
  emits: ["save-appointment"],
  props: {
    appointmentSlot: {
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
    time(): string {
      const date = this.appointmentSlot.start.toLocaleDateString();
      const day = this.appointmentSlot.start.toLocaleDateString([], {
        weekday: "long",
      });
      const startTime = this.appointmentSlot.start.toLocaleTimeString([], {
        hour: "2-digit",
        minute: "2-digit",
      });
      const endTime = this.appointmentSlot.start.toLocaleTimeString([], {
        hour: "2-digit",
        minute: "2-digit",
      });
      return `${date}, ${day}, ${startTime}-${endTime}`;
    },
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
      const appointment = {
        ...this.appointmentSlot,
        phone: this.phone.val,
      };
      this.$emit("save-appointment", appointment);
    },
  },
});
</script>

<style scoped>
header {
  display: flex;
  margin-bottom: 24px;
  color: rgb(56, 64, 71);
  font-size: 24px;
  font-weight: 700;
  line-height: 1.33333;
  justify-content: center;
}

.content {
  margin-bottom: 24px;
}

ul {
  margin-bottom: 16px;
  list-style-type: none;
  font-size: 14px;
}

li {
  border: 1px solid rgb(218, 223, 225);
  margin-bottom: 12px;
  border-radius: 4px;
}

.scheduleItem {
  padding: 16px 0px 16px 16px;
}

.scheduleItemRow {
  position: relative;
  display: flex;
  -webkit-box-align: center;
  align-items: center;
  margin-bottom: 4px;
  color: rgb(56, 64, 71);
}

.scheduleItemRow:last-child {
  margin-bottom: 0px;
}

.iconWrap {
  margin-right: 8px;
  width: 24px;
  font-size: 0px;
  text-align: center;
}

.iconSuccess {
  width: 24px;
  height: 24px;
  fill: rgb(80, 191, 22);
}

.scheduleTime {
  font-weight: 500;
}

.closeButton {
  position: absolute;
  right: 0;
  height: 48px;
  width: 48px;
  background: none;
  border: 0;
  outline: 0;
  cursor: pointer;
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

button:disabled {
  background-color: rgba(19, 1, 1, 0.3);
  border-color: rgba(195, 195, 195, 0.3);
  color: rgba(16, 16, 16, 0.3);
}
</style>
