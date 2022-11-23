import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../app/store/store";
import { useParams } from "react-router-dom";
import { Grid } from "semantic-ui-react";
import MasterDetailsHeader from "./MasterDetailsHeader";
import MasterDetailsTime from "./MasterDetailsTime";
import MasterDetailsHeaderPlaceholder from "./MasterDetailsHeaderPlaceholder";

export default observer(function MasterDetails() {
  const { masterStore } = useStore();
  const { loadMaster, master, loading } = masterStore;
  const { id } = useParams<{ id: string }>();

  useEffect(() => {
    if (id) {
      loadMaster(id);
    }
  }, [id, loadMaster]);

  return (
    <Grid centered>
      <Grid.Column mobile={16} computer={4}>
        <Grid centered>
          <Grid.Column width={8}>
            {loading || !master ? (
              <MasterDetailsHeaderPlaceholder />
            ) : (
              <MasterDetailsHeader master={master} />
            )}
          </Grid.Column>
        </Grid>
        {id && <MasterDetailsTime id={id} />}
      </Grid.Column>
    </Grid>
  );
});
