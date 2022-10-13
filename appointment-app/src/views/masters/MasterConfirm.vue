<template>
  <section>
    <base-card>
      <n-result
        v-if="!confirmed"
        status="info"
        title="Подтвердите расписание сеанса"
      >
        <template #footer>
          <div class="content">
            <div class="scheduleItem">
              <div class="scheduleItemRow">
                <div class="scheduleTime">{{ time }}</div>
                <button class="closeButton" @click="clear">x</button>
              </div>
            </div>
            <appointment-form
              :appointment="appointment"
              @save-appointment="saveAppointment"
            ></appointment-form>
          </div>
        </template>
      </n-result>
      <n-result
        v-else
        status="success"
        title="Бронирование создано успешно"
      ></n-result>
    </base-card>
  </section>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useAppointmentStore } from "@/store";
import AppointmentSlot from "@/models/AppointmentSlot";
import { useToast } from "vue-toastification";
import AppointmentForm from "@/components/masters/AppointmentForm.vue";

const toast = useToast();

export default defineComponent({
  name: "MasterConfirm",
  components: { AppointmentForm },
  props: ["id"],
  data() {
    return {
      store: useAppointmentStore(),
      appointment: {} as AppointmentSlot,
      confirmed: false,
    };
  },
  created() {
    const appointment = this.store.selectedAppointment;
    if (appointment === null || appointment.masterId !== this.id) {
      this.$router.replace(`/masters/${this.id}`);
    } else {
      this.appointment = appointment;
    }
  },
  computed: {
    time(): string {
      const date = this.appointment.start.toLocaleDateString();
      const day = this.appointment.start.toLocaleDateString([], {
        weekday: "long",
      });
      const startTime = this.appointment.start.toLocaleTimeString([], {
        hour: "2-digit",
        minute: "2-digit",
      });
      const endTime = this.appointment.start.toLocaleTimeString([], {
        hour: "2-digit",
        minute: "2-digit",
      });
      return `${date}, ${day}, ${startTime}-${endTime}`;
    },
  },
  methods: {
    async saveAppointment(appointment: AppointmentSlot) {
      try {
        console.log(appointment);
        // await this.store.saveAppointment(appointment);
        toast.success("Бронирование выполнено успешно");
        this.store.clearAppointment();
        this.confirmed = true;
      } catch (e) {
        console.log(e);
      }
    },
    clear() {
      this.store.clearAppointment();
      this.$router.replace(`/masters/${this.id}`);
    },
  },
});
</script>

<style scoped>
.content {
  margin-bottom: 24px;
}

.scheduleItem {
  border: 1px solid rgb(218, 223, 225);
  margin-bottom: 12px;
  border-radius: 4px;
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
</style>
