export interface TimeSlot {
    id: string;
    dayOfWeek: number;
    start: Date;
    end: Date;
  }

  export class TimeSlot implements TimeSlot {
    constructor(init?: TimeSlotFormValues) {
      Object.assign(this, init)
    }
  }
  
  export class TimeSlotFormValues {
    id = "";
    dayOfWeek?: number = undefined;
    start: Date | null = null;
    end: Date | null = null;
  
    constructor(slot?: TimeSlotFormValues) {
      if (slot) {
        this.id = slot.id;
        this.dayOfWeek = slot.dayOfWeek;
        this.start = slot.start;
        this.end = slot.end;
      }
    }
  }