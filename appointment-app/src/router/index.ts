import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";

import ServicesList from "@/views/services/ServicesList.vue";
import MastersList from "@/views/masters/MastersList.vue";

const routes: Array<RouteRecordRaw> = [
  // {
  //   path: "/",
  //   name: "home",
  //   component: HomeView,
  // },
  // {
  //   path: "/about",
  //   name: "about",
  //   // route level code-splitting
  //   // this generates a separate chunk (about.[hash].js) for this route
  //   // which is lazy-loaded when the route is visited.
  //   component: () =>
  //     import(/* webpackChunkName: "about" */ "../views/AboutView.vue"),
  // },
  { path: "/", redirect: "/services" },
  { path: "/services", component: ServicesList },
  { path: "/masters", component: MastersList },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
