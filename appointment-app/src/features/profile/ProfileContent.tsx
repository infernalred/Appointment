import React from "react";
import {observer} from "mobx-react-lite";
import ProfileMasters from "./ProfileMasters";
import ProfileServices from "./ProfileServices";
import ProfileTimeSettings from "./ProfileTimeSettings";
import ProfileMyAppointments from "./ProfileAppointments";
import {Tab} from "semantic-ui-react";
import {useStore} from "../../app/store/store";

export default observer(function ProfileContent() {
    const {userStore} = useStore();
    const {isAdmin, isManager, isMaster} = userStore;

    function panes() {
        const panes = [];
        if (isAdmin || isManager) {
            panes.push({menuItem: "Мастера", render: () => <ProfileMasters />});
            panes.push({menuItem: "Сервисы", render: () => <ProfileServices />});
        }
        if (isMaster) {
            panes.push({menuItem: "Брони", render: () => <ProfileMyAppointments />});
            panes.push({menuItem: "Настройки", render: () => <ProfileTimeSettings />});
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
