<template>
  <div>
    <section>
      <base-card>
        <header>
          <h2>{{ service.title }}</h2>
        </header>
      </base-card>
    </section>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import Service from "@/models/Service";

export default defineComponent({
  name: "ServiceItem",
  props: {
    service: {
      type: Object as PropType<Service>,
      required: true,
    },
  },
});
</script>

<style scoped></style>
