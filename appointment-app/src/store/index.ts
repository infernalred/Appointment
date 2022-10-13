import { defineStore } from "pinia";
import Service from "@/models/Service";
import agent from "@/api/agent";
import Master from "@/models/Master";
import SlotParams from "@/models/SlotParams";
import SlotModel from "@/models/SlotModel";
import AppointmentSlot from "@/models/AppointmentSlot";
import Account from "@/models/Account";
import LoginModel from "@/models/LoginModel";

let timer: number;

export const useAppointmentStore = defineStore("appointment", {
  state: () => ({
    services: [] as Service[],
    service: {} as Service,
    masters: [] as Master[],
    master: {} as Master,
    masterSlots: [] as SlotModel[],
    selectedAppointment: null as null | AppointmentSlot,
    account: null as null | Account,
    token: "",
    didAutoLogout: false,
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
    async loadSlots(id: string, slotParams: SlotParams) {
      const response = await agent.Masters.freeSlots(id, slotParams);
      const slots = [] as SlotModel[];
      for (const key in response.result) {
        const slot = {
          id: response.result[key].id,
          start: new Date(response.result[key].start),
          end: new Date(response.result[key].end),
        } as SlotModel;
        slots.push(slot);
      }
      this.masterSlots = slots;
    },
    async saveAppointment(appointment: AppointmentSlot) {
      await agent.Appointments.create(appointment);
    },
    confirmAppointment(appointment: AppointmentSlot) {
      this.selectedAppointment = appointment;
    },
    clearAppointment() {
      this.selectedAppointment = null;
    },
    async login(cred: LoginModel) {
      const response = await agent.Users.login(cred);

      localStorage.setItem("appointment_token", response.token);
      localStorage.setItem("appointment_user", JSON.stringify(response));
      const jwt = JSON.parse(atob(response.token.split(".")[1]));
      const expDate = jwt.exp * 1000 - new Date().getTime();
      console.log(expDate);
      console.log(new Date(expDate).getTime());
      console.log(new Date().getTime());

      timer = setTimeout(() => this.autoLogout(), expDate);

      console.log("login");
      this.account = response;
      this.token = response.token;
    },
    tryLogin() {
      const token = localStorage.getItem("appointment_token");
      const user = localStorage.getItem("appointment_user");
      if (!token) {
        return;
      }
      const jwt = JSON.parse(atob(token.split(".")[1]));

      const expiresIn = jwt.exp * 1000 - new Date().getTime();
      console.log("ExpresIn");
      console.log(expiresIn);
      if (expiresIn < 0) {
        return;
      }
      timer = setTimeout(() => this.autoLogout(), expiresIn);

      if (token && user) {
        this.token = token;
        this.account = JSON.parse(user);
      }
    },
    logout() {
      localStorage.removeItem("appointment_token");
      localStorage.removeItem("appointment_user");
      clearTimeout(timer);
      this.token = "";
      this.account = null;
    },
    autoLogout() {
      console.log("autologout");
      this.logout();
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
    isAuthenticated(): boolean {
      return !!this.token;
    },
  },
});
