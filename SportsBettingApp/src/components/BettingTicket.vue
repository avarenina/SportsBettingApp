<template>
  <div v-if="$store.getters.selectedPairsList.length != 0" class="card card-body ticket-preview">
    <div>
      <transition-group name="list" tag="ul" class="list-group ticket-items">
        <li v-for="pair in $store.getters.selectedPairsList" :key="pair.bettingPair.id"
          class="list-group-item ticket-item">
          <div class="item-content">
            <div class="pair-title">
              <div class="pair-name">
                {{ pair.bettingPair.firstOpponent }} -
                {{ pair.bettingPair.secondOpponent }}
              </div>
              <span @click="removePairFromList(pair.bettingPair)" class="pair-remove">
                <i class="fa-solid fa-xmark"></i>
              </span>
            </div>
            <div class="meta panel fluid discrete">
              <div class="panel-content items-left">
                <span class="item">
                  <span class="icon ico-clock-outilne"></span>
                  {{ formatDate(pair.bettingPair.matchStartUTC) }}
                </span>
              </div>
              <div class="items-right">
                <span class="tip">{{ pair.tip.name }}</span>
                <span class="item-distant tecaj">{{ pair.tip.stake }}</span>
              </div>
            </div>
          </div>
        </li>
      </transition-group>
    </div>
    <div class="ticket-summary">
      <div class="band quote-info">
        <div class="panel">
          <div class="panel-content">
            {{ $store.getters.selectedPairsList.length }} pairs
          </div>
          <div class="panel-content right">
            Total Stake: {{ Number(bettingTicket.totalStake.toFixed(3)) }}
          </div>
        </div>
      </div>
      <div class="band bet-amount">
        <div class="input-wrp">
          <div class="input-label">Bet Amount €</div>
          <input type="text" maxlength="10" v-model="displayBetAmount" @blur="isInputActive = false"
            @focus="isInputActive = true" />
          <div v-if="betErrorMessage != ''" class="arrow_box">
            {{ betErrorMessage }}
          </div>
        </div>
        <div class="mt">
          <span>-{{ formatPrice(bettingTicket.manipulationCost) }} € (5% mt) = </span><span>{{ bettingTicket.betAmountFinal.toFixed(2) }} €</span>
        </div>
      </div>
      <div class="band amounts">
        <div class="panel payout">
          <div class="panel-content title">Payout:</div>
          <div class="panel-content right">
            {{ formatPrice(bettingTicket.payoutAmount) }} €
          </div>
        </div>
        <div class="panel win">
          <div class="panel-content title">Win:</div>
          <div class="panel-content right">{{ formatPrice(bettingTicket.winAmount) }} €</div>
        </div>
        <div class="panel tax">
          <div class="panel-content">
            <span class="tax">Tax:</span>
          </div>
          <div class="panel-content right">{{ formatPrice(bettingTicket.taxAmount) }} €</div>
        </div>
      </div>
    </div>

    <div class="betting-controls">
      <div class="d-grid gap-2">
        <button class="btn btn-primary btn-bet" :disabled="betErrorMessage != ''" @click="placeBet()" type="button">
          Place Bet
        </button>
      </div>
    </div>
  </div>
</template>
<style>
.pair-name {
  display: inline-block;
}

.pair-remove {
  display: none;
  float: right;
  cursor: pointer;
}

.pair-remove:hover {
  color: red;
}

.arrow_box {
  position: relative;
  background: #0d6efd;
  border: #0d6efd;
  border-radius: 3px;
  width: 100%;
  margin-top: 7px;
  color: var(--bs-gray-100);
  padding: 5px;
  font-size: 12px;
  font-weight: 300;
}

.arrow_box:after,
.arrow_box:before {
  bottom: 100%;
  left: 10%;
  border: solid transparent;
  content: "";
  height: 0;
  width: 0;
  position: absolute;
  pointer-events: none;
}

.arrow_box:after {
  border-color: rgba(0, 121, 213, 0);
  border-bottom-color: #0079d5;
  border-width: 5px;
  margin-left: -5px;
}

.arrow_box:before {
  border-color: rgba(0, 121, 213, 0);
  border-bottom-color: #0079d5;
  border-width: 6px;
  margin-left: -6px;
}

.ticket-item {
  padding-left: 5px !important;
  border-top: 0px !important;
  border-left: 0px !important;
  border-right: 0px !important;
  display: flex !important;
}

