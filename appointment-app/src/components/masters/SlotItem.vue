<template>
  <div class="header">
    <h1 class="header1">{{ day }}</h1>
  </div>
  <div v-if="hasSlots">
    <div v-for="slot in slotByDay" :key="slot.id">
      <slot-button>{{ time(slot.start) }}</slot-button>
    </div>
  </div>
  <slot-button v-else>Слотов нет!</slot-button>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import SlotModel from "@/models/SlotModel";
import SlotButton from "@/components/masters/SlotButton.vue";

export default defineComponent({
  name: "SlotItem",
  components: { SlotButton },
  props: {
    date: {
      type: Object as PropType<Date>,
      required: true,
    },
    masterSlots: {
      type: Object as PropType<SlotModel[]>,
      required: true,
    },
  },
  computed: {
    slotByDay(): SlotModel[] {
      return this.masterSlots.filter(
        (x) => x.start.getDay() === this.date.getDay()
      );
    },
    day(): string {
      return this.date.toLocaleDateString([], { weekday: "short" });
    },
    hasSlots() {
      return this.slotByDay.length !== 0;
    },
  },
  methods: {
    time(date: Date) {
      return date.toLocaleTimeString([], {
        hour: "2-digit",
        minute: "2-digit",
      });
    },
  },
});
</script>

<style scoped></style>
