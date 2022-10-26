import React from "react";
import {observer} from "mobx-react-lite";
import {Grid} from "semantic-ui-react";
import {useStore} from "../../app/store/store";
import DashboardHeader from "./DashboardHeader";
import DashboardContent from "./DashboardContent";

export default observer(function DashboardPage() {
    const {userStore} = useStore();
    const {user} = userStore;

    return (
        <Grid centered>
            <Grid.Column mobile={16} computer={6}>
                {user &&
                    <>
                        <DashboardHeader profile={user} />
                        <DashboardContent />
                    </>}
            </Grid.Column>
        </Grid>
    );
});
