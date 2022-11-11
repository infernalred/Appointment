import React from "react";
import {observer} from "mobx-react-lite";
import {Grid} from "semantic-ui-react";
import {useStore} from "../../app/store/store";
import ProfileHeader from "./ProfileHeader";
import ProfileContent from "./ProfileContent";

export default observer(function ProfilePage() {
    const {userStore} = useStore();
    const {user} = userStore;

    return (
        <Grid centered>
            <Grid.Column mobile={16} computer={6}>
                {user &&
                    <>
                        <ProfileHeader profile={user} />
                        <ProfileContent />
                    </>}
            </Grid.Column>
        </Grid>
    );
});
