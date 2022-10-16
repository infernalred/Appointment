import React, {useEffect} from "react";
import {observer} from "mobx-react-lite";
import {useStore} from "../../../app/store/store";
import {useParams} from "react-router-dom";
import {Grid} from "semantic-ui-react";

export default observer(function ServiceDetails() {
    const {serviceStore} = useStore();
    const {loadService, service, loading} = serviceStore;
    const { id } = useParams<{ id: string }>();

    useEffect(() => {
        if (id) {
            loadService(Number(id));
        }
    }, [id, loadService])

    if (loading) return <h1>Загрузка</h1>

    return (
        <Grid>
            <Grid.Column width={8}></Grid.Column>
        </Grid>
    )
})
