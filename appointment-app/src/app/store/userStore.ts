import {Account} from "../models/Account";
import {makeAutoObservable} from "mobx";
import agent from "../api/agent";
import {LoginModel} from "../models/LoginModel";


export default class UserStore {
    user: Account | null = null;
    token = "";
    timer: NodeJS.Timeout | undefined = undefined;
    appLoaded = false;
    activeTab = 0;

    constructor() {
        makeAutoObservable(this)
    }

    get isLoggedIn() {
        return !!this.user;
    }

    login = async (creds: LoginModel) => {
        const response = await agent.Users.login(creds);

        localStorage.setItem("appointment_token", response.token);
        localStorage.setItem("appointment_user", JSON.stringify(response));

        const jwt = JSON.parse(atob(response.token.split(".")[1]));
        const expDate = jwt.exp * 1000 - new Date().getTime();
        console.log(expDate);
        console.log(new Date(expDate).getTime());
        console.log(new Date().getTime());

        this.setAccount(response, response.token, expDate)
        this.setAppLoaded();
    }

    tryLogin = () => {
        const token = localStorage.getItem("appointment_token");
        const user = localStorage.getItem("appointment_user");

        if (!token) {
            this.setAppLoaded();
            return;
        }

        const jwt = JSON.parse(atob(token.split(".")[1]));
        const expiresIn = jwt.exp * 1000 - new Date().getTime();
        console.log("ExpresIn");
        console.log(expiresIn);

        if (expiresIn < 0) {
            this.setAppLoaded();
            return;
        }

        if (token && user) {
            this.setAccount(JSON.parse(user), token, expiresIn);
            this.setAppLoaded();
        }
    }

    logout = () => {
        localStorage.removeItem("appointment_token");
        localStorage.removeItem("appointment_user");
        clearTimeout(this.timer);
        this.token = "";
        this.user = null;
    }

    setActiveTab = (activeTab: any) => {
        this.activeTab = activeTab;
    }

    private setAccount = (user: Account, token: string, expiresIn: number) => {
        this.token = token;
        this.user = user;
        this.timer = setTimeout(() => this.logout(), expiresIn);
    }

    private setAppLoaded = () => {
        this.appLoaded = true;
    }
}
