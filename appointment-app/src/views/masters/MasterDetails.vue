<template>
  <section>
    <base-card>
      <h1>{{ getMaster.displayName }}</h1>
    </base-card>
  </section>
  <section>
    <base-card>
      <master-slots
        @change-params="setSlotParams"
        :master-slots="getMasterSlots"
        :slot-params="slotParams"
        @confirm-selected="confirm"
      ></master-slots>
    </base-card>
  </section>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useAppointmentStore } from "@/store";
import Master from "@/models/Master";
import MasterSlots from "@/components/masters/MasterSlots.vue";
import SlotModel from "@/models/SlotModel";
import SlotParams from "@/models/SlotParams";
import AppointmentSlot from "@/models/AppointmentSlot";

export default defineComponent({
  name: "MasterDetails",
  props: ["id"],
  components: { MasterSlots },
  data() {
    return {
      slotParams: {
        start: new Date(),
        quantityDays: 6,
      } as SlotParams,
      store: useAppointmentStore(),
    };
  },
  computed: {
    getMaster(): Master {
      return this.store.currentMaster;
    },
    getMasterSlots(): SlotModel[] {
      return this.store.masterSlots;
    },
  },
  created() {
    this.initMaster(this.id);
    this.initSlots(this.id, this.slotParams);
  },
  methods: {
    async initMaster(id: string) {
      try {
        await this.store.loadMaster(id);
      } catch (e) {
        console.log(e);
      }
    },
    async initSlots(id: string, slotParams: SlotParams) {
      try {
        await this.store.loadSlots(id, slotParams);
      } catch (e) {
        console.log(e);
      }
    },
    setSlotParams(updatedSlotParams: SlotParams) {
      this.slotParams = updatedSlotParams;
      this.initSlots(this.id, this.slotParams);
    },
    confirm(slot: SlotModel) {
      const appointmentSlot = {
        id: slot.id,
        masterId: this.id,
        start: slot.start,
        end: slot.end,
      } as AppointmentSlot;
      this.store.confirmAppointment(appointmentSlot);
      this.$router.push(this.$route.path + "/confirm");
    },
  },
});
</script>

<style scoped></style>
