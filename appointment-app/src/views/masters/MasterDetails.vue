<template>
  <section>
    <base-card>
      <h1>{{ getMaster.displayName }}</h1>
    </base-card>
  </section>
  <section>
    <base-card>
      <master-slots
        v-if="!confirmed"
        @change-params="setSlotParams"
        :master-slots="getMasterSlots"
        :slot-params="slotParams"
        @confirm-selected="confirm"
      ></master-slots>
      <master-confirm
        v-else
        :appointment-slot="appointmentSlot"
        @save-appointment="saveData"
      ></master-confirm>
    </base-card>
  </section>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { mapActions, mapState } from "pinia";
import { useAppointmentStore } from "@/store";
import Master from "@/models/Master";
import MasterSlots from "@/components/masters/MasterSlots.vue";
import SlotModel from "@/models/SlotModel";
import SlotParams from "@/models/SlotParams";
import AppointmentSlot from "@/models/AppointmentSlot";
import MasterConfirm from "@/components/masters/MasterConfirm.vue";

export default defineComponent({
  name: "MasterDetails",
  props: ["id"],
  components: { MasterConfirm, MasterSlots },
  data() {
    return {
      slotParams: {
        start: new Date(),
        quantityDays: 6,
      } as SlotParams,
      confirmed: false,
      appointmentSlot: null as null | AppointmentSlot,
    };
  },
  computed: {
    ...mapState(useAppointmentStore, ["currentMaster"]),
    ...mapState(useAppointmentStore, ["masterSlots"]),
    getMaster(): Master {
      return this.currentMaster;
    },
    getMasterSlots(): SlotModel[] {
      return this.masterSlots;
    },
  },
  created() {
    this.initMaster(this.id);
    this.initSlots(this.id, this.slotParams);
  },
  methods: {
    ...mapActions(useAppointmentStore, ["loadMaster"]),
    ...mapActions(useAppointmentStore, ["loadSlots"]),
    ...mapActions(useAppointmentStore, ["saveAppointment"]),
    async initMaster(id: string) {
      try {
        await this.loadMaster(id);
      } catch (e) {
        console.log(e);
      }
    },
    async initSlots(id: string, slotParams: SlotParams) {
      try {
        await this.loadSlots(id, slotParams);
      } catch (e) {
        console.log(e);
      }
    },
    setSlotParams(updatedSlotParams: SlotParams) {
      this.slotParams = updatedSlotParams;
      this.initSlots(this.id, this.slotParams);
    },
    confirm(slot: SlotModel) {
      this.appointmentSlot = {
        id: slot.id,
        masterId: this.id,
        start: slot.start,
        end: slot.end,
      } as AppointmentSlot;
      this.confirmed = true;
      //this.$router.push(this.$route.path + "/confirm");
    },
    async saveData(appointment: AppointmentSlot) {
      try {
        await this.saveAppointment(appointment);
        this.$router.replace("/masters");
      } catch (e: any) {
        console.log(e.response.data);
      }
    },
  },
});
</script>

<style scoped></style>
