import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import HomeView from "../views/HomeView.vue";
import ResultsView from "../views/ResultsView.vue";
import TicketsView from "../views/TicketsView.vue";
import WalletView from "../views/WalletView.vue";
const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "home",
    component: HomeView,
  },
  {
    path: "/day/:day?",
    name: "day",
    alias: "/day/:day/sport/:id",
    component: HomeView,
  },
  {
    path: "/results",
    name: "results",
    component: ResultsView,
  },
  {
    path: "/tickets",
    name: "tickets",
    component: TicketsView,
  },
  {
    path: "/wallet",
    name: "wallet",
    component: WalletView,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
  linkActiveClass: "active",
  linkExactActiveClass: "selected",
});

export default router;
