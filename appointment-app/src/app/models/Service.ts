import Master from "./Master";

export interface Service {
  id: string;
  title: string;
  description?: string;
  image?: string;
  durationMinutes: number;
  masters: Master[];
}

export class Service implements Service {
  constructor(init?: ServiceFormValues) {
    Object.assign(this, init)
  }
}

export class ServiceFormValues {
  id = "";
  title = "";
  description?: string = undefined;
  image?: string = undefined;
  durationMinutes = 30;

  constructor(service?: ServiceFormValues) {
    if (service) {
      this.id = service.id;
      this.title = service.title;
      this.description = service.description;
      this.image = service.image;
      this.durationMinutes = service.durationMinutes;
    }
  }
}
