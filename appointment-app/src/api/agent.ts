import axios, { AxiosError, AxiosResponse } from "axios";
import { OperationResult } from "@/models/OperationResult";
import Service from "@/models/Service";
import Master from "@/models/Master";
import SlotModel from "@/models/SlotModel";
import SlotParams from "@/models/SlotParams";
import AppointmentSlot from "@/models/AppointmentSlot";
import { useToast } from "vue-toastification";
import LoginModel from "@/models/LoginModel";
import Account from "@/models/Account";

const toast = useToast();

axios.defaults.baseURL = process.env.VUE_APP_API_ADDRESS;

axios.interceptors.response.use(
  async (response) => {
    return response;
  },
  (error: AxiosError) => {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    const { data, status } = error.response!;
    switch (status) {
      case 409:
        toast.error(
          (data as OperationResult<never>).error || "Произошла ошибка"
        );
    }
    return Promise.reject(error);
  }
);

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: unknown) =>
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

const Appointments = {
  create: (appointment: AppointmentSlot) =>
    requests.post<OperationResult<unknown>>("/appointments", appointment),
};

const Users = {
  login: (login: LoginModel) => requests.post<Account>("/users/login", login),
};

const agent = {
  Services,
  Masters,
  Appointments,
  Users,
};

export default agent;
