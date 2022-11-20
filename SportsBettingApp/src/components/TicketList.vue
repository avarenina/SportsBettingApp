<template>
    <div class="ticket-list">
        <ul class="list-group" v-if="$store.getters.bettingTicketsList.length > 0">
            <li v-for="bettingTicket in $store.getters.bettingTicketsList" :key="bettingTicket.id"
                class="list-group-item">
                <div class="grid-container">
                    <div class="ticket-info">
                        <div>
                            <span data-bs-toggle="tooltip" data-bs-placement="top" title="Bet amount">
                                <font-awesome-icon icon="fa-solid fa-money-bill" /> {{
                                        formatPrice(bettingTicket.betAmount)
                                }} €
                            </span>
                            <span class="icon icon-arrow">
                                <font-awesome-icon icon="fa-solid fa-arrow-right" />
                            </span>
                            <span data-bs-toggle="tooltip" data-bs-placement="top" title="Possible win amount">
                                <font-awesome-icon icon="fa-solid fa-money-bills" /> {{
                                        formatPrice(bettingTicket.winAmount)
                                }} €
                            </span>

                        </div>
                        <div data-bs-toggle="tooltip" data-bs-placement="top" title="Ticket placed date">
                            <span class="icon">
                                <font-awesome-icon icon="fa-regular fa-clock" />
                            </span>
                            <span>{{ new Date(bettingTicket.ticketPlacedTime).toLocaleString() }}</span>
                        </div>
                    </div>
                    <div @click="loadBettingTicketDetails(bettingTicket)" class="ticket-icon" data-bs-toggle="tooltip"
                        data-bs-placement="top" title="View ticket">
                        <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
                    </div>
                </div>
            </li>
        </ul>
        <div v-else>
            There are no tickets available!
        </div>
    </div>
    <BettingTicketDetails :bettingTicket="selectedBettingTicket" @modalClosed="modalCloseCallback" />
</template>
<style>
.grid-container {
    display: grid;
    grid-template-areas:
        'info info info info icon'
        'info info info info icon'
}

.icon {
    margin-right: 5px;
}

.icon-arrow {
    margin-left: 5px;
    color: green;
}

.ticket-list {
    margin-top: 1rem;
}

.ticket-info {
    grid-area: info;

}

.ticket-icon {
    grid-area: icon;
    align-self: center;
    text-align: center;
    cursor: pointer;
}
</style>

<script lang="ts">
import { defineComponent } from "vue";
import BettingService from "@/services/BettingService";
import ResponseData from "@/types/ResponseData";
import BettingTicketDetails from "./BettingTicketDetails.vue";
import BettingTicket from "@/types/BettingTicket";

export default defineComponent({
    name: "ticket-list",
    components: {
        BettingTicketDetails
    },
    data() {
        return {
            selectedBettingTicket: {} as BettingTicket
        };
    },
    methods: {
        retrieveBettingTickets() {
            BettingService.getBettingTickets()
                .then((response: ResponseData) => {
                    this.$store.dispatch("updateBettingTicketsList", response.data);
                })
                .catch((e: Error) => {
                    console.log(e);
                });
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
        loadBettingTicketDetails(bettingTicket: BettingTicket) {
            this.selectedBettingTicket = bettingTicket;
        },
        modalCloseCallback() {
            this.selectedBettingTicket = {} as BettingTicket;
        }
    },
    mounted() {
        this.retrieveBettingTickets()
    },
});
</script>