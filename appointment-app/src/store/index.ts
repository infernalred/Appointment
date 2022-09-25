import { defineStore } from "pinia";
import Service from "@/models/Service";
import agent from "@/api/agent";
import Master from "@/models/Master";

export const useAppointmentStore = defineStore("appointment", {
  state: () => ({
    services: [] as Service[],
    service: {} as Service,
    masters: [] as Master[],
    master: {} as Master,
  }),
  actions: {
    async loadServices() {
      const response = await agent.Services.list();
      this.services = response.result;
    },
    async loadService(id: number) {
      const response = await agent.Services.details(id);
      this.service = response.result;
    },
    async loadMasters() {
      const response = await agent.Masters.list();
      this.masters = response.result;
    },
    async loadMaster(id: string) {
      const response = await agent.Masters.details(id);
      this.master = response.result;
    },
  },
  getters: {
    allServices(): Service[] {
      return this.services;
    },
    currentService(): Service {
      return this.service;
    },
    allMasters(): Master[] {
      return this.masters;
    },
    currentMaster(): Master {
      return this.master;
    },
  },
});
