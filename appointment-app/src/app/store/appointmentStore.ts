import { makeAutoObservable, runInAction } from "mobx"
import {AppointmentSlot} from "../models/AppointmentSlot";
import agent from "../api/agent";
import { AppointmentsOnDateParams } from "../models/AppointmentsOnDateParams";

export default class AppointmentStore {
    appointments: AppointmentSlot[] = []; 
    loading = false;

    constructor() {
        makeAutoObservable(this)
    }

    loadAppointmentsByDate = async (params: AppointmentsOnDateParams) => {
        this.loading = true;
        try {
            const response = await agent.Appointments.appointmentsOnDate(params);
            response.result.forEach(x => {x.start = new Date(x.start); x.end = new Date(x.end)});
            runInAction(() => {
                this.appointments = response.result;
            })
        } catch (e) {
            console.log(e);
        }
        finally {
            this.loading = false;
        }
    }
}
