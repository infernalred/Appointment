import React from "react";
import { Link } from "react-router-dom";
import { Segment, Header, Icon, Button } from "semantic-ui-react";

export default function NotFound() {
    return (
        <Segment placeholder>
            <Header icon>
                <Icon name="search" />
                Извините, мы везде искали и не нашли.
            </Header>
            <Segment.Inline>
                <Button as={Link} to="/services" primary>
                    Вернуться к услугам
                </Button>
            </Segment.Inline>
        </Segment>
    )
}