import { makeAutoObservable, runInAction } from "mobx"
import {AppointmentSlot} from "../models/AppointmentSlot";
import agent from "../api/agent";
export default class AppointmentStore {
    appointments: AppointmentSlot[] = []; 
    loading = false;

    constructor() {
        makeAutoObservable(this)
    }

    loadMyAppointmentsByDate = async () => {
        this.loading = true;
        try {
            const result = await agent.Appointments.myAppointments();
            runInAction(() => {
                this.appointments = result.result;
            })
        } catch (e) {
            console.log(e);
        }
        finally {
            this.loading = false;
        }
    }
}
