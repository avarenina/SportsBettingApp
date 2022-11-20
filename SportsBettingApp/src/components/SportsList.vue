<template>
    <div class="accordion">
        <SpecialOffer />
        <div v-for="sport in $store.getters.activeSportsList" :key="sport.id" class="accordion-item">
            <h2 class="accordion-header" :id="'sport-category-panel-heading-' + sport.id">
                <button class="accordion-button" type="button" data-bs-toggle="collapse"
                    :data-bs-target="'#sport-category-panel-collapse-' + sport.id" aria-expanded="true"
                    :aria-controls="'sport-category-panel-collapse-' + sport.id">
                    <i :class="sport.icon"></i>&nbsp; {{ sport.name }}
                </button>
            </h2>
            <div :id="'sport-category-panel-collapse-' + sport.id" class="accordion-collapse collapse show"
                :aria-labelledby="'sport-category-panel-heading-' + sport.id">
                <div class="accordion-body">
                    <div v-if="filteredBettingPairs.filter((bp) => bp.sport.id == sport.id).length > 0
                    " class="table-responsive">
                        <table class="table">
                            <thead>
                                <tr class="tips">
                                    <th scope="col" class="tipovi-info" colspan="6"></th>
                                    <th v-for="tip in sport.availableTips" :key="tip.id" scope="col"
                                        class="tip td-shrink">
                                        {{ tip.name.toUpperCase() }}
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="pair in filteredBettingPairs.filter(
                                    (bp) => bp.sport.id == sport.id
                                )" :key="pair.id">
                                    <td class="bet-pair" colspan="6">
                                        <span class="competitor">{{ pair.firstOpponent }}</span> -
                                        <span class="competitor">{{ pair.secondOpponent }}</span>
                                        <span class="pair-time">
                                            <small>{{ formatDate(pair.matchStartUTC) }}</small>
                                        </span>
                                    </td>
                                    <td v-for="tip in sport.availableTips" :key="tip.id" style="padding: 0px !important"
                                        class="bet-quote td-shrink clickable">
                                        <div v-if="pair.tips.some((t) => t.name == tip.name)" style="padding: 0.5rem !important" @click="toggleSelection(pair, tip.name)"
                                            :class="$store.getters.selectedPairsList.some((sp: SelectedPair) => sp.bettingPair.id === pair.id && sp.tip.id === getTipByName(pair, tip.name).id) ? 'quote-selected' : ''">
                                            {{getTipByName(pair, tip.name).stake}}
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
    </div>
    
</template>

<style>
.pair {
    display: inline-block;
}

.list-empty {
    padding: 5px 15px 5px 15px;
}

.pair-time {
    float: right;
    top: 50%;
}

.quote-selected {
    background-color: #d3d8db !important;
}

.bet-pair {
    vertical-align: middle;
}

.competitor {
    line-height: 14px;
    font-weight: 600;
}

.bet-quote {
    text-align: center;
    border-left: 1px solid #dce2e6;
    border-spacing: 0px;
    vertical-align: middle;
}

.clickable {
    cursor: pointer;
}

.tip {
    border-left: 1px solid #b7c5d0;
    white-space: nowrap;
    text-align: center;
    padding: 0 4px;
    min-width: 34px;
}

.td-shrink {
    width: 1px;
}

.tips {
    background-color: #d3d8db;
    line-height: 10px;
}

.table {
    font-size: small;
    border: 1px solid black;
    width: 100% !important;
    min-width: 100% !important;
}

.clickable:hover {
    background-color: #d3d8db;
}

.table-responsive {
    margin: 0px !important;
}

.accordion-body {
    padding: 0px !important;
}
</style>

<script lang="ts">
import { defineComponent } from "vue";
import DataService from "@/services/DataService";
import Sport from "@/types/Sport";
import ResponseData from "@/types/ResponseData";
import BettingPair from "@/types/BettingPair";
import Tip from "@/types/Tip";
import SelectedPair from "@/types/SelectedPair";
import BettingDay from "@/types/BettingDay";
import SpecialOffer from "./SpecialOffer.vue";

