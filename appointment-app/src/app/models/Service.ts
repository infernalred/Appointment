import Master from "./Master";

export default interface Service {
  id: number;
  title: string;
  description?: string;
  image?: string;
  durationMinutes: number;
  masters: Master[];
}
