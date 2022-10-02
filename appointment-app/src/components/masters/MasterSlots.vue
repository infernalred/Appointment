<template>
  <h1>Часы приема</h1>
  <section class="timeSlots">
    <div class="management">
      <base-button :disabled="isToday" @click="changeWeek(-7)"
        >&#60;</base-button
      >
      <base-button @click="changeWeek(7)">&#62;</base-button>
      <h3>{{ weekLabelDate }}</h3>
    </div>
    <div class="days">
      <ul v-for="day in days" :key="day">
        <slot-item :master-slots="masterSlots" :date="day"></slot-item>
      </ul>
    </div>
  </section>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import SlotModel from "@/models/SlotModel";
import SlotItem from "@/components/masters/SlotItem.vue";

export default defineComponent({
  name: "MasterSlots",
  components: { SlotItem },
  props: {
    masterSlots: {
      type: Object as PropType<SlotModel[]>,
      required: true,
    },
  },
  data() {
    return {
      nDate: 7,
      date: new Date(),
      days: [] as Date[],
    };
  },
  created() {
    this.initDays(this.nDate);
  },
  computed: {
    weekLabelDate() {
      const date = new Date(this.date);
      date.setDate(date.getDate() + this.nDate);
      return `${this.date.toLocaleString("default", {
        month: "long",
      })} ${this.date.getDate()}-${date.getDate()}`;
    },
    isToday(): boolean {
      return new Date() >= this.date;
    },
  },
  methods: {
    initDays(nDays: number) {
      let date = new Date(this.date);
      for (let i = 0; i < nDays; i++) {
        this.days.push(new Date(date));
        date.setDate(date.getDate() + 1);
      }
    },
    slotByDay(day: Date): SlotModel[] {
      return this.masterSlots.filter((x) => x.start.getDay() === day.getDay());
    },
    changeWeek(cntDay: number): void {
      this.date = new Date(this.date.setDate(this.date.getDate() + cntDay));
    },
  },
});
</script>

<style scoped>
.management {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: left;
}
.days {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 0.5rem;
}
ul {
  margin: 0;
  padding: 0;
  border-top: 4px solid rgb(59, 179, 189);
  background: rgb(243, 250, 251);
}
</style>
