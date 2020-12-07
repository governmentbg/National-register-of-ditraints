<template>
  <div class="aircraftView">
    <v-row>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.id"
          :label="$t('propertiesList.aircrafts.id')"
          readonly
        ></v-text-field>
      </v-col>

      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.msnserialNumber"
          :label="$t('propertiesList.aircrafts.msnserialNumber')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.icao"
          :label="$t('propertiesList.aircrafts.icao')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.producerCountryCode"
          :label="$t('propertiesList.aircrafts.producerCountryCode')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.producerCountryName"
          :label="$t('propertiesList.aircrafts.producerCountryName')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.producerName"
          :label="$t('propertiesList.aircrafts.producerName')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.producerNameEn"
          :label="$t('propertiesList.aircrafts.producerNameEn')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.airCategoryCode"
          :label="$t('propertiesList.aircrafts.airCategoryCode')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.airCategoryName"
          :label="$t('propertiesList.aircrafts.airCategoryName')"
          readonly
        ></v-text-field>
      </v-col>

      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.modelName"
          :label="$t('propertiesList.aircrafts.modelName')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.modelNameEn"
          :label="$t('propertiesList.aircrafts.modelNameEn')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
        <aircraft-debt-view-card class="col-md-12"
          :items="localItem.debts"
        ></aircraft-debt-view-card>
    </v-row>
    <v-row>
        <aircraft-registration-view-card class="col-md-12"
          :items="localItem.registrations"
        ></aircraft-registration-view-card>
    </v-row>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop } from "vue-property-decorator";
import AircraftDebtViewCard from "@/components/properties/aircraftDebtViewCard.vue";
import AircraftRegistrationViewCard from "@/components/properties/aircraftRegistrationViewCard.vue";

@Component({
  components: {
    AircraftDebtViewCard,
    AircraftRegistrationViewCard,
  },
})
export default class AircraftViewCard extends Vue {
  @Prop({ required: true }) item: any;

  get localItem() {
    return this.item || { id: 0 };
  }
}
</script>