import {createContext, useContext} from "react";
import ServiceStore from "./serviceStore";

interface Store {
    serviceStore: ServiceStore
}

export const store: Store = {
    serviceStore: new ServiceStore(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}
