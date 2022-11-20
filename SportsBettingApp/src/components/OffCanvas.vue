<template>
    <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvas" aria-labelledby="offcanvasLabel">
        <div class="offcanvas-header">
            <h4>Administration</h4>
            <button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
        </div>
        <div class="offcanvas-body">
            <ul class="nav nav-tabs" id="myTab" role="tablist">
                <li class="nav-item" role="presentation">
                    <button
                        :class="$store.getters.activeAdministrationTab == 'tickets' ? 'nav-link active' : 'nav-link'"
                        data-bs-toggle="tab" data-bs-target="#tickets" @click="setAdministrationTab('tickets')"
                        type="button" role="tab" aria-controls="tickets" aria-selected="true">Tickets</button>
                </li>
                <li class="nav-item" role="presentation">
                    <button :class="$store.getters.activeAdministrationTab == 'wallet' ? 'nav-link active' : 'nav-link'"
                        data-bs-toggle="tab" data-bs-target="#wallet" @click="setAdministrationTab('wallet')"
                        type="button" role="tab" aria-controls="wallet" aria-selected="false">Wallet</button>
                </li>
            </ul>
            <div class="tab-content">
                <div :class="$store.getters.activeAdministrationTab == 'tickets' ? 'tab-pane fade show active' : 'tab-pane fade'"
                    id="tickets" role="tabpanel" aria-labelledby="tickets-tab">
                    <TicketList />
                </div>
                <div :class="$store.getters.activeAdministrationTab == 'wallet' ? 'tab-pane fade show active' : 'tab-pane fade'"
                    id="wallet" role="tabpanel" aria-labelledby="wallet-tab">
                   <Wallet />
                </div>
            </div>
        </div>
    </div>
</template>
<style>
</style>
<script lang="ts">
import { defineComponent } from "vue";
import TicketList from "./TicketList.vue";
import Wallet from "./Wallet.vue"
export default defineComponent({
    name: "offcanvas-component",
    components: {
        TicketList,
        Wallet
    },
    data() {
        return {
        }
    },
    methods: {
        setAdministrationTab(tabName: string) {
            this.$store.dispatch('setAdministrationTab', tabName);
        }
    }
});
</script>