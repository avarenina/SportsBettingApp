<template>
    <div class="row">
        <div class="sport-days">

            <router-link class="day" to="/day/3h">3h</router-link>
            <router-link class="day" to="/day/6h">6h</router-link>
            <router-link class="day" to="/day/12h">12h</router-link>
            <router-link v-for="bettingDay in bettingDays" :key="bettingDay.id" class="day"
                :to="'/day/' + bettingDay.queryStringId">
                {{ bettingDay.label }}
            </router-link>
            <router-link class="day" to="/day/all">ALL</router-link>
            <a data-bs-toggle="offcanvas" href="#offcanvasExample" @click="setAdministrationTab('wallet')" role="button"
        aria-controls="offcanvasExample" class="day" style="float:right;color:#f23535 !important;font-weight: 600;">Wallet</a>
            <a data-bs-toggle="offcanvas" href="#offcanvasExample" @click="setAdministrationTab('tickets')" role="button"
        aria-controls="offcanvasExample" class="day" style="float:right;color:#f23535 !important;font-weight: 600;">Tickets</a>
        </div>
    </div>
</template>
<style>
.sport-days .day {
    display: inline-block;
    height: 28px;
    position: relative;
    top: 2px;
    padding: 3px 10px;
    font-size: 14px;
    vertical-align: bottom;
    border-bottom: 2px solid #c5cfd6;
    cursor: pointer;
    color: #39444c !important;
    text-decoration: none !important;
}

.day.active {
    color: #f23535 !important;
    border-bottom-color: #f23535;
}

.sport-days {
    width: 100%;
    margin-bottom: 15px;
    text-align: left;
    color: #39444c !important;
    border-bottom: 2px solid #c5cfd6;
}
</style>

<script lang="ts">

import { defineComponent } from 'vue';
import DataService from "@/services/DataService";
import ResponseData from "@/types/ResponseData";
import BettingDay from "@/types/BettingDay";
import router from "@/router";

export default defineComponent({
    name: "time-navigation",
    components: {

    },
    data() {
        return {
            bettingDays: [] as BettingDay[],
            currentBettingDay: {} as BettingDay
        };
    },
    methods: {
        retrieveTimes() {
            DataService.getTimes()
                .then((response: ResponseData) => {
                    this.bettingDays = response.data;
                    this.setActiveDay(this.$route.params.day as string);
                })
                .catch((e: Error) => {
                    console.log(e);
                });
        },
        setActiveDay(queryStringId: string) {
            let activeDay = this.bettingDays.find((d: BettingDay) => d.queryStringId == queryStringId);
            if (!activeDay) {
                // Means we have either all, 3hrs, 6hr or 12hrs window
                if (queryStringId == 'all' || queryStringId == '3h' || queryStringId == '6h' || queryStringId == '12h') {
                    activeDay = { id: 0, date: '', label: queryStringId, queryStringId: queryStringId } as BettingDay
                }
            }
            this.$store.dispatch('setActiveDay', activeDay);
        },
        setAdministrationTab(tabName: string) {
            this.$store.dispatch('setAdministrationTab', tabName);
        }
    },
    watch: {
        '$route'() {
            if (!this.$route.params.day) {
                router.push('/day/all')
            } else {
                this.setActiveDay(this.$route.params.day as string);
            }
        }
    },
    mounted() {
        this.retrieveTimes();
        if (!this.$route.params.day) {
            router.push('/day/all')
        }
    },
});
</script>