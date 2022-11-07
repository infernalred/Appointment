import {makeAutoObservable, runInAction} from "mobx";
import agent from "../api/agent";
import { TimeSlot, TimeSlotFormValues } from "../models/TimeSlot";
import {toast} from "react-toastify";

export default class TimeSlotStore {
    loading = false;
    slots: TimeSlot[] = [];
    days = new Map<number, Date>();
    slot: TimeSlot | undefined = undefined;

    constructor() {
        makeAutoObservable(this)
    }

    setLoading = (state: boolean) => {
        this.loading = state;
    }

    loadMySlots = async () => {
        this.setLoading(true);
        this.initWeekDays();
        try {
            const response = await agent.TimeSlots.list();
            response.result.forEach(x => {x.start = new Date(x.start); x.end = new Date(x.end)});
            runInAction(() => {
                this.slots = response.result.sort((a, b) => a.start.getTime() - b.start.getTime());
            })
        } catch (e) {
            console.log(e);
        }
        finally {
            this.setLoading(false);
        }
    }

    loadSlot = async (id: string) => {
        let slot = this.slots.find(x => x.id == id);
        if (slot) {
            this.slot = slot;
            return slot;
        } else {
            this.setLoading(true);
            try {
                const response = await agent.TimeSlots.details(id);
                runInAction(() => {
                    slot = response.result;
                    this.slot = slot;
                    const index = this.slots.findIndex(x => x.id === id);
                    this.slots.splice(index, 1);
                    this.slots.push(slot);
                })
                return slot;
            } catch (e) {
                console.log(e);
            }
            finally {
                this.setLoading(false);
            }
        }
    }

    createSlot = async (slot: TimeSlotFormValues) => {
        try {
            const result = await agent.TimeSlots.create(slot);
            if (result.error) {
                toast.error(result.error);
            } else {
                const newSlot = new TimeSlot(slot);
                runInAction(() => {
                    this.slots.push(newSlot);
                    this.slots = this.slots.sort((a, b) => a.start.getTime() - b.start.getTime())
                    this.slot = newSlot;
                })
            }
        } catch (e) {
            console.log(e);
        }
    }

    updateSlot = async (slot: TimeSlotFormValues) => {
        try {
            await agent.TimeSlots.update(slot);
        } catch (e) {
            console.log(e);
        }
    }

    delSlot = async (id: string) => {
        this.setLoading(true);
        try {
            const index = this.slots.findIndex(x => x.id === id);
            if (index > -1)
            {
                await agent.TimeSlots.delete(id);
                runInAction(() => {
                    this.slots.splice(index, 1);
                })
            }
        } catch (e) {
            console.log(e);
        }
        finally {
            this.setLoading(false);
        }
    }

    private initWeekDays = () => {
        const date = new Date();
        for (let i = 0; i < 7; i++) {
            this.setDay(new Date(date));
            date.setDate(date.getDate() + 1);
        }
    }

    getDay = (type: number) => {
        return this.days.get(type);
    }
    private setDay = (day: Date) => {
        this.days.set(day.getDay(), day);
    }
}