import {createContext, useContext} from "react";
import ServiceStore from "./serviceStore";
import MasterStore from "./masterStore";

interface Store {
    serviceStore: ServiceStore,
    masterStore: MasterStore
}

export const store: Store = {
    serviceStore: new ServiceStore(),
    masterStore: new MasterStore(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}
