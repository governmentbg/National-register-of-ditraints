<template>
  <div class="agriculturalMachineView">
    <v-row>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.id"
          :label="$t('agriculturalMachinery.id')"
          readonly
        ></v-text-field>
      </v-col>

      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.registrationNumber"
          :label="$t('agriculturalMachinery.registrationNumber')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.frameNumber"
          :label="$t('agriculturalMachinery.frameNumber')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.type"
          :label="$t('agriculturalMachinery.type')"
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
export default class AgriculturalMachineViewCard extends Vue {
  @Prop({ required: true }) item: any;

  get localItem() {
    return this.item || { id: 0 };
  }
}
</script>