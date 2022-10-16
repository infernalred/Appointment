import React from "react";
import Service from "../../../app/models/Service";

interface Props {
    service: Service
}

export default function ServiceDetailsInfo({service}: Props) {
    return (
        <h1>Info</h1>
    )
}
