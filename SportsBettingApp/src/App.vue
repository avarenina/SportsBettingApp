<template>
    
    <div class="main-layout">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
            <div class="container">
                <a class="navbar-brand" href="">Sports Betting App</a>
                <div class="collapse navbar-collapse">
                    <ul class="navbar-nav mb-lg-0">
                        <li class="nav-item">
                            <router-link class="nav-link" to="/">Bet</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link class="nav-link" to="/wallet">Wallet</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link class="nav-link" to="/about">About</router-link>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        <div v-if="isAppLoading" class="loader-container">
            <div class="spinner">
                <div></div>
                <div></div>
                <div></div>
                <div></div>
            </div>
        </div>
        <router-view v-else  />



    </div>





</template>

<style>
    .loader-container {
        width: 100%;
        height:100%;
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
        0%, 100% {
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

<script>

export default {
        data() {
            return {
                isAppLoading: false,
                
            };
        },
}
</script>