.list-enter-active,
.list-leave-active {
  transition: all 0.5s ease;
}

.list-enter-from,
.list-leave-to {
  opacity: 0;
  transform: translateX(30px);
}

.ticket-item:hover {
  background-color: var(--bs-list-group-action-hover-bg);
}

.ticket-item:hover .pair-remove {
  display: inline;
}

.ticket-preview {
  padding: 0px !important;
}

.ticket-item:last-child {
  border-bottom-left-radius: 0px !important;
  border-bottom-right-radius: 0px !important;
}

.btn-bet {
  border-top-left-radius: 0px !important;
  border-top-right-radius: 0px !important;
}

.ticket-summary {
  background-color: #e7f1ff;
  padding: 5px;
}

.quote-info {
  font-size: small;
  font-weight: 600;
  padding: 0px 5px 15px 5px;
}

.mt {
  float: right;
  font-size: x-small;
  font-weight: 600;
}

.band.bet-amount .input-label {
  position: absolute;
  left: 10px;
  color: #2d373e;
  line-height: 34px;
  font-size: 12px;
  font-weight: 600;
}

input {
  text-align: right;
  box-sizing: border-box;
  width: 100%;
  line-height: 20px;
  border: 1px solid white;
  padding: 4px 5px 4px 5px;
  border-radius: 3px;
  font-size: 14px;
  outline: 0;
}

.panel {
  display: table;
  table-layout: fixed;
  width: 100%;
}

.panel-content {
  display: table-cell;
  vertical-align: top;
  text-align: left;
}

.panel .right {
  text-align: right;
}

.input-wrp {
  position: relative;
}

.amounts {
  line-height: 22px;
}

.amounts {
  font-size: 13px;
  font-weight: 600;
  padding-left: 5px;
}

.select {
  width: 20px;
  align-items: center;
  display: flex;
  justify-content: center;
}

.item-content {
  font-size: 12px;
  flex-grow: 1;
  padding-left: 10px;
}

.meta {
  font-size: 12px;
  line-height: 19px;
  color: #636f78;
  margin-top: 3px;
}

.items-left {
  text-align: left;
}

.items-left .item {
  margin-right: 5px;
}

.meta .tip {
  display: inline-block;
  padding: 0 5px;
  border-right: 1px solid #c5cfd6;
  border-left: 1px solid #c5cfd6;
  border-top: 1px solid #c5cfd6;
  border-bottom: 1px solid #c5cfd6;
  border-radius: 3px;
  background-color: rgba(255, 255, 255, 0.6);
}

.item-distant {
  margin-left: 10px;
}

.items-right {
  float: right;
}
</style>
<script lang="ts">
import { defineComponent } from "vue";
import BettingPair from "@/types/BettingPair";
import SelectedPair from "@/types/SelectedPair";
import BettingTicket from "@/types/BettingTicket";
import BettingService from "@/services/BettingService";
import GlobalNotification from "@/types/GlobalNotification";

