import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { store } from "../store/store";

const AxiosInterceptorsSetup = (navigate: any) => {
    axios.interceptors.response.use(async response => {
        return response;
    }, (error: AxiosError) => {
        const { data, status, headers } = error.response as AxiosResponse;
        switch (status) {
            case 400:
                if (data.errors) {
                    const modalStateErrors = [];
                    for (const key in (data as any).errors) {
                        if (data.errors[key]) {
                            modalStateErrors.push(data.errors[key])
                        }
                    }
                    throw modalStateErrors.flat();
                } else {
                    toast.error(data);
                }
                break;
            case 401:
                if (status === 401 && headers["www-authenticate"]?.startsWith('Bearer error="invalid_token"')) {
                    store.userStore.logout();
                    toast.error("Сессия истекла - пожалуйста, авторизуйтесь снова");
                } else {
                    toast.error("Требуется авторизация");
                }
                break;
            case 403:
                toast.error("Доступ запрещен");
                break;
            case 404:
                navigate("/not-found");
                break;
            case 409:
                toast.error(data.error || "Произошла ошибка");
                break;
            case 500:
                store.commonStore.setServerError(data);
                navigate("/server-error");
                break;
        }
        return Promise.reject(error);
    })
}

export default AxiosInterceptorsSetup;