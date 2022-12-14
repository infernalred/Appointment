import {Account} from "../models/Account";
import {makeAutoObservable} from "mobx";
import agent from "../api/agent";
import {LoginModel} from "../models/LoginModel";


export default class UserStore {
    user: Account | null = null;
    token = "";
    timer: NodeJS.Timeout | undefined = undefined;
    appLoaded = false;
    activeTab: 0 | string | number | undefined;
    roles: string[] = [];

    constructor() {
        makeAutoObservable(this)
    }

    get isLoggedIn() {
        return !!this.user;
    }

    get isAdmin() {
        return this.roles.includes("Admin");
    }

    get isManager() {
        return this.roles.includes("Manager");
    }

    get isMaster() {
        return this.roles.includes("Master");
    }

    login = async (creds: LoginModel) => {
        const response = await agent.Users.login(creds);

        localStorage.setItem("appointment_token", response.token);
        localStorage.setItem("appointment_user", JSON.stringify(response));

        const jwt = JSON.parse(window.atob(response.token.split(".")[1]));
        const expDate = jwt.exp * 1000 - new Date().getTime();
        const roles = [] as string[];
        if (typeof jwt.role === typeof "test") {
            roles.push(jwt.role)
        } else {
            roles.push(...jwt.role)
        }

        this.setAccount(response, response.token, expDate, roles)
        this.setAppLoaded();
    }

    tryLogin = () => {
        const token = localStorage.getItem("appointment_token");
        const user = localStorage.getItem("appointment_user");

        if (!token) {
            this.setAppLoaded();
            return;
        }

        const jwt = JSON.parse(window.atob(token.split(".")[1]));
        const expiresIn = jwt.exp * 1000 - new Date().getTime();
        const roles = [] as string[];
        if (typeof jwt.role === typeof "test") {
            roles.push(jwt.role)
        } else {
            roles.push(...jwt.role)
        }

        if (expiresIn < 0) {
            this.setAppLoaded();
            return;
        }

        if (token && user) {
            this.setAccount(JSON.parse(user), token, expiresIn, roles);
            this.setAppLoaded();
        }
    }

    logout = () => {
        localStorage.removeItem("appointment_token");
        localStorage.removeItem("appointment_user");
        clearTimeout(this.timer);
        this.token = "";
        this.user = null;
        this.roles = [];
    }

    setActiveTab = (activeTab: string | number | undefined) => {
        this.activeTab = activeTab;
    }

    private setAccount = (user: Account, token: string, expiresIn: number, roles: string[]) => {
        this.token = token;
        this.user = user;
        this.roles = roles;
        this.timer = setTimeout(() => this.logout(), expiresIn);
    }

    private setAppLoaded = () => {
        this.appLoaded = true;
    }
}
