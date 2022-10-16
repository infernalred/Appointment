import React from "react";
import Service from "../../../app/models/Service";

interface Props {
    service: Service
}

export default function ServiceDetailsHeader({service}: Props) {
    return (
        <h1>Header</h1>
    )
}
