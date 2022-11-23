export class AppointmentsOnDateParams {
    onDate;
  
    constructor(onDate = new Date()) {
      this.onDate = onDate;
    }
  }
  