import {makeAutoObservable, runInAction} from "mobx";
import agent from "../api/agent";
import { TimeSlot } from "../models/TimeSlot";

export default class TimeSlotStore {
    loading = false;
    slots: TimeSlot[] = [];
    
    constructor() {
        makeAutoObservable(this)
    }
}