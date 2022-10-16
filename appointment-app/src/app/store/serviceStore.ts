import Service from "../models/Service";
import {makeAutoObservable, runInAction} from "mobx";
import agent from "../api/agent";

export default class ServiceStore {
    service: Service | undefined = undefined;
    serviceRegistry = new Map<number, Service>();
    loading = false;

    constructor() {
        makeAutoObservable(this)
    }

    setLoading = (state: boolean) => {
        this.loading = state;
    }

    loadServices = async () => {
        this.setLoading(true);
        try {
            const request = await agent.Services.list();
            runInAction(() => {
                request.result.forEach(service => {
                    this.setService(service);
                })
            })
        } catch (e) {
            console.log(e)
        }
        finally {
            this.setLoading(false);
        }
    }

    loadService = async (id: number) => {
        const service = this.getService(id);
        if (service) {
            this.service = service;
        } else {
            this.setLoading(true);
            try {
                const response = await agent.Services.details(id);
                this.setService(response.result);
                runInAction(() => {
                    this.service = response.result;
                })
            } catch (e) {
                console.log(e);
            }
            finally {
                this.setLoading(false);
            }
        }
    }

    get services() {
        return Array.from(this.serviceRegistry.values());
    }

    private getService = (id: number) => {
        return this.serviceRegistry.get(id);
    }
    private setService = (service: Service) => {
        this.serviceRegistry.set(service.id, service);
    }
}
