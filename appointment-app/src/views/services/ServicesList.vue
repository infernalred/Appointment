<template>
  <h1>Салон красоты</h1>
  <br />
  <div>
    <img alt="logo" src="../../assets/logo.png" />
  </div>
  <div class="cards">
    <service-item
      class="card"
      v-for="service in getServices"
      :key="service.id"
      :service="service"
    ></service-item>
  </div>
</template>

<script lang="ts">
import ServiceItem from "@/components/services/ServiceItem.vue";
import { mapActions, mapState } from "pinia";
import { useAppointmentStore } from "@/store";
import { defineComponent } from "vue";
import Service from "@/models/Service";

export default defineComponent({
  name: "ServicesList",
  components: { ServiceItem },
  computed: {
    ...mapState(useAppointmentStore, ["allServices"]),
    getServices(): Service[] {
      return this.allServices;
    },
  },
  created() {
    this.initServices();
  },
  methods: {
    ...mapActions(useAppointmentStore, ["loadServices"]),
    async initServices() {
      try {
        await this.loadServices();
      } catch (e) {
        console.log(e);
      }
    },
  },
});
</script>

<style scoped>
.cards {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
}
img {
  border-radius: 12px;
}
</style>
