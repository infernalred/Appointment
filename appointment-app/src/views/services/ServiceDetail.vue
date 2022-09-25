<template>
  <h1>{{ getService.id }}</h1>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { mapActions, mapState } from "pinia";
import { useAppointmentStore } from "@/store";
import Service from "@/models/Service";

export default defineComponent({
  name: "ServiceDetail",
  computed: {
    ...mapState(useAppointmentStore, ["currentService"]),
    getService(): Service {
      return this.currentService;
    },
  },
  created() {
    this.initService(Number(this.$route.params.id));
  },
  methods: {
    ...mapActions(useAppointmentStore, ["loadService"]),
    async initService(id: number) {
      try {
        await this.loadService(id);
      } catch (e) {
        console.log(e);
      }
    },
  },
});
</script>

<style scoped></style>
