import React, {useEffect} from "react";
import {observer} from "mobx-react-lite";
import {useStore} from "../../../app/store/store";
import {useParams} from "react-router-dom";
import {Grid} from "semantic-ui-react";
import ServiceDetailsHeader from "./ServiceDetailsHeader";
import LoadingComponent from "../../../app/layout/LoadingComponent";

export default observer(function ServiceDetails() {
    const {serviceStore} = useStore();
    const {loadService, service, loading} = serviceStore;
    const { id } = useParams<{ id: string }>();

    useEffect(() => {
        if (id) {
            loadService(id);
        }
    }, [id, loadService])

    if (loading || !service) return <LoadingComponent content="Загрузка данных..." />

    return (
        <Grid centered>
            <Grid.Column mobile={16} computer={4}>
                <ServiceDetailsHeader service={service}/>
            </Grid.Column>
        </Grid>
    )
})
