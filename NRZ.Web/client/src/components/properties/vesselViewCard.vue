<template>
  <div class="vesselView">
    <v-row>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.id"
          :label="$t('propertiesList.vessels.id')"
          readonly
        ></v-text-field>
      </v-col>

      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItemRegistrationData.shipName"
          :label="$t('propertiesList.vessels.registrationData.shipName')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItemRegistrationData.shipNameLatin"
          :label="$t('propertiesList.vessels.registrationData.shipNameLatin')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItemRegistrationData.registrationPort"
          :label="
            $t('propertiesList.vessels.registrationData.registrationPort')
          "
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItemRegistrationData.registrationNumber"
          :label="
            $t('propertiesList.vessels.registrationData.registrationNumber')
          "
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItemRegistrationData.tom"
          :label="$t('propertiesList.vessels.registrationData.tom')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItemRegistrationData.page"
          :label="$t('propertiesList.vessels.registrationData.page')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItemRegistrationData.type"
          :label="$t('propertiesList.vessels.registrationData.type')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItemRegistrationData.statusName"
          :label="$t('propertiesList.vessels.registrationData.status')"
          readonly
        ></v-text-field>
      </v-col>

      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.bt"
          :label="$t('propertiesList.vessels.bt')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.nt"
          :label="$t('propertiesList.vessels.nt')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.maxLength"
          :label="$t('propertiesList.vessels.maxLength')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.lengthBetweenPerpendiculars"
          :label="$t('propertiesList.vessels.lengthBetweenPerpendiculars')"
          readonly
        ></v-text-field>
      </v-col>

      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.maxWidth"
          :label="$t('propertiesList.vessels.maxWidth')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.waterplane"
          :label="$t('propertiesList.vessels.waterplane')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.shipboardHeight"
          :label="$t('propertiesList.vessels.shipboardHeight')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.deadweight"
          :label="$t('propertiesList.vessels.deadweight')"
          readonly
        ></v-text-field>
      </v-col>

      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.numberOfEngines"
          :label="$t('propertiesList.vessels.numberOfEngines')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.enginesFuel"
          :label="$t('propertiesList.vessels.enginesFuel')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.sumEnginePower"
          :label="$t('propertiesList.vessels.sumEnginePower')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="6" sm="3">
        <v-text-field
          v-model="localItem.bodyNumber"
          :label="$t('propertiesList.vessels.bodyNumber')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <vessel-engine-view-card
        class="col-md-12"
        :items="localItem.engines"
      ></vessel-engine-view-card>
    </v-row>
    <v-row>
      <vessel-owner-view-card
        class="col-md-12"
        :items="localItem.owners"
      ></vessel-owner-view-card>
    </v-row>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop } from "vue-property-decorator";
import VesselEngineViewCard from "@/components/properties/vesselEngineViewCard.vue";
import VesselOwnerViewCard from "@/components/properties/vesselOwnerViewCard.vue";

@Component({
  components: {
    VesselEngineViewCard,
    VesselOwnerViewCard,
  },
})
export default class VesselViewCard extends Vue {
  @Prop({ required: true }) item: any;

  get localItem() {
    return this.item || { id: 0 };
  }

  get localItemRegistrationData() {
    return this.item.registrationData || {};
  }
}
</script>