import { createStore } from "vuex";
import Sport from "@/types/Sport";
import BettingPair from "@/types/BettingPair";
import SelectedPair from "@/types/SelectedPair";
import GlobalNotification from "@/types/GlobalNotification";
import BettingDay from "@/types/BettingDay";
import BettingTicket from "@/types/BettingTicket";
import { WalletTransaction } from "@/types/WalletTransaction";

// TODO: Break it up into modules and refactor component to use map action/getters
export default createStore({
  state: {
    sportsList: [] as Sport[],
    activeSportsList: [] as Sport[],
    selectedPairsList: [] as SelectedPair[],
    bettingTicketsList: [] as BettingTicket[],
    administrationTab: 'tickets',
    walletAvailableFunds: 0,
    activeBettingDay: {} as BettingDay,
    globalNotifictationList: [] as GlobalNotification[],
    globalNotificationsIndex: 0,
    walletTransactionList: [] as WalletTransaction[],
    appLoading: false,
  },
  mutations: {
    UPDATE_SELECTED_PAIRS(state, pairList: SelectedPair[]) {
      state.selectedPairsList = pairList;
    },
    UPDATE_SPORTS(state, sportsList: Sport[]) {
      state.sportsList = sportsList;
      state.activeSportsList = sportsList;
    },
    UPDATE_ACTIVE_SPORTS(state, activeSportsList: Sport[]) {
      state.activeSportsList = activeSportsList;
    },
    UPDATE_ACTIVE_BETTING_DAY(state, activeBettingDay: BettingDay) {
      state.activeBettingDay = activeBettingDay;
    },
    UPDATE_GLOBAL_NOTIFICATIONS(state, globalNotifictationList: GlobalNotification[]) {
      state.globalNotifictationList = globalNotifictationList;
    },
    UPDATE_ADMINISTRATION_TAB(state, administrationTab: string) {
      state.administrationTab = administrationTab;
    },
    UPDATE_BETTING_TICKETS(state, bettingTicketsList: BettingTicket[]) {
      state.bettingTicketsList = bettingTicketsList;
    },
    UPDATE_AVAILABLE_FUNDS(state, walletAvailableFunds: number) {
      state.walletAvailableFunds = walletAvailableFunds;
    },
    UPDATE_TRANSACTION_LIST(state, walletTransactionList: WalletTransaction[]) {
      state.walletTransactionList = walletTransactionList;
    },
    UPDATE_APP_LOADING(state, status: boolean) {
      state.appLoading = status;
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

    showGlobalNotification(context, globalNotification: GlobalNotification) {
      globalNotification.id = context.state.globalNotificationsIndex++;

      const notificationList = context.state.globalNotifictationList;
      notificationList.push(globalNotification);
      context.commit("UPDATE_GLOBAL_NOTIFICATIONS", notificationList);

      setTimeout(() => {
        const notificationList = context.state.globalNotifictationList.filter(
          (m) => m.id !== globalNotification.id
        );
        context.commit("UPDATE_GLOBAL_NOTIFICATIONS", notificationList);
      }, globalNotification.duration);
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
    updateWalletAvailableFunds(context, amount: number) {
      context.commit("UPDATE_AVAILABLE_FUNDS", amount);
    },
    updateTransactionsList(context, newTransactionsList: WalletTransaction[]): void {
      const transactionsList = newTransactionsList;
      context.commit("UPDATE_TRANSACTION_LIST", transactionsList);
    },
    addTransactionToList(context, newTransaction: WalletTransaction): void {
      const transactionsList = context.state.walletTransactionList;
      transactionsList.unshift(newTransaction);
      context.commit("UPDATE_TRANSACTION_LIST", transactionsList);
    },
    updateAppLoadingStatus(context, status: boolean) {
      context.commit("UPDATE_APP_LOADING", status);
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
    walletAvailableFunds: function (state) :number {
      return state.walletAvailableFunds;
    },
    walletTransactionList: function (state) : WalletTransaction[] {
      return state.walletTransactionList;
    },
    appLoading: function (state) : boolean {
      return state.appLoading;
    }
  },
});
