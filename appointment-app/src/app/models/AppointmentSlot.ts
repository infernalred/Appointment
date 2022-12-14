export class AppointmentSlot {
  id = "";
  userName = "";
  phone = "";
  masterId = "";
  start: Date = new Date();
  end: Date = new Date();

  constructor(slot?: AppointmentSlot) {
    if (slot) {
      this.id = slot.id;
      this.userName = slot.userName;
      this.phone = slot.phone;
      this.masterId = slot.masterId;
      this.start = slot.start;
      this.end = slot.end;
    }
  }
}
