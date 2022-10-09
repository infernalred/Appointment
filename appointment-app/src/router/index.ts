import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";

import ServicesList from "@/views/services/ServicesList.vue";
import MastersList from "@/views/masters/MastersList.vue";
import ServiceDetails from "@/views/services/ServiceDetails.vue";
import MasterDetails from "@/views/masters/MasterDetails.vue";
// import MasterConfirm from "@/views/masters/MasterConfirm.vue";

const routes: Array<RouteRecordRaw> = [
  { path: "/", redirect: "/services" },
  { path: "/services", component: ServicesList },
  { path: "/services/:id", component: ServiceDetails },
  { path: "/masters", component: MastersList },
  {
    path: "/masters/:id",
    component: MasterDetails,
    props: true,
  },
  // { path: "/masters/:id/confirm", component: MasterConfirm },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
