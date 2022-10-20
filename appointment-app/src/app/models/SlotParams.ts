export class SlotParams {
  start;
  quantityDaysNumber;

  constructor(start = new Date(), quantityDaysNumber = 6) {
    this.start = start;
    this.quantityDaysNumber = quantityDaysNumber;
  }
}
