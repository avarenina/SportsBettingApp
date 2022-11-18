import { createStore } from "vuex";
import DataService from "@/services/DataService";
import Sport from "@/types/Sport";
import ResponseData from "@/types/ResponseData";
import BettingPair from "@/types/BettingPair";
import Tip from "@/types/Tip";
import SelectedPair from "@/types/SelectedPair";
import GlobalNotification from "@/types/GlobalNotification";
import BettingDay from "@/types/BettingDay";
import BettingTicket from "@/types/BettingTicket";
export default createStore({
  state: {
    sportsList: [] as Sport[],
    activeSportsList: [] as Sport[],
    selectedPairsList: [] as SelectedPair[],
    bettingTicketsList: [] as BettingTicket[],
    administrationTab: 'tickets',
    activeBettingDay: {} as BettingDay,
    globalNotifictationList: [] as GlobalNotification[],
    globalNotificationsIndex: 0,
  },
  mutations: {
    UPDATE_SELECTED_PAIRS(state, payload) {
      state.selectedPairsList = payload;
    },
    UPDATE_SPORTS(state, payload) {
      state.sportsList = payload;
      state.activeSportsList = payload;
    },
    UPDATE_ACTIVE_SPORTS(state, payload) {
      state.activeSportsList = payload;
    },
    UPDATE_ACTIVE_BETTING_DAY(state, payload) {
      state.activeBettingDay = payload;
    },
    UPDATE_GLOBAL_NOTIFICATIONS(state, payload) {
      state.globalNotifictationList = payload;
    },
    UPDATE_ADMINISTRATION_TAB(state, payload) {
      state.administrationTab = payload;
    },
    UPDATE_BETTING_TICKETS(state, payload) {
      state.bettingTicketsList = payload;
    },
  },
  actions: {
    updateSportsList(context, newSportsList: Sport[]): void {
      const sportsList = newSportsList;
      context.commit("UPDATE_SPORTS", sportsList);
    },
    setActiveSport(context, sportId: number) {
      const sportsList = context.state.sportsList.filter(
        (s) => s.id == sportId
      );
      context.commit("UPDATE_ACTIVE_SPORTS", sportsList);
    },
    addToSelectedPairList(context, selectedPair: SelectedPair) {
      const selectedPairsList = context.state.selectedPairsList;
      selectedPairsList.push(selectedPair);
      context.commit("UPDATE_SELECTED_PAIRS", selectedPairsList);
    },
    removeFromSelectedPairList(context, bettingPair: BettingPair) {
      const selectedPairsList = context.state.selectedPairsList.filter(
        (sp) => sp.bettingPair.id !== bettingPair.id
      );
      context.commit("UPDATE_SELECTED_PAIRS", selectedPairsList);
    },
    clearSelectedPairList(context) {
      const selectedPairsList = [] as SelectedPair[];
      context.commit("UPDATE_SELECTED_PAIRS", selectedPairsList);
    },

    showGlobalNotification(context, payload) {
      payload.id = context.state.globalNotificationsIndex++;

      const notificationList = context.state.globalNotifictationList;
      notificationList.push(payload);
      context.commit("UPDATE_GLOBAL_NOTIFICATIONS", notificationList);

      setTimeout(() => {
        const notificationList = context.state.globalNotifictationList.filter(
          (m) => m.id !== payload.id
        );
        context.commit("UPDATE_GLOBAL_NOTIFICATIONS", notificationList);
      }, payload.duration);
    },
    setActiveDay(context, activeDay: BettingDay) {
      context.commit("UPDATE_ACTIVE_BETTING_DAY", activeDay);
    },
    setAdministrationTab(context, activeTab: string) {
      context.commit("UPDATE_ADMINISTRATION_TAB", activeTab);
    },
    updateBettingTicketsList(context, newBettingTicketsList: BettingTicket[]): void {
      const bettingTicketsList = newBettingTicketsList;
      context.commit("UPDATE_BETTING_TICKETS", bettingTicketsList);
    },
    addBettingTicketList(context, newBettingTicket: BettingTicket): void {
      const bettingTicketsList = context.state.bettingTicketsList;
      bettingTicketsList.unshift(newBettingTicket);
      context.commit("UPDATE_BETTING_TICKETS", bettingTicketsList);
    },
  },
  getters: {
    selectedPairsList: function (state): SelectedPair[] {
      return state.selectedPairsList;
    },
    sportsList: function (state): Sport[] {
      return state.sportsList;
    },
    activeSportsList: function (state): Sport[] {
      return state.activeSportsList;
    },
    globalNotificationList: function (state): GlobalNotification[] {
      return state.globalNotifictationList;
    },
    activeBettingDay: function (state): BettingDay {
      return state.activeBettingDay;
    },
    activeAdministrationTab: function (state): string {
      return state.administrationTab;
    },
    bettingTicketsList: function (state): BettingTicket[] {
      return state.bettingTicketsList;
    },

  },
});
