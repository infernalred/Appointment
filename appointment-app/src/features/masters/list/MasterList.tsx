import React, { Fragment, useEffect } from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../app/store/store";
import { Grid, Header } from "semantic-ui-react";
import MasterItem from "./MasterItem";
import MasterListItemPlaceholder from "./MasterListItemPlaceholder";

export default observer(function MasterList() {
  const { masterStore } = useStore();
  const { loadMasters, masterRegistry, masters, loading } = masterStore;

  useEffect(() => {
    if (masterRegistry.size <= 1) {
      loadMasters();
    }
  }, [masterRegistry.size, loadMasters]);

  return (
    <Grid centered>
      <Grid.Column mobile={16} computer={4}>
        <Header textAlign="center" as={"h1"}>
          Наши мастера:
        </Header>
        {loading ? (
          <>
            <MasterListItemPlaceholder />
            <MasterListItemPlaceholder />
          </>
        ) : (
          <Fragment>
            {masters.map((master) => (
              <MasterItem key={master.id} master={master} />
            ))}
          </Fragment>
        )}
      </Grid.Column>
    </Grid>
  );
});
