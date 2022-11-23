import React, { Fragment } from "react";
import { Button, Card, Placeholder, Segment } from "semantic-ui-react";

export default function ServiceListItemPlaceholder() {
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
        <Card.Content>
          <Placeholder>
            <Placeholder.Paragraph>
              <Placeholder.Line length="full" />
              <Placeholder.Line length="full" />
            </Placeholder.Paragraph>
          </Placeholder>
        </Card.Content>
        <Card.Content extra>
          <Button positive disabled>
            Подробнее
          </Button>
        </Card.Content>
      </Card>
    </Fragment>
  );
}
