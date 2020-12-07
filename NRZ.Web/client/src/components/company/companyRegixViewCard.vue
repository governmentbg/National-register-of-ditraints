<template>
  <div class="RegixCompanyView">
    <div>
      <v-row>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.name"
            :label="$t('regixCompany.name')"
            readonly
          ></v-text-field>
        </v-col>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.legalFormAbbr"
            :label="$t('regixCompany.legalFormAbbr')"
            readonly
          ></v-text-field>
        </v-col>
        <v-col cols="12" sm="6">
          <v-text-field
            v-model="localItem.legalFormName"
            :label="$t('regixCompany.legalFormName')"
            readonly
          ></v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.uic"
            :label="$t('regixCompany.uic')"
            readonly
          ></v-text-field>
        </v-col>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.statusName"
            :label="$t('regixCompany.statusName')"
            readonly
          ></v-text-field>
        </v-col>
      </v-row>
    </div>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop } from "vue-property-decorator";
import { RegixCompanyModel } from "@/models/company";

@Component({
  components: {
  },
})
export default class RegixCompanyViewCard extends Vue {
  @Prop({ required: true }) item: any;

  constructor() {
    super();
  }

  get localItem() {
    return this.item;
  }

}
</script>