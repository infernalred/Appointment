import axios, { AxiosResponse } from "axios";
import { OperationResult } from "@/models/OperationResult";
import Service from "@/models/Service";
import Master from "@/models/Master";
import SlotModel from "@/models/SlotModel";
import SlotParams from "@/models/SlotParams";

axios.defaults.baseURL = process.env.VUE_APP_API_ADDRESS;

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: Record<string, unknown>) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: Record<string, unknown>) =>
    axios.put<T>(url, body).then(responseBody),
  del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Services = {
  list: () => requests.get<OperationResult<Service[]>>("/services"),
  details: (id: number) =>
    requests.get<OperationResult<Service>>(`/services/${id}`),
};

const Masters = {
  list: () => requests.get<OperationResult<Master[]>>("/masters"),
  details: (id: string) =>
    requests.get<OperationResult<Master>>(`/masters/${id}`),
  freeSlots: (id: string, slotParams?: SlotParams) =>
    axios
      .get<OperationResult<SlotModel[]>>(`/masters/slots/${id}`, {
        params: slotParams,
      })
      .then(responseBody),
};

const agent = {
  Services,
  Masters,
};

export default agent;
