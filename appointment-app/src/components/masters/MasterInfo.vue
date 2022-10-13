<template>
  <h2>{{ master.displayName }}</h2>
  <router-link :to="masterLink"
    ><n-button strong secondary round type="primary" size="large"
      >Подробнее</n-button
    ></router-link
  >
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import Master from "@/models/Master";

export default defineComponent({
  name: "MasterInfo",
  props: {
    master: {
      type: Object as PropType<Master>,
      required: true,
    },
  },
  computed: {
    masterLink(): string {
      return "/masters/" + this.master.id;
    },
  },
});
</script>

<style scoped>
a {
  text-decoration: none;
}
</style>
