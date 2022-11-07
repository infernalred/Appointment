import {createContext, useContext} from "react";
import ServiceStore from "./serviceStore";
import MasterStore from "./masterStore";
import UserStore from "./userStore";
import AppointmentStore from "./appointmentStore";
import ModalStore from "./modalStore";
import TimeSlotStore from "./timeSlotStore";

interface Store {
    serviceStore: ServiceStore,
    masterStore: MasterStore,
    userStore: UserStore,
    modalStore: ModalStore,
    appointmentStore: AppointmentStore,
    timeSlotStore: TimeSlotStore,
}

export const store: Store = {
    serviceStore: new ServiceStore(),
    masterStore: new MasterStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore(),
    appointmentStore: new AppointmentStore(),
    timeSlotStore: new TimeSlotStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}
