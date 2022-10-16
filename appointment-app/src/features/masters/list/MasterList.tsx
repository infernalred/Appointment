import React, {useEffect} from "react";
import {observer} from "mobx-react-lite";
import {useStore} from "../../../app/store/store";

export default observer(function MasterList() {
    const {masterStore} = useStore();
    const {loadMasters, masterRegistry, master, loading} = masterStore;

    useEffect(() => {
        if (masterRegistry.size <= 1) {
            loadMasters();
        }
    }, [masterRegistry.size, loadMasters])

    if (loading) return <h1>Загрузка</h1>
    return (
        <h1>Мастеры</h1>
    )
})
