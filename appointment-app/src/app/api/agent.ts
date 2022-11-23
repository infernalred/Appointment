import axios, {AxiosResponse} from "axios";
import OperationResult from "../models/OperationResult";
import {AppointmentSlot} from "../models/AppointmentSlot";
import {LoginModel} from "../models/LoginModel";
import {Account} from "../models/Account";
import SlotModel from "../models/SlotModel";
import {SlotParams} from "../models/SlotParams";
import Master from "../models/Master";
import {Service, ServiceFormValues} from "../models/Service";
import {store} from "../store/store";
import {RegisterMaster} from "../models/RegisterMaster";
import { TimeSlot, TimeSlotFormValues } from "../models/TimeSlot";
import { AppointmentsOnDateParams } from "../models/AppointmentsOnDateParams";

axios.defaults.baseURL = process.env.REACT_APP_API_URL;

axios.interceptors.request.use(config => {
    const token = store.userStore.token;
    if (token && config.headers) config.headers.Authorization = `Bearer ${token}`
    return config;
})

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: unknown) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: unknown) => axios.put<T>(url, body).then(responseBody),
    del: <T>(url: string) => axios.delete<T>(url).then(responseBody)
}

const Services = {
    list: () => requests.get<OperationResult<Service[]>>("/services"),
    details: (id: string) => requests.get<OperationResult<Service>>(`/services/${id}`),
    create: (service: ServiceFormValues) => requests.post<void>("/services", service),
    update: (service: ServiceFormValues) => requests.put<void>(`/services/${service.id}`, service)
};

const Masters = {
    list: () => requests.get<OperationResult<Master[]>>("/masters"),
    details: (id: string) => requests.get<OperationResult<Master>>(`/masters/${id}`),
    freeSlots: (id: string, slotParams?: SlotParams) => axios.get<OperationResult<SlotModel[]>>(
        `/masters/slots/${id}`, {
                params: slotParams,
            }).then(responseBody),
    create: (registerMaster: RegisterMaster) => requests.post<void>("/users/master", registerMaster),
};

const Appointments = {
    create: (appointment: AppointmentSlot) =>
        requests.post<OperationResult<unknown>>("/appointments", appointment),
    appointmentsOnDate: (params: AppointmentsOnDateParams) => axios.get<OperationResult<AppointmentSlot[]>>("/appointments/", { params})
        .then(responseBody)
};

const TimeSlots = {
    list: () => requests.get<OperationResult<TimeSlot[]>>("/timeslots"),
    details: (id: string) => requests.get<OperationResult<TimeSlot>>(`/timeslots/${id}`),
    create: (slot: TimeSlotFormValues) => requests.post<OperationResult<unknown>>("/timeslots", slot),
    update: (slot: TimeSlotFormValues) => requests.put<OperationResult<unknown>>(`/timeslots/${slot.id}`, slot),
    delete: (id: string) => requests.del<void>(`/timeslots/${id}`)
}

const Users = {
    login: (login: LoginModel) => requests.post<Account>("/users/login", login),
};

const agent = {
    Services,
    Masters,
    Appointments,
    Users,
    TimeSlots
};

export default agent;
