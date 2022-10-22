import {createContext, useContext} from "react";
import ServiceStore from "./serviceStore";
import MasterStore from "./masterStore";
import UserStore from "./userStore";

interface Store {
    serviceStore: ServiceStore,
    masterStore: MasterStore,
    userStore: UserStore,
}

export const store: Store = {
    serviceStore: new ServiceStore(),
    masterStore: new MasterStore(),
    userStore: new UserStore(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}
