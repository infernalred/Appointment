import React from "react";
import {Account} from "../../app/models/Account";
import {observer} from "mobx-react-lite";
import DashboardMasters from "./DashboardMasters";
import DashboardServices from "./DashboardServices";
import DashboardTimeSettings from "./DashboardTimeSettings";
import DashboardMyAppointments from "./DashboardMyAppointments";
import {Tab} from "semantic-ui-react";
import {useStore} from "../../app/store/store";


interface Props {
    profile: Account;
}

export default observer(function DashboardContent() {
    const {userStore} = useStore();

    const panes = [
        {menuItem: "Мастера", render: () => <DashboardMasters />},
        {menuItem: "Сервисы", render: () => <DashboardServices />},
        {menuItem: "Настройки", render: () => <DashboardTimeSettings />},
        {menuItem: "Брони", render: () => <DashboardMyAppointments />},
    ]

    return (
        <Tab
            menu={{fluid: true, vertical: false, width: 50}}
            panes={panes}
            onTabChange={(e, data) => userStore.setActiveTab(data.activeIndex)}
        />
    )
})
