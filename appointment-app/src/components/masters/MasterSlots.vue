<template>
  <h1>Часы приема</h1>
  <section class="timeSlots">
    <div class="management">
      <base-button :disabled="isToday" @click="setSlotParams(-7)"
        >&#60;</base-button
      >
      <base-button @click="setSlotParams(7)">&#62;</base-button>
      <h3>{{ weekLabelDate }}</h3>
      <div v-if="selectedSlot">
        <span
          ><span>
            {{ selectedDate }}
            <button class="cancel" @click="clearSelected">x</button>
          </span>
          <button class="next" @click="confirm">Далее</button>
        </span>
      </div>
    </div>
    <div class="days">
      <ul v-for="day in days" :key="day">
        <slot-item
          @select-slot="setSelectedSlot"
          :master-slots="masterSlots"
          :selected-slot="selectedSlot"
          :date="day"
        ></slot-item>
      </ul>
    </div>
  </section>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import SlotModel from "@/models/SlotModel";
import SlotItem from "@/components/masters/SlotItem.vue";
import SlotParams from "@/models/SlotParams";

export default defineComponent({
  emits: ["change-params", "confirm-selected"],
  name: "MasterSlots",
  components: { SlotItem },
  props: {
    masterSlots: {
      type: Object as PropType<SlotModel[]>,
      required: true,
    },
    slotParams: {
      type: Object as PropType<SlotParams>,
      required: true,
    },
  },
  data() {
    return {
      days: [] as Date[],
      selectedSlot: null as null | SlotModel,
    };
  },
  created() {
    this.initDays(this.slotParams.quantityDays);
  },
  computed: {
    weekLabelDate() {
      const date = new Date(this.slotParams.start);
      date.setDate(date.getDate() + this.slotParams.quantityDays);
      return `${this.slotParams.start.toLocaleString([], {
        month: "long",
      })} ${this.slotParams.start.getDate()}-${date.getDate()}`;
    },
    isToday(): boolean {
      return new Date() >= this.slotParams.start;
    },
    selectedDate(): string {
      return `${this.selectedSlot?.start.toLocaleDateString()} ${this.selectedSlot?.start.toLocaleTimeString(
        [],
        { timeStyle: "short" }
      )}-${this.selectedSlot?.end.toLocaleTimeString([], {
        timeStyle: "short",
      })}`;
    },
    link() {
      return this.$route.path + "/confirm";
    },
  },
  methods: {
    initDays(nDays: number) {
      let date = new Date(this.slotParams.start);
      for (let i = 0; i <= nDays; i++) {
        this.days.push(new Date(date));
        date.setDate(date.getDate() + 1);
      }
    },
    slotByDay(day: Date): SlotModel[] {
      return this.masterSlots.filter((x) => x.start.getDay() === day.getDay());
    },
    setSlotParams(cntDay: number) {
      this.clearSelected();
      const updatedSlotParams = {
        ...this.slotParams,
        start: new Date(
          this.slotParams.start.setDate(
            this.slotParams.start.getDate() + cntDay
          )
        ),
      } as SlotParams;
      this.$emit("change-params", updatedSlotParams);
    },
    setSelectedSlot(slot: SlotModel) {
      if (this.selectedSlot?.id === slot.id) {
        this.clearSelected();
      } else {
        this.selectedSlot = slot;
      }
    },
    clearSelected() {
      this.selectedSlot = null;
    },
    confirm() {
      this.$emit("confirm-selected", this.selectedSlot);
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

span {
  height: 32px;
  padding-left: 2px;
  font: inherit;
  font-weight: 500;
  position: relative;
  width: 100%;
  display: flex;
  -webkit-box-pack: justify;
  justify-content: space-between;
  -webkit-box-align: center;
  align-items: center;
  border-radius: 2px;
  background-color: rgb(59, 179, 189);
  color: rgb(255, 255, 255);
  margin-left: 0.5rem;
}

.cancel {
  width: 32px;
  height: 38px;
  border: none;
  background: none;
  cursor: pointer;
  color: rgb(255, 255, 255);
}

.next {
  width: auto;
  height: 38px;
  border: none;
  background: none;
  cursor: pointer;
  color: rgb(255, 255, 255);
}
</style>
