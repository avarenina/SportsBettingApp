import { createStore } from 'vuex'


import DataService from "@/services/DataService";
import Sport from "@/types/Sport";
import ResponseData from "@/types/ResponseData";
import BettingPair from "@/types/BettingPair";
import Tip from "@/types/Tip";
import SelectedPair from "@/types/SelectedPair";
import Notification from "@/types/Notification";
import Time from '@/types/Time';


export default createStore({
    state: {
        sportsList: [] as Sport[],
        activeSportsList: [] as Sport[],
        selectedPairsList: [] as SelectedPair[],

        activeBettingDay: {} as Time,
        globalNotifictationList: [] as Notification[],
        globalNotificationsIndex: 0,
    },
    mutations: {
        UPDATE_SELECTED_PAIRS(state, payload) {
            state.selectedPairsList = payload
        },
        UPDATE_SPORTS(state, payload) {
            state.sportsList = payload
            state.activeSportsList = payload
        },
        UPDATE_ACTIVE_SPORTS(state, payload) {
            state.activeSportsList = payload
        },
        UPDATE_ACTIVE_BETTING_DAY(state, payload) {
            state.activeBettingDay = payload
        },
        UPDATE_GLOBAL_NOTIFICATIONS(state, payload) {
            state.globalNotifictationList = payload
        },
    },
    actions: {
        updateSportsList(context, payload) {
            const sportsList = payload;
            context.commit('UPDATE_SPORTS', sportsList)
        },
        setActiveSport(context, payload) {
            const sportsList = context.state.sportsList.filter(s => s.id == payload);
            context.commit('UPDATE_ACTIVE_SPORTS', sportsList)
        },
        addToSelectedPairList(context, payload) {
            const selectedPairsList = context.state.selectedPairsList
            selectedPairsList.push(payload)
            context.commit('UPDATE_SELECTED_PAIRS', selectedPairsList)
        },
        removeFromSelectedPairList(context, payload) {
            const selectedPairsList = context.state.selectedPairsList.filter(sp => sp.bettingPair.id !== payload.id);
            context.commit('UPDATE_SELECTED_PAIRS', selectedPairsList)
        },
        showGlobalNotification(context, payload) {

            payload.id = context.state.globalNotificationsIndex++

            const notificationList = context.state.globalNotifictationList
            notificationList.push(payload)
            context.commit('UPDATE_GLOBAL_NOTIFICATIONS', notificationList)

            setTimeout(() => {
                const notificationList = context.state.globalNotifictationList.filter(m => m.id !== payload.id)
                context.commit('UPDATE_GLOBAL_NOTIFICATIONS', notificationList)
            }, payload.duration);

        },
        setActiveDay(context, activeDay: Time)
        {
            context.commit('UPDATE_ACTIVE_BETTING_DAY', activeDay)
        }
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
        globalNotificationList: function (state): Notification[] {
            return state.globalNotifictationList;
        },
        activeBettingDay: function (state): Time {
            return state.activeBettingDay;
        }
    }
})
