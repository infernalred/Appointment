import axios, { AxiosError } from "axios";
import { toast } from "react-toastify";
import OperationResult from "../models/OperationResult";
import { ServerError } from "../models/ServerError";
import { store } from "../store/store";

const AxiosInterceptorsSetup = (navigate: any) => {
    axios.interceptors.response.use(async response => {
        return response;
    }, (error: AxiosError) => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        const { data, status, headers } = error.response!;
        switch (status) {
            case 400:
                if ((data as any).errors) {
                    const modalStateErrors = [] as string[];
                    for (const key in (data as any).errors) {
                        if ((data as any).errors[key]) {
                            modalStateErrors.push((data as any).errors[key])
                        }
                    }
                    throw modalStateErrors.flat();
                } else {
                    toast.error(data as string);
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
                toast.error((data as OperationResult<never>).error || "Произошла ошибка");
                break;
            case 500:
                store.commonStore.setServerError(data as ServerError);
                navigate("/server-error");
                break;
        }
        return Promise.reject(error);
    })
}

export default AxiosInterceptorsSetup;