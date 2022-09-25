<template>
  <li>
    <header>{{ master.displayName }}</header>
    <base-button link :to="masterLink">Подробнее</base-button>
  </li>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import Master from "@/models/Master";

export default defineComponent({
  name: "MasterItem",
  props: {
    master: {
      type: Object as PropType<Master>,
      required: true,
    },
  },
  computed: {
    masterLink() {
      return this.$route.path + "/" + this.master.id;
    },
  },
});
</script>

<style scoped>
li {
  margin: 1rem 0;
  border: 1px solid #424242;
  border-radius: 12px;
  padding: 1rem;
}
</style>
