<template>
    
        <transition-group name="toasts" tag="div" class="c-toasts">
            
            <div v-for="toast in $store.getters.globalNotificationList"  :key="toast.id" :class="['toasts-item','alert-success', 'c-toasts__item', 'c-toasts__item--' + toast.type]">
                <span class="c-toasts__item-text">{{ toast.message }}</span>
            </div>

            
        </transition-group>
   
    

</template>
<style>
    
    .toasts-item {
        transition: all 0.5s;
    }

    .toasts-enter,
    .toasts-leave-to {
        opacity: 0;
        transform: scale(0.9);
    }

    .toasts-leave-active {
        position: absolute;
        z-index: -1;
    }

    
    .c-toasts {
        position: fixed;
        top: 20px;
        right: 20px;
        z-index: 9999;
        width: 300px;
        pointer-events: none;
    }

   
    .c-toasts__item {
        display: flex;
        align-items: center;
        width: 100%;
        margin-bottom: 10px;
        padding: 15px;
        border-radius: 5px;
        color: #fff;
        background-color: rgba(0,0,0, 0.6);
    }

    .c-toasts__item-text {
        font-size: 14px;
        line-height: 1.5;
    }

    .c-toasts__item--success {
        color: #0f5132;
        background-color: #a4eccb;
        border-color: #badbcc;
    }

    .c-toasts__item--danger {
        background-color: #cc3300;
    }

    .c-toasts__item--warning {
        background-color: #ffcc00;
    }

    .c-toasts__item--info {
        background-color: #40a6ce;
    }
</style>
<script lang="ts">

    import { defineComponent } from "vue";


export default defineComponent({
    name: "global-notification",
    data() {
        return {
            toasts: []
        }
    },

    methods: {
        
    },

    mounted() {
        
    }
});
</script>