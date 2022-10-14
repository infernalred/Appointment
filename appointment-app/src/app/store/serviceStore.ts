import Service from "../models/Service";
import {makeAutoObservable} from "mobx";

export default class ServiceStore {
    service: Service | undefined = undefined;
    serviceRegistry = new Map<number, Service>();

    constructor() {
        makeAutoObservable(this)
    }
}