export default defineComponent({
  name: "betting-ticket",
  components: {},
  data() {
    return {

      bettingTicket: {
        ticketPairs: this.$store.state.selectedPairsList as SelectedPair[],
        totalStake:0,
        betAmount:5,
        manipulationCost: 0,
        betAmountFinal: 0,
        payoutAmount: 0,
        winAmount: 0,
        taxAmount: 0,
        isWinningTicket: false
      } as BettingTicket,

      
      isInputActive: false,
      maxBetAmount: 20000,
      minBetAmount: 5,
      betErrorMessage: "",
    };
  },
  watch: {
    "$store.state.selectedPairsList": {
      handler(val) {
        this.reCalculateTicketVariables(val);
      },
      deep: true,
    },
    "bettingTicket.betAmount": {
      handler(val) {
        this.reCalculateTicketVariables(this.$store.getters.selectedPairsList);
        if (val > this.maxBetAmount) {
          this.betErrorMessage = "Maximum bet amount is 20.000,00 €";
        } else if (val < this.minBetAmount) {
          this.betErrorMessage = "Minimum bet amount is 5,00 €";
        } else {
          this.betErrorMessage = "";
        }
      },
    },
  },
  computed: {
    displayBetAmount: {
      get: function (): string {
        if (this.isInputActive) {
          // Cursor is inside the input field. unformat display value for user
          return this.bettingTicket.betAmount.toString();
        } else {
          // User is not modifying now. Format display value for user interface
          return this.formatPrice(this.bettingTicket.betAmount);
        }
      },
      set: function (modifiedValue: string) {
        let newValue = parseFloat(modifiedValue);
        // Ensure that it is not NaN
        if (isNaN(newValue)) {
          newValue = 0;
        }

        this.bettingTicket.betAmount = newValue;
      },
    },
  },
  methods: {
    reCalculateTicketVariables(selectedPairsList: SelectedPair[]) {
      let calculatedStake = 1;

      for (let index = 0; index < selectedPairsList.length; index++) {
        calculatedStake = calculatedStake * selectedPairsList[index].tip.stake;
      }

      this.bettingTicket.totalStake = calculatedStake; // We have the total stake here, now multiply it with betAmount
      this.bettingTicket.manipulationCost = this.bettingTicket.betAmount * 0.05; // manipulation costs of 5%
      this.bettingTicket.betAmountFinal = this.bettingTicket.betAmount - this.bettingTicket.manipulationCost;
      this.bettingTicket.winAmount = this.bettingTicket.betAmountFinal * this.bettingTicket.totalStake;

      // Apply tax classes here
      /*
                0       - 10.000    kn	10 % -- class 1
                10.000  - 30.000    kn	15 % -- class 2
                30.000  - 500.000   kn	20 % -- class 3
                500.000 - inf       kn  30 % -- class 4
                */

      let taxBase = this.bettingTicket.winAmount - this.bettingTicket.betAmountFinal;

      // Todo: display them as info on tax screen
      let taxClass1Amount = 0;
      let taxClass2Amount = 0;
      let taxClass3Amount = 0;
      let taxClass4Amount = 0;

      if (taxBase > 500000) {
        taxClass4Amount = (taxBase - 500000) * 0.3;
        taxClass3Amount = (500000 - 30000) * 0.2;
        taxClass2Amount = (30000 - 10000) * 0.15;
        taxClass1Amount = 10000 * 0.1;
      } else if (taxBase > 30000) {
        taxClass3Amount = (taxBase - 30000) * 0.2;
        taxClass2Amount = (30000 - 10000) * 0.15;
        taxClass1Amount = 10000 * 0.1;
      } else if (taxBase > 10000) {
        taxClass2Amount = (taxBase - 10000) * 0.15;
        taxClass1Amount = 10000 * 0.1;
      } else {
        taxClass1Amount = taxBase * 0.1;
      }

      this.bettingTicket.taxAmount =
        taxClass1Amount + taxClass2Amount + taxClass3Amount + taxClass4Amount;

      this.bettingTicket.payoutAmount = this.bettingTicket.winAmount - this.bettingTicket.taxAmount;
    },
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
    removePairFromList(bettingPair: BettingPair) {
      this.$store.dispatch("removeFromSelectedPairList", bettingPair);
    },
    showNotification(message: string, type: string = 'success') {

      let notification: GlobalNotification = {
        type: type,
        message: message,
        duration: 4000,
      }

      this.$store.dispatch("showGlobalNotification", notification);
    },
    formatDate(date: string) {
      const days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

      const d = new Date(date);

      return days[d.getDay()] + ", " + d.getHours() + ":" + d.getMinutes();
    },
    placeBet() {
      BettingService.placeBet(this.bettingTicket).then((response) => {
          // We have succesifully placed a ticket
          // Push ticket to the list
          this.resetBettingTicket();
          this.$store.dispatch("addBettingTicketList", response.data);
          this.$store.dispatch("clearSelectedPairList");
          this.showNotification('Betting ticket submitted with success!')
        }).catch((error) => {
          if (error.response) {
            // Request made and server responded
            this.showNotification(error.response.data, 'danger')

          } else {
            // Something happened in setting up the request that triggered an Error or there is no response from server
            this.showNotification("Error placing the bet! Please try again!", 'danger')
          }
        });
    },
    resetBettingTicket() {
      this.bettingTicket = {
            ticketPairs: this.$store.state.selectedPairsList as SelectedPair[],
            totalStake:0,
            betAmount:5,
            manipulationCost: 0,
            betAmountFinal: 0,
            payoutAmount: 0,
            winAmount: 0,
            taxAmount: 0,
            isWinningTicket: false
          } as BettingTicket;
    }
  },
});
</script>
