<template>
  <h1>Мастера</h1>
  <section>
    <base-card>
      <ul>
        <master-item
          v-for="master in getMasters"
          :key="master.id"
          :master="master"
        ></master-item>
      </ul>
    </base-card>
  </section>
</template>

<script lang="ts">
import MasterItem from "@/components/masters/MasterItem.vue";
import { mapActions, mapState } from "pinia";
import { useAppointmentStore } from "@/store";
import Master from "@/models/Master";
import { defineComponent } from "vue";

export default defineComponent({
  name: "MastersList",
  components: { MasterItem },
  computed: {
    ...mapState(useAppointmentStore, ["allMasters"]),
    getMasters(): Master[] {
      return this.allMasters;
    },
  },
  created() {
    this.initMasters();
  },
  methods: {
    ...mapActions(useAppointmentStore, ["loadMasters"]),
    async initMasters() {
      try {
        await this.loadMasters();
      } catch (e) {
        console.log(e);
      }
    },
  },
});
</script>

<style scoped>
ul {
  list-style: none;
  margin: 0;
  padding: 0;
}
</style>
