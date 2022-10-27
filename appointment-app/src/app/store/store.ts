import {createContext, useContext} from "react";
import ServiceStore from "./serviceStore";
import MasterStore from "./masterStore";
import UserStore from "./userStore";
import ModalStore from "./modalStore";
import AppointmentStore from "./appointmentStore";

interface Store {
    serviceStore: ServiceStore,
    masterStore: MasterStore,
    userStore: UserStore,
    modalStore: ModalStore,
    appointmentStore: AppointmentStore,
}

export const store: Store = {
    serviceStore: new ServiceStore(),
    masterStore: new MasterStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore(),
    appointmentStore: new AppointmentStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}
