<template>
    <div class="list-group">
        <router-link v-for="sport in sports"
                     :key="sport.id" class="list-group-item list-group-item-action"
                     :to="'/day/' + $route.params.day + '/sport/' + sport.id">
            <i :class="sport.icon"></i>&nbsp;<span>{{ sport.name }}</span>
        </router-link>
    </div>
</template>
<style>
   
</style>
<script lang="ts">

    import { defineComponent } from "vue";
    import DataService from "@/services/DataService";
    import Sport from "@/types/Sport";
    import ResponseData from "@/types/ResponseData";
    
export default defineComponent({
    name: "sports-list",
    components: {
        
    },
  data() {
    return {
      sports: [] as Sport[],
      currentSport: {} as Sport,
      currentIndex: -1,
      name: "",
    };
    },
    
  methods: {
    retrieveSportsList() {
      DataService.getAllSports()
        .then((response: ResponseData) => {
            this.sports = response.data;
            this.$store.dispatch('updateSportsList', this.sports);


          console.log(response.data);
        })
        .catch((e: Error) => {
          console.log(e);
        });
      },

      setActiveSport(sportId: string) {
          this.$store.dispatch('setActiveSport', sportId);
      },
        refreshList() {
            this.retrieveSportsList();
      
        },
        
    },
    watch: {
        '$route'() {
            if (this.$route.params.id) {
                this.setActiveSport(this.$route.params.id as string)
            } else {
                this.setActiveSport('0') // reset the sport
                this.refreshList()
            }
            console.log(this.$route.params.id);
        }
    },

  mounted() {
      this.retrieveSportsList();
      
    }
});
</script>