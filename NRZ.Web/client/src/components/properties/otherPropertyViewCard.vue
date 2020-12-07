<template>
  <div class="otherPropertyView">
    <v-row>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.id"
          :label="$t('propertiesList.otherProperties.id')"
          readonly
        ></v-text-field>
      </v-col>

      <v-col cols="8" sm="4">
        <v-text-field
          v-model="localItem.identifier"
          :label="$t('propertiesList.otherProperties.identifier')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12" sm="6">
        <v-text-field
          v-model="localItem.description"
          :label="$t('propertiesList.otherProperties.description')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop } from "vue-property-decorator";

@Component({})
export default class OtherPropertyViewCard extends Vue {
  @Prop({ required: true }) item: any;


  get localItem() {
    return this.item || {id: 0};
  }
}
</script>