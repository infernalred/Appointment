<template>
  <section>
    <base-card>
      <h1>{{ getMaster.displayName }}</h1>
    </base-card>
  </section>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { mapActions, mapState } from "pinia";
import { useAppointmentStore } from "@/store";
import Master from "@/models/Master";

export default defineComponent({
  name: "MasterDetails",
  computed: {
    ...mapState(useAppointmentStore, ["currentMaster"]),
    getMaster(): Master {
      return this.currentMaster;
    },
  },
  created() {
    this.initMaster(this.$route.params.id);
  },
  methods: {
    ...mapActions(useAppointmentStore, ["loadMaster"]),
    async initMaster(id: string) {
      try {
        await this.loadMaster(id);
      } catch (e) {
        console.log(e);
      }
    },
  },
});
</script>

<style scoped></style>
