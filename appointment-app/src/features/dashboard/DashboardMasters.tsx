import React, {useEffect} from "react";
import {observer} from "mobx-react-lite";
import {Button, Card, Grid, Tab} from "semantic-ui-react";
import {useStore} from "../../app/store/store";

export default observer(function DashboardMasters() {
    const {masterStore} = useStore();
    const {masterRegistry, masters, loadMasters} = masterStore;

    useEffect(() => {
        if (masterRegistry.size <= 1) {
            loadMasters();
        }
    }, [masterRegistry.size, loadMasters])
    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                    <Button size={"small"} positive content={"+"}/>
                    <Card.Group itemsPerRow={2}>
                        {masters.map(master => (
                            <Card key={master.id}>
                                <Card.Header>{master.displayName}</Card.Header>
                                <Button size={"small"} positive>Изменить</Button>
                            </Card>
                        ))}
                    </Card.Group>
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    )
})
