import {Service, ServiceFormValues} from "../models/Service";
import {makeAutoObservable, runInAction} from "mobx";
import agent from "../api/agent";

export default class ServiceStore {
    service: Service | undefined = undefined;
    serviceRegistry = new Map<string, Service>();
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

    loadService = async (id: string) => {
        let service = this.getService(id);
        if (service) {
            this.service = service;
            return service;
        } else {
            this.setLoading(true);
            try {
                const response = await agent.Services.details(id);
                service = response.result;
                this.setService(response.result);
                runInAction(() => {
                    this.service = response.result;
                })
                return service;
            } catch (e) {
                console.log(e);
            }
            finally {
                this.setLoading(false);
            }
        }
    }

    createService = async (service: ServiceFormValues) => {
        try {
            await agent.Services.create(service);
            const newService = new Service(service);
            this.setService(newService);
            runInAction(() => {
                this.service = newService;
            })
        } catch (e) {
            console.log(e);
        }
    }

    updateService = async (service: ServiceFormValues) => {
        try {
            await agent.Services.update(service);
            if (service.id) {
                const updateService = {...this.getService(service.id), ...service};
                this.setService(updateService as Service);
                runInAction(() => {
                    this.service = updateService as Service;
                })
            }
        } catch (e) {
            console.log(e);
        }
    }

    get services() {
        return Array.from(this.serviceRegistry.values());
    }

    private getService = (id: string) => {
        return this.serviceRegistry.get(id);
    }
    private setService = (service: Service) => {
        this.serviceRegistry.set(service.id, service);
    }
}
