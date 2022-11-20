<template>
    <div class="card">
        <div class="card-body">
            <div class="input-group mb-3">
                <span class="input-group-text" id="available-funds-label">Available funds €</span>
                <input type="text" class="form-control" id="available-funds" aria-describedby="available-funds-label"
                    disabled :value="formatPrice($store.getters.walletAvailableFunds)">
            </div>
            <div class="form-floating mb-3">
                <input type="text" class="form-control" id="deposit" placeholder="Deposit amount"
                    v-model="displayDepositAmount" @blur="isInputActive = false" @focus="isInputActive = true">
                <div v-if="depositErrorMessage != ''" class="arrow_box">
                    {{ depositErrorMessage }}
                </div>
                <label for="deposit">Deposit amount</label>
                <div id="depositHelp" class="form-text">Enter the amount of € that you wish to deposit in your wallet!
                </div>
            </div>
            <button type="submit" class="btn btn-primary"
                :disabled="depositErrorMessage != '' || !walletTransaction.transactionAmount"
                @click="submitDeposit()">Deposit</button>
        </div>
    </div>
    <!-- Transaction list -->
    <div class="transaction-list">
        <ul class="list-group">
            <li class="list-group-item" v-for="transaction in $store.getters.walletTransactionList" :key="transaction.id">
                <div class="transaction-info">
                    <div class="transaction-id">{{transaction.id}}</div>
                    <div class="transaction-amount"><font-awesome-icon icon="fa-solid fa-money-bill-1-wave" /> <span :class="getTransactionType(transaction.transactionType).toLowerCase()">{{formatPrice(transaction.transactionAmount)}} €</span></div>
                    <div class="transaction-type"><font-awesome-icon icon="fa-solid fa-money-bill-transfer" /> <span>{{getTransactionType(transaction.transactionType)}}</span></div>
                    <div class="transaction-time"><font-awesome-icon icon="fa-regular fa-clock" /> {{new Date(transaction.transactionTimestamp).toLocaleString()}}</div>
                </div>
            </li>
        </ul>
    </div>
</template>
<style scoped>
.card {
    margin-top: 15px;
}
.transaction-info {
    display: grid;
    grid-template-columns: 0.5fr 4fr ;
    grid-template-areas:
        'id amount'
        'id type' 
        'id time' 
}

.deposit {
    color: green;
}

.transaction-id {
    grid-area: id;
    align-self: center;
    text-align: center;
}

.transaction-amount {
    grid-area: amount;
    padding-left: 15px;
}

.transaction-type {
    grid-area: type;
    padding-left: 15px;
}

.transaction-time {
    grid-area: time;
    padding-left: 15px;
}

.transaction-list {
    margin-top: 15px;
}
</style>
<script lang="ts">
import { defineComponent } from "vue";
import { WalletTransaction, TransactionType } from "@/types/WalletTransaction";
import WalletService from "@/services/WalletService";
import GlobalNotification from "@/types/GlobalNotification";
import ResponseData from "@/types/ResponseData";

export default defineComponent({
    name: "results-view",
    components: {
    },
    data() {
        return {
            walletTransaction: {} as WalletTransaction,
            isInputActive: false,
            maxDepositAmount: 20000,
            minDepositAmount: 5,
            depositErrorMessage: "",
        };
    },
    watch: {
        "walletTransaction.transactionAmount": {
            handler(val) {
                if (val > this.maxDepositAmount) {
                    this.depositErrorMessage = "Maximum deposit amount is 20.000,00 €";
                } else if (val < this.minDepositAmount) {
                    this.depositErrorMessage = "Minimum deposit amount is 5,00 €";
                } else {
                    this.depositErrorMessage = "";
                }
            },
        },
    },
    computed: {
        displayDepositAmount: {
            get: function (): string {
                if (this.isInputActive) {
                    // Cursor is inside the input field. unformat display value for user
                    return this.walletTransaction.transactionAmount.toString();
                } else {
                    // User is not modifying now. Format display value for user interface
                    return this.formatPrice(this.walletTransaction.transactionAmount);
                }
            },
            set: function (modifiedValue: string) {
                let newValue = parseFloat(modifiedValue);
                // Ensure that it is not NaN
                if (isNaN(newValue)) {
                    newValue = 0;
                }

                this.walletTransaction.transactionAmount = newValue;
            },
        },
    },
    methods: {
        getTransactions() {
            WalletService.getAllTransactions()
                .then((response: ResponseData) => {
                    this.$store.dispatch("updateTransactionsList", response.data);
                })
                .catch((e: Error) => {
                    console.log(e);
                });
        },
        getWalletBalance() {
            WalletService.getWalletBalance()
                .then((response: ResponseData) => {
                    this.$store.dispatch("updateWalletAvailableFunds", response.data);
                })
                .catch((e: Error) => {
                    console.log(e);
                });
        },
        getTransactionType(type: number) {
           return TransactionType[type];
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
        showNotification(message: string, type: string = 'success') {
            let notification: GlobalNotification = {
                type: type,
                message: message,
                duration: 4000,
            }
            this.$store.dispatch("showGlobalNotification", notification);
        },
        submitDeposit() {
            WalletService.deposit(this.walletTransaction).then((response) => {
                this.$store.dispatch("addTransactionToList", response.data);
                this.$store.dispatch("updateWalletAvailableFunds", response.data.walletFinalAmount);
                this.walletTransaction = {} as WalletTransaction;
                this.showNotification('Funds deposited with success!')
            }).catch((error) => {
                if (error.response) {
                    // Request made and server responded
                    this.showNotification(error.response.data, 'danger')

                } else {
                    // Something happened in setting up the request that triggered an Error or there is no response from server
                    this.showNotification("Error placing the bet! Please try again!", 'danger')
                }
            });
        }
    },
    mounted() {
        this.getTransactions();
        this.getWalletBalance();
        this.getTransactionType(1);
    },
});
</script>