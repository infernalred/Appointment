import React, { Fragment } from "react";
import { Card, Segment, Button, Placeholder } from "semantic-ui-react";

export default function MasterListItemPlaceholder() {
  return (
    <Fragment>
      <Card fluid>
        <Segment
          attached="top"
          inverted
          color="teal"
          style={{ border: "none", minHeight: 50 }}
        >
          <Placeholder>
            <Placeholder.Header>
              <Placeholder.Line length="full" />
            </Placeholder.Header>
          </Placeholder>
        </Segment>
        <Placeholder fluid>
          <Placeholder.Image square />
        </Placeholder>
        <Card.Content extra>
          <Button floated={"right"} positive disabled>
            Подробнее
          </Button>
        </Card.Content>
      </Card>
    </Fragment>
  );
}
