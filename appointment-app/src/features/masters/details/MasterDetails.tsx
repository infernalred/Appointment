import React, {useEffect} from "react";
import {observer} from "mobx-react-lite";
import {useStore} from "../../../app/store/store";
import {useParams} from "react-router-dom";
import {Grid} from "semantic-ui-react";
import MasterDetailsHeader from "./MasterDetailsHeader";
import MasterDetailsTime from "./MasterDetailsTime";

export default observer(function MasterDetails() {
    const {masterStore} = useStore();
    const {loadMaster, master, loading} = masterStore;
    const { id } = useParams<{ id: string }>();

    useEffect(() => {
        if (id) {
            loadMaster(id);
        }
    }, [id, loadMaster])

    if (loading || !master) return <h1>Загрузка</h1>

    return (
        <Grid centered>
            <Grid.Column mobile={16} computer={4}>
                <MasterDetailsHeader master={master}/>
                <MasterDetailsTime id={master.id}/>
            </Grid.Column>
        </Grid>
    )
})
