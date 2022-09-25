<template>
  <section>
    <base-card>
      <h1>{{ getService.title }}</h1>
      <p>{{ getService.description }}</p>
      <section>
        <h1>Мастера оказывающие услугу:</h1>
        <master-info
          v-for="master in getService.masters"
          :key="master.id"
          :master="master"
        ></master-info>
      </section>
    </base-card>
  </section>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { mapActions, mapState } from "pinia";
import { useAppointmentStore } from "@/store";
import Service from "@/models/Service";
import MasterInfo from "@/components/masters/MasterInfo.vue";

export default defineComponent({
  name: "ServiceDetail",
  components: { MasterInfo },
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
