import React from "react";
import {observer} from "mobx-react-lite";
import DashboardMasters from "./DashboardMasters";
import DashboardServices from "./DashboardServices";
import DashboardTimeSettings from "./DashboardTimeSettings";
import DashboardMyAppointments from "./DashboardMyAppointments";
import {Tab} from "semantic-ui-react";
import {useStore} from "../../app/store/store";

export default observer(function DashboardContent() {
    const {userStore} = useStore();
    const {isAdmin, isManager, isMaster} = userStore;

    function panes() {
        const panes = [];
        if (isAdmin || isManager) {
            panes.push({menuItem: "Мастера", render: () => <DashboardMasters />});
            panes.push({menuItem: "Сервисы", render: () => <DashboardServices />});
        }
        if (isMaster) {
            panes.push({menuItem: "Брони", render: () => <DashboardMyAppointments />});
            panes.push({menuItem: "Настройки", render: () => <DashboardTimeSettings />});
        }
        return panes;
    }

    return (
        <Tab
            menu={{fluid: true, vertical: false}}
            panes={panes()}
            onTabChange={(e, data) => userStore.setActiveTab(data.activeIndex)}
        />
    )
})
