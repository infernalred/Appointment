import React from "react";
import { Navigate  } from "react-router-dom";
import { useStore } from "../store/store";

interface Props {
    children: JSX.Element
}

export default function PrivateRoute({children}: Props) {
    const {userStore: {isLoggedIn}} = useStore();
    
    return isLoggedIn ? children : <Navigate to="/auth" />;
}