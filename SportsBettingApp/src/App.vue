<template>

    <div class="main-layout">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
            <div class="container">
                <a class="navbar-brand" href="">Sports Betting App</a>
                <div class="collapse navbar-collapse">
                    
                </div>
            </div>
        </nav>
       <div class="loader-container" :style="$store.getters.appLoading === true ? 'display:block' : 'display:none'">
            <div class="spinner">
                <div></div>
                <div></div>
                <div></div>
                <div></div>
            </div>
        </div>
        <div :style="$store.getters.appLoading === true ? 'display:none' : 'display:block'">
            <router-view />
        </div>
        
    </div>
</template>

<style>
.loader-container {
    width: 100%;
    height: 100%;
    margin-left: 50%;
    margin-top: 10rem;
}

.spinner {
    width: 17.6px;
    height: 17.6px;
    animation: spinner-o824ag 1.2s infinite linear;
}

.spinner div {
    position: absolute;
    width: 100%;
    height: 100%;
    background: #474bff;
    border-radius: 50%;
    animation: spinner-vse6n7 1.5s infinite ease;
}

.spinner div:nth-child(1) {
    --rotation: 90;
}

.spinner div:nth-child(2) {
    --rotation: 180;
}

.spinner div:nth-child(3) {
    --rotation: 270;
}

.spinner div:nth-child(4) {
    --rotation: 360;
}

@keyframes spinner-vse6n7 {

    0%,
    100% {
        transform: rotate(calc(var(--rotation) * 1deg)) translateY(0);
    }

    50% {
        transform: rotate(calc(var(--rotation) * 1deg)) translateY(300%);
    }
}

@keyframes spinner-o824ag {
    to {
        transform: rotate(360deg);
    }
}
</style>

<script lang="ts">
export default {
    data() {
        return {
            isAppLoading: true,

        };
    },
    
}
</script>