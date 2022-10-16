import Master from "../models/Master";
import {makeAutoObservable, runInAction} from "mobx";
import agent from "../api/agent";

export default class MasterStore {
    master: Master | undefined = undefined;
    masterRegistry = new Map<string, Master>();
    loading = false;

    constructor() {
        makeAutoObservable(this)
    }

    setLoading = (state: boolean) => {
        this.loading = state;
    }

    loadMasters = async () => {
        this.setLoading(true);
        try {
            const request = await agent.Masters.list();
            runInAction(() => {
                request.result.forEach(master => {
                    this.setMaster(master);
                })
            })
        } catch (e) {
            console.log(e)
        }
        finally {
            this.setLoading(false);
        }
    }

    loadMaster = async (id: string) => {
        const master = this.getMaster(id);
        if (master) {
            this.master = master;
        } else {
            this.setLoading(true);
            try {
                const response = await agent.Masters.details(id);
                this.setMaster(response.result);
                runInAction(() => {
                    this.master = response.result;
                })
            } catch (e) {
                console.log(e);
            }
            finally {
                this.setLoading(false);
            }
        }
    }

    get masters() {
        return Array.from(this.masterRegistry.values());
    }

    private getMaster = (id: string) => {
        return this.masterRegistry.get(id);
    }
    private setMaster = (master: Master) => {
        this.masterRegistry.set(master.id, master);
    }
}
