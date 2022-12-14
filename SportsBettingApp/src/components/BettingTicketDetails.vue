<template>

    <div>
        <b-modal size="lg" v-model="modalShow" title="Ticket details">
            <div class="pair-table">
                <div v-for="pair in bettingTicket.ticketPairs" :key="pair.bettingPair.id" >
                    <div :class="!getResultForPair(pair.bettingPair) ? 'table-row' : (pair.tip.name.includes(getResultForPair(pair.bettingPair).winningTip) ? 'table-row win' : 'table-row  lost')">
                        <div class="name">{{ pair.bettingPair.firstOpponent }} - {{ pair.bettingPair.secondOpponent }}</div>
                        <div class="time">{{ new Date(pair.bettingPair.matchStartUTC).toLocaleString() }}</div>
                        <div class="stake">{{ pair.tip.stake }}</div>
                        <div class="selected-tip">{{ pair.tip.name }}</div>
                        <div class="score">{{ displayResult(getResultForPair(pair.bettingPair)) }}</div>
                    </div>
                    
                </div>

            </div>
            <div class="summary">
                <div class="info-ticket"><span class="title-text">STAKE:</span>
                    {{ formatPrice(bettingTicket.totalStake) }}</div>
                <div class="info-ticket"><span class="title-text">BET AMOUNT:</span>
                    {{ formatPrice(bettingTicket.betAmount) }} € <span class="title-text"
                        style="font-size:small !important;">({{ formatPrice(bettingTicket.betAmountFinal) }} € +
                        {{ formatPrice(bettingTicket.manipulationCost) }} € mc)</span></div>
                <div class="info-ticket"><span class="title-text">PAYOUT:</span>
                    {{ formatPrice(bettingTicket.payoutAmount) }} €</div>
            </div>
            <div class="tax-info">
                <span class="tax-title">TAX:</span>
                <span class="tax-value">{{ formatPrice(bettingTicket.taxAmount) }} €</span>
            </div>
        </b-modal>
    </div>

</template>

<style scoped>
.win {
    background-color: green;
}
.lost {
    background-color: red;
}
.info-ticket {
    box-sizing: border-box;
    color: rgb(45, 55, 62);
    cursor: default;
    display: table-cell;
    font-family: "Open Sans", sans-serif;
    font-size: 18px;
    font-weight: 700;
    text-align: left;
    vertical-align: top;
    -moz-text-size-adjust: none
}

.title-text {
    box-sizing: border-box;
    color: rgb(153, 153, 153);
    cursor: default;
    font-family: "Open Sans", sans-serif;
    font-size: 16.2px;
    font-weight: 400;
    text-align: left;
    -moz-text-size-adjust: none
}

.tax-info {
    width: 100%;
    padding: 0px 10px;
}

.tax-value {
    color: #666;
    padding: 15px;
    font-size: 12px;
}

.tax-title {

    padding: 5px 0;
    font-size: 12px;
    font-weight: bold;
    text-transform: uppercase;

}

.summary {
    display: grid;
    grid-template-columns: 1.5fr 4.5fr 2fr;
    border-bottom-color: rgb(197, 207, 214);
    border-bottom-style: solid;
    border-bottom-width: 2px;
    padding: 20px 10px 0px 10px
}

.pair-table {
    border-top: solid;
    border-right: solid;
    border-left: solid;
    border-width: 1px;
}

.table-row {
    display: grid;
    grid-template-columns: 7fr 3fr 0.7fr 0.3fr 2fr;
    border-bottom: solid;
    border-width: 1px;

}

.name {
    padding-left: 10px;
}

.time {
    text-align: center;
}

.stake {
    text-align: center;
}

.selected-tip {
    text-align: center;
}

.score {
    padding-left: 10px;
}

.modal-footer>.btn-secondary {
    display: none !important;
}
</style>
<script lang="ts">
import { defineComponent, PropType } from "vue";
import BettingTicket from "@/types/BettingTicket";
import { BModal } from "bootstrap-vue-3";
import BettingPair from "@/types/BettingPair";
import BettingPairResult from "@/types/BettingPairResult";
import DataService from "@/services/DataService";
import ResponseData from "@/types/ResponseData";

export default defineComponent({
    props: {
        bettingTicket: {
            type: Object as PropType<BettingTicket>,
            required: true
        }
    },
    components: {
        BModal
    },
    data() {
        return {
            modalShow: false,
            resultList: [] as BettingPairResult[]
        }
    },
    watch: {

        bettingTicket: {
            handler(value) {
                if(value.id)
                {
                    this.loadResults();
                    this.modalShow = true;
                }
                
            },
        },
        modalShow: {
            handler(val) {
                if(val == false)
                {
                    // emit the event so that parent know we have closed the modal
                    this.$emit('modalClosed')
                }
            },
        }
    },
    methods: {
        formatPrice(value: number) {
            if (typeof value !== "number") {
                return value;
            }
            var formatter = new Intl.NumberFormat("hr-HR", {
                style: "currency",
                currency: "EUR",
                currencyDisplay: "code",
            });
            return formatter.format(value).replace("EUR", "").trim();
        },
        loadResults() {
            DataService.getResults()
                .then((response: ResponseData) => {
                    this.resultList = response.data
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
        displayResult(result: BettingPairResult)
        {
            if(result)
            {
                 return '(' + result.firstOpponentScore + ' - ' + result.secondOpponentScore + ')';
            }
           
            return "Not started"
        },
        getResultForPair(bettingPair: BettingPair) : BettingPairResult {
            let result = this.resultList.filter(r => r.bettingPair.id == bettingPair.id)
            if(result)
            {
                return result[0]
            }
            
            return null as unknown as BettingPairResult;
        }
    }
});

</script>