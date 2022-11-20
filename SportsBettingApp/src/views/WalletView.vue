<template>
    <div class="container">
        <GlobalNotification />
        <h4>TBD</h4>
    </div>

</template>

<style>
.container {
    margin-top: 30px;
}
</style>
<script lang="ts">

import { defineComponent } from "vue";
import GlobalNotification from "@/components/GlobalNotification.vue";

export default defineComponent({
    name: "results-view",
    components: {
        GlobalNotification
    }
});
</script>