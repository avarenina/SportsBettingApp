<template>
    <div class="container">
        <GlobalNotification />
        <TimeNavigation />
        <div class="row">
            <div class="sports col-2 ">
                <SportsNavigation />
            </div>
            <div class="col-7">
                <SportsList />
            </div>
            <div class="col-3">
                <BettingTicket />
            </div>
        </div>
    </div>
    <!--Offcanvas - wallet and tickets-->
    <OffCanvas />
</template>

<style>
.container {
    margin-top: 30px;
}
</style>
<script lang="ts">

import { defineComponent } from "vue";
import TimeNavigation from "@/components/TimeNavigation.vue";
import SportsNavigation from "@/components/SportsNavigation.vue";
import SportsList from "@/components/SportsList.vue";
import BettingTicket from "@/components/BettingTicket.vue";
import GlobalNotification from "@/components/GlobalNotification.vue";
import OffCanvas from "@/components/OffCanvas.vue";

export default defineComponent({
    name: "home-view",
    components: {
        TimeNavigation,
        SportsNavigation,
        SportsList,
        BettingTicket,
        GlobalNotification,
        OffCanvas
    }
});
</script>