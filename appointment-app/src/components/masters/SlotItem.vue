<template>
  <div class="header">
    <h1 class="header1">{{ day }}</h1>
  </div>
  <div v-if="hasSlots">
    <div v-for="slot in slotByDay" :key="slot.id">
      <slot-button
        :disabled="isSelectedSlot(slot.id)"
        :type="isActiveSlot(slot.id)"
        @click="setSelectedSlot(slot)"
        >{{ time(slot.start) }}</slot-button
      >
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
  emits: ["select-slot"],
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
    selectedSlot: {
      type: null as unknown as PropType<SlotModel | null>,
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
    isSelectedSlot(id: string): boolean {
      return !!this.selectedSlot && this.selectedSlot.id !== id;
    },
    isActiveSlot(id: string): string {
      if (this.selectedSlot?.id === id) {
        return "selected";
      }
      return "";
    },
    setSelectedSlot(slot: SlotModel) {
      this.$emit("select-slot", slot);
    },
  },
});
</script>

<style scoped></style>
