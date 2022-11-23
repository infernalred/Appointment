import React from "react";
import { Segment, Placeholder } from "semantic-ui-react";

export default function MasterDetailsHeaderPlaceholder() {
  return (
    <Segment.Group>
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
      <Segment>
        <Placeholder fluid>
          <Placeholder.Image square />
        </Placeholder>
      </Segment>
    </Segment.Group>
  );
}
