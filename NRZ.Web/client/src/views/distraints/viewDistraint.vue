<template>
  <div class="distraint">
    <div>
      <v-btn @click="backToDistraintsList">
        <v-icon small class="mr-2"> mdi-arrow-left</v-icon>
        {{ $t("buttons.back") }}
      </v-btn>
    </div>
    <div class="mb-3 text-center">
      <h3>{{ $t("viewDistraint.title") }}</h3>
    </div>
    <div>
      <distraint-view-card :id="currentId"></distraint-view-card>
    </div>
    <div>
      <v-row>
        <v-col cols="6" sm="3">
          <v-btn @click="backToDistraintsList">
            <v-icon small class="mr-2"> mdi-arrow-left</v-icon>
            {{ $t("buttons.back") }}
          </v-btn>
        </v-col>
      </v-row>
    </div>
  </div>
</template>



 <script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop } from "vue-property-decorator";
import DistraintViewCard from "@/components/distraints/distraintViewCard.vue";

@Component({
  components: {
    DistraintViewCard,
  },
  computed: {},
})
export default class ViewDistraint extends Vue {
  @Prop({ required: true }) id: any;

  get currentId() {
    return this.id;
  }

  backToDistraintsList() {
    this.$router.push({ path: "/distraints/list" });
  }
}
</script>