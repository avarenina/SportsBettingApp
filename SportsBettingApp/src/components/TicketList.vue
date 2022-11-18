<template>
    <div class="ticket-list">
        <ul class="list-group">
            <li v-for="bettingTicket in $store.getters.bettingTicketsList" :key="bettingTicket.id" class="list-group-item">
                <div class="grid-container">
                    <div class="ticket-info">
                        <div>
                            <span>{{bettingTicket.betAmount}}</span>
                            <span class="icon icon-arrow"><font-awesome-icon icon="fa-solid fa-arrow-right" /></span>
                            <span>{{bettingTicket.winAmount}}</span>
                            
                        </div>
                        <div>
                            <span class="icon"><font-awesome-icon icon="fa-regular fa-clock" /></span>
                            <span>{{new Date(bettingTicket.ticketPlacedTime).toLocaleString()}}</span>
                        </div>
                    </div>
                    <div class="ticket-icon">
                        <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
                    </div>
                </div>

            </li>
        </ul>
    </div>
   
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


export default defineComponent({
    name: "ticket-list",
    components: {

    },
    data() {
        return {

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
    },
    mounted() {
        this.retrieveBettingTickets()
    },
});
</script>