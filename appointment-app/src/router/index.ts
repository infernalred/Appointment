import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";

import ServicesList from "@/views/services/ServicesList.vue";
import MastersList from "@/views/masters/MastersList.vue";
import ServiceDetail from "@/views/services/ServiceDetail.vue";

const routes: Array<RouteRecordRaw> = [
  { path: "/", redirect: "/services" },
  { path: "/services", component: ServicesList },
  { path: "/services/:id", component: ServiceDetail },
  { path: "/masters", component: MastersList },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
