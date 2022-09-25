<template>
  <h2>{{ master.displayName }}</h2>
  <base-button link :to="masterLink">Подробнее</base-button>
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

<style scoped></style>
