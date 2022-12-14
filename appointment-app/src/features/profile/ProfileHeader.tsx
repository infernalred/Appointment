import React from "react";
import {observer} from "mobx-react-lite";
import {Grid, Header, Item, Segment} from "semantic-ui-react";
import {Account} from "../../app/models/Account";

interface Props {
    profile: Account;
}

export default observer(function ProfilePage({profile}: Props) {
    return (
        <Segment>
            <Grid>
                <Grid.Column width={16}>
                    <Item.Group>
                        <Item>
                            <Item.Image avatar size={"small"} src={"/assets/user.png"}/>
                            <Item.Content verticalAlign={"middle"}>
                                <Header as={"h1"} content={profile.userName}/>
                            </Item.Content>
                        </Item>
                    </Item.Group>
                </Grid.Column>
            </Grid>
        </Segment>
    )
})
