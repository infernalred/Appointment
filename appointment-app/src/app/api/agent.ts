import axios, {AxiosError, AxiosResponse} from "axios";
import OperationResult from "../models/OperationResult";
import {AppointmentSlot} from "../models/AppointmentSlot";
import {LoginModel} from "../models/LoginModel";
import {Account} from "../models/Account";
import SlotModel from "../models/SlotModel";
import {SlotParams} from "../models/SlotParams";
import Master from "../models/Master";
import {Service, ServiceFormValues} from "../models/Service";
import {store} from "../store/store";
import {toast} from "react-toastify";
import {RegisterMaster} from "../models/RegisterMaster";
import { TimeSlot, TimeSlotFormValues } from "../models/TimeSlot";
import { ServerError } from "../models/ServerError";
import { useNavigate } from "react-router-dom";

axios.defaults.baseURL = process.env.REACT_APP_API_URL;

axios.interceptors.request.use(config => {
    const token = store.userStore.token;
    if (token && config.headers) config.headers.Authorization = `Bearer ${token}`
    return config;
})

axios.interceptors.response.use(async response => {
    return response;
}, (error: AxiosError) => {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    const { data, status, headers } = error.response!;
    switch (status) {
        case 400:
            if ((data as any).errors) {
                const modalStateErrors = [];
                for (const key in (data as any).errors) {
                    if ((data as any).errors[key]) {
                        modalStateErrors.push((data as any).errors[key])
                    }
                }
                throw modalStateErrors.flat();
            } else {
                toast.error(data as string);
            }
            break;
        case 401:
            if (status === 401 && headers["www-authenticate"]?.startsWith('Bearer error="invalid_token"')) {
                store.userStore.logout();
                toast.error("Сессия истекла - пожалуйста, авторизуйтесь снова");
            }
            break;
        // case 404:
        //     history.push('/not-found');
        //     break;
        case 409:
            toast.error((data as OperationResult<never>).error || "Произошла ошибка");
            break;
        // case 500:
        //     store.commonStore.setServerError(data as ServerError);
        //     //history.push('/server-error')
        //     navigate("/server-error");
        //     break;
    }
    return Promise.reject(error);
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
    myAppointments: () => requests.get<OperationResult<AppointmentSlot[]>>("/appointments")
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
