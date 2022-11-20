<template>
<div class="accordion-item">
            <h2 class="accordion-header" id="'sport-category-panel-heading-special-offer">
                <button class="accordion-button" type="button" data-bs-toggle="collapse"
                    data-bs-target="#sport-category-panel-collapse-special-offer" aria-expanded="true"
                    aria-controls="sport-category-panel-collapse-special-offer">
                    <font-awesome-icon icon="fa-solid fa-trophy" />&nbsp; Special Offer
                </button>
            </h2>
            <div id="sport-category-panel-collapse-special-offer" class="accordion-collapse collapse show"
                aria-labelledby="sport-category-panel-heading-special-offer">
                <div class="accordion-body">
                    <div v-if="filteredSpecialOfferList.length > 0
                    " class="table-responsive">
                        <table class="table">
                            <thead>
                                <tr class="tips">
                                    <th scope="col" class="tipovi-info" colspan="6"></th>
                                    <th v-for="tip in allTips" :key="tip.name" scope="col" class="tip td-shrink">
                                        {{tip.name.toUpperCase()}}
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="offer in filteredSpecialOfferList" :key="offer.id">
                                    <td class="bet-pair" colspan="6">
                                        <span class="competitor">{{offer.bettingPair.firstOpponent}}</span> -
                                        <span class="competitor">{{offer.bettingPair.secondOpponent}}</span>
                                        <span class="pair-time">
                                            <small>{{formatDate(offer.bettingPair.matchStartUTC)}}</small>
                                        </span>
                                    </td>
                                    <td v-for="tip in allTips" :key="tip.id" style="padding: 0px !important" class="bet-quote td-shrink clickable">
                                        <div v-if="offer.tips.some((t) => t.name == tip.name)" style="padding: 0.5rem !important" @click="toggleSelection(offer, tip.name)"
                                            :class="$store.getters.selectedPairsList.some((sp: SelectedPair) => sp.bettingPair.id === offer.bettingPair.id && sp.tip.id === getTipByName(offer,tip.name).id) ? 'quote-selected' : ''">
                                            {{getTipByName(offer,tip.name).stake}}
                                        </div>
                                        <div v-else style="padding: 0.5rem !important">
                                            -
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="list-empty" v-else>
                        There are no pairs for betting right now!
                    </div>
                </div>
            </div>
        </div>
</template>
<style>
</style>
<script lang="ts">
import { defineComponent } from "vue";
import DataService from "@/services/DataService";
import SpecialOffer from "@/types/SpecalOffer";
import ResponseData from "@/types/ResponseData";
import Tip from "@/types/Tip";
import SelectedPair from "@/types/SelectedPair";
import BettingPair from "@/types/BettingPair";
import BettingDay from "@/types/BettingDay";
export default defineComponent({
    name: "sports-list",
    data() {
        return {
            specialOfferList: [] as SpecialOffer[],
            filteredSpecialOfferList: [] as SpecialOffer[],
            allTips: [
                { name: "1" },
                { name: "x" },
                { name: "2" },
                { name: "1x" },
                { name: "x2" },
                { name: "12" }
            ] as Tip[]
        };
    },
    methods: {
        getSpecialOffers() {
            DataService.getSpecialOffer()
                .then((response: ResponseData) => {
                    this.specialOfferList = response.data;
                    this.filterSpecialOffer();
                })
                .catch((e: Error) => {
                    this.$store.dispatch("showGlobalNotification", {
                        id: 0,
                        type: "danger",
                        message: e,
                        duration: 4000,
                    });
                });
        },
        getTipByName(offer: SpecialOffer,name: string) : Tip{
            let tip = offer.tips.find(t => t.name === name);
            if(tip)
            {
                return tip;
            }
            
            return null as unknown as Tip;
        },
        addPairToList(offer: SpecialOffer, selectedTip: string) {
            let tip = offer.tips.find((t: Tip) => t.name === selectedTip);

            //only one special offer can be on ticket
            let specialOfferPair = this.$store.getters.selectedPairsList.find((sp: SelectedPair) => sp.isSpecialOffer === true) as SelectedPair;

            if(specialOfferPair)
            {
                this.removePairFromList(specialOfferPair.bettingPair);
            }

            // create new selectedPair
            this.$store.dispatch("addToSelectedPairList", {
                bettingPair: offer.bettingPair,
                isSpecialOffer:true,
                tip: tip,
            });
        },
        filterSpecialOffer() {
            let activeDay = this.$store.getters.activeBettingDay as BettingDay;

            this.specialOfferList = this.specialOfferList.filter(
                (bp) => new Date(bp.bettingPair.matchStartUTC) > new Date()
            ); // Discard those that have already started

            if (activeDay.queryStringId) {
                const currentTime = new Date();
                // we need to handle these cases : 3h, 6h, 12h and all
                if (activeDay.queryStringId == "all") {
                    this.filteredSpecialOfferList = this.specialOfferList;
                } else if (activeDay.queryStringId == "3h") {
                    this.filteredSpecialOfferList = this.specialOfferList.filter(
                        (bp) =>
                            new Date(bp.bettingPair.matchStartUTC) <
                            new Date(currentTime.getTime() + 3 * 60 * 60 * 1000)
                    );
                } else if (activeDay.queryStringId == "6h") {
                    this.filteredSpecialOfferList = this.specialOfferList.filter(
                        (bp) =>
                            new Date(bp.bettingPair.matchStartUTC) <
                            new Date(currentTime.getTime() + 6 * 60 * 60 * 1000)
                    );
                } else if (activeDay.queryStringId == "12h") {
                    this.filteredSpecialOfferList = this.specialOfferList.filter(
                        (bp) =>
                            new Date(bp.bettingPair.matchStartUTC) <
                            new Date(currentTime.getTime() + 12 * 60 * 60 * 1000)
                    );
                } else {
                    this.filteredSpecialOfferList = this.specialOfferList.filter(
                        (bp) =>
                            new Date(bp.bettingPair.matchStartUTC).getDate() ==
                            new Date(activeDay.date).getDate()
                    );
                }
            } else {
                this.filteredSpecialOfferList = this.specialOfferList;
            }
        },
        removePairFromList(bettingPair: BettingPair) {
            this.$store.dispatch("removeFromSelectedPairList", bettingPair);
        },
        toggleSelection(offer: SpecialOffer, selectedTip: string) {
            // If we have betting pair then check the tip, if different then remove the selected pair and push new one, otherwise just remove it
            if (this.$store.getters.selectedPairsList.some((sp: SelectedPair) => sp.bettingPair.id === offer.bettingPair.id)) {
                if (!this.$store.getters.selectedPairsList.some((sp: SelectedPair) => sp.tip.name === selectedTip)) {
                    this.removePairFromList(offer.bettingPair);
                    this.addPairToList(offer, selectedTip);
                } else {
                    this.removePairFromList(offer.bettingPair);
                }
            } else {
                this.addPairToList(offer, selectedTip);
            }
        },
        
        formatDate(date: string) {
            const days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

            const d = new Date(date);
            let hours = d.getHours().toString();
            let minutes = d.getMinutes().toString();
            // prepend zero
            if(!hours.startsWith('0') && hours.length == 1)
            {
                hours = '0' + hours;
            }

            if(!minutes.startsWith('0') && minutes.length == 1)
            {
                minutes = '0' + minutes;
            }

            return days[d.getDay()] + ", " + hours + ":" + minutes;
        },
    },
    watch: {
        $route() {
            this.filterSpecialOffer();
        },
    },
    mounted() {
       this.getSpecialOffers();
      
    },

});
</script>