import axios, { AxiosResponse } from "axios";
import { OperationResult } from "@/models/OperationResult";
import Service from "@/models/Service";

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
  list: () => requests.get<OperationResult<Service[]>>("/service"),
  details: (id: number) =>
    requests.get<OperationResult<Service>>(`/service/${id}`),
};

const agent = {
  Services,
};

export default agent;
