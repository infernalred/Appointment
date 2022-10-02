<template>
  <section>
    <base-card>
      <h1>{{ getMaster.displayName }}</h1>
    </base-card>
  </section>
  <section>
    <base-card>
      <master-slots :master-slots="getMasterSlots"></master-slots>
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

export default defineComponent({
  name: "MasterDetails",
  components: { MasterSlots },
  data() {
    return {
      slotParams: {} as SlotModel,
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
    const id = this.$route.params.id as string;
    this.initMaster(id);
    this.initSlots(id, this.slotParams);
  },
  methods: {
    ...mapActions(useAppointmentStore, ["loadMaster"]),
    ...mapActions(useAppointmentStore, ["loadSlots"]),
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
  },
});
</script>

<style scoped></style>
