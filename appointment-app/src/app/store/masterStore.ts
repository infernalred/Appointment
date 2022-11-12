import Master from "../models/Master";
import {makeAutoObservable, runInAction} from "mobx";
import agent from "../api/agent";
import SlotModel from "../models/SlotModel";
import {SlotParams} from "../models/SlotParams";
import {AppointmentSlot} from "../models/AppointmentSlot";
import {RegisterMaster} from "../models/RegisterMaster";

export default class MasterStore {
    master: Master | undefined = undefined;
    masterRegistry = new Map<string, Master>();
    loading = false;
    slotParams = new SlotParams();
    masterSlotsRegistry = new Map<number, SlotModel[]>();
    slotLoading = false;
    days: Date[] = [] ;
    selected: AppointmentSlot | undefined = undefined;

    constructor() {
        makeAutoObservable(this)
    }

    setLoading = (state: boolean) => {
        this.loading = state;
    }

    setSlotLoading = (state: boolean) => {
        this.slotLoading = state;
    }

    setSlotParams = (slotParams: SlotParams) => {
        this.slotParams = slotParams;
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
                if (response.result) {
                    this.setMaster(response.result);
                    runInAction(() => {
                        this.master = response.result;
                    })
                }
            } catch (e) {
                console.log(e);
            }
            finally {
                this.setLoading(false);
            }
        }
    }

    loadMasterSlots = async (id: string) => {
        this.setSlotLoading(true);
        try {
            this.masterSlotsRegistry.clear();
            this.initWeekDays();
            this.clearSelected();
            const response = await agent.Masters.freeSlots(id, this.slotParams);
            response.result.forEach(x => {x.start = new Date(x.start); x.end = new Date(x.end)});
            const day = new Date(this.slotParams.start);
            runInAction(() => {
                for (let i = 0; i <= this.slotParams.quantityDaysNumber; i++) {
                    const dayNumber = day.getDay();
                    const slots = response.result.filter(x => x.start.getDay() === dayNumber);
                    this.masterSlotsRegistry.set(dayNumber, slots);
                    day.setDate(day.getDay() + 1)
                }
            })
        } catch (e) {
            console.log(e);
        } finally {
            this.setSlotLoading(false);
        }
    }

    createMaster = async (registerMaster: RegisterMaster) => {
        await agent.Masters.create(registerMaster);
        await this.loadMasters();
    }

    setSelected = (appointment: AppointmentSlot) => {
        this.selected = appointment;
    }
    clearSelected = () => {
        this.selected = undefined;
    }

    saveAppointment = async (appointment: AppointmentSlot) => {
        try {
            await agent.Appointments.create(appointment);
            this.clearSelected();
        } catch (e) {
            console.log(e);
        }
    }

    get masters() {
        return Array.from(this.masterRegistry.values());
    }

    get weekLabelDate() {
        const date = new Date(this.slotParams.start);
        date.setDate(date.getDate() + this.slotParams.quantityDaysNumber);
        return `${this.slotParams.start.toLocaleString([], {
            month: "long",
        })} ${this.slotParams.start.getDate()}-${date.getDate()}`;
    }

    private getMaster = (id: string) => {
        return this.masterRegistry.get(id);
    }
    private setMaster = (master: Master) => {
        this.masterRegistry.set(master.id, master);
    }

    private initWeekDays = () => {
        const copy = [] as Date[];
        const date = new Date(this.slotParams.start);
        for (let i = 0; i <= this.slotParams.quantityDaysNumber; i++) {
            copy.push(new Date(date));
            date.setDate(date.getDate() + 1);
        }
        this.days = copy;
    }
}