export default defineComponent({
    name: "sports-list",
    components: {
        SpecialOffer
    },
    data() {
        return {
            bettingPairs: [] as BettingPair[],
            filteredBettingPairs: [] as BettingPair[],
            selectedPairs: [] as SelectedPair[],
            currentSport: {} as Sport,
            currentIndex: -1,
            name: "",
        };
    },
    methods: {
        retrieveBettingPairs() {
            DataService.getBettingPairs()
                .then((response: ResponseData) => {
                    this.bettingPairs = response.data;
                    console.log(response.data)
                    this.filterBettingPairs();
                })
                .catch((e: Error) => {
                    console.log(e);
                    this.$store.dispatch("showGlobalNotification", {
                        id: 0,
                        type: "error",
                        message: e,
                        duration: 4000,
                    });
                });
        },
        getTipByName(bettingPair: BettingPair, name: string) : Tip{
            let tip = bettingPair.tips.find(t => t.name === name);
            if(tip)
            {
                return tip;
            }
            
            return null as unknown as Tip;
        },
        getStakeFromTip(tips: Tip[], tipName: string): number {
            for (let index = 0; index < tips.length; ++index) {
                if (tips[index].name == tipName) {
                    return tips[index].stake;
                }
            }
            return 0;
        },
        addPairToList(bettingPair: BettingPair, selectedTip: string) {
            let tip = bettingPair.tips.find((t: Tip) => t.name === selectedTip);

            this.$store.dispatch("addToSelectedPairList", {
                bettingPair: bettingPair,
                tip: tip,
            });
        },
        removePairFromList(bettingPair: BettingPair) {
            this.$store.dispatch("removeFromSelectedPairList", bettingPair);
        },
        toggleSelection(bettingPair: BettingPair, selectedTip: string) {
           
            // If we have betting pair then check the tip, if different then remove the selected pair and push new one, otherwise just remove it
            if (this.$store.getters.selectedPairsList.some((sp: SelectedPair) => sp.bettingPair.id === bettingPair.id)) {
                if (!this.$store.getters.selectedPairsList.some((sp: SelectedPair) => sp.tip.name === selectedTip) || 
                !this.$store.getters.selectedPairsList.some((sp: SelectedPair) => sp.tip.id === this.getTipByName(bettingPair, selectedTip).id)) {
                    this.removePairFromList(bettingPair);
                    this.addPairToList(bettingPair, selectedTip);
                } else {
                    this.removePairFromList(bettingPair);
                }
            } else {
                this.addPairToList(bettingPair, selectedTip);
            }
        },
        filterBettingPairs() {
            let activeDay = this.$store.getters.activeBettingDay as BettingDay;

            this.bettingPairs = this.bettingPairs.filter(
                (bp) => new Date(bp.matchStartUTC) > new Date()
            ); // Discard those that have already started

            if (activeDay.queryStringId) {
                const currentTime = new Date();
                // we need to handle these cases : 3h, 6h, 12h and all
                if (activeDay.queryStringId == "all") {
                    this.filteredBettingPairs = this.bettingPairs;
                } else if (activeDay.queryStringId == "3h") {
                    this.filteredBettingPairs = this.bettingPairs.filter(
                        (bp) =>
                            new Date(bp.matchStartUTC) <
                            new Date(currentTime.getTime() + 3 * 60 * 60 * 1000)
                    );
                } else if (activeDay.queryStringId == "6h") {
                    this.filteredBettingPairs = this.bettingPairs.filter(
                        (bp) =>
                            new Date(bp.matchStartUTC) <
                            new Date(currentTime.getTime() + 6 * 60 * 60 * 1000)
                    );
                } else if (activeDay.queryStringId == "12h") {
                    this.filteredBettingPairs = this.bettingPairs.filter(
                        (bp) =>
                            new Date(bp.matchStartUTC) <
                            new Date(currentTime.getTime() + 12 * 60 * 60 * 1000)
                    );
                } else {
                    this.filteredBettingPairs = this.bettingPairs.filter(
                        (bp) =>
                            new Date(bp.matchStartUTC).getDate() ==
                            new Date(activeDay.date).getDate()
                    );
                }
            } else {
                this.filteredBettingPairs = this.bettingPairs;
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
            this.filterBettingPairs();
        },
    },
    mounted() {
        this.retrieveBettingPairs();
    },
});
</script>
