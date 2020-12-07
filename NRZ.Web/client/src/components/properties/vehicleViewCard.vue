<template>
  <div class="vehicleView">
    <v-row>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.id"
          :label="$t('propertiesList.vehicles.id')"
          readonly
        ></v-text-field>
      </v-col>

      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.registrationNumber"
          :label="$t('propertiesList.vehicles.registrationNumber')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="formattedFirstRegistrationDate"
          :label="$t('propertiesList.vehicles.firstRegistrationDate')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.vin"
          :label="$t('propertiesList.vehicles.vin')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.engineNumber"
          :label="$t('propertiesList.vehicles.engineNumber')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.vehicleType"
          :label="$t('propertiesList.vehicles.vehicleType')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.model"
          :label="$t('propertiesList.vehicles.model')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.typeApprovalNumber"
          :label="$t('propertiesList.vehicles.typeApprovalNumber')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.approvalType"
          :label="$t('propertiesList.vehicles.approvalType')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.tradeDescription"
          :label="$t('propertiesList.vehicles.tradeDescription')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.color"
          :label="$t('propertiesList.vehicles.color')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.category"
          :label="$t('propertiesList.vehicles.category')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.offRoadSymbols"
          :label="$t('propertiesList.vehicles.offRoadSymbols')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.massG"
          :label="$t('propertiesList.vehicles.massG')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.massF1"
          :label="$t('propertiesList.vehicles.massF1')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.massF2"
          :label="$t('propertiesList.vehicles.massF2')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.massF3"
          :label="$t('propertiesList.vehicles.massF3')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.vehicleNumOfAxles"
          :label="$t('propertiesList.vehicles.vehicleNumOfAxles')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.vehicleMassO1"
          :label="$t('propertiesList.vehicles.vehicleMassO1')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.vehicleMassO2"
          :label="$t('propertiesList.vehicles.vehicleMassO2')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.capacity"
          :label="$t('propertiesList.vehicles.capacity')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.maxPower"
          :label="$t('propertiesList.vehicles.maxPower')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.fuel"
          :label="$t('propertiesList.vehicles.fuel')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.environmentalCategory"
          :label="$t('propertiesList.vehicles.environmentalCategory')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.vehicleDocumentNumber"
          :label="$t('propertiesList.vehicles.vehicleDocumentNumber')"
          readonly
        ></v-text-field>
      </v-col>
      <v-col cols="4" sm="2">
        <v-text-field
          v-model="localItem.vehicleDocumentDate"
          :label="$t('propertiesList.vehicles.vehicleDocumentDate')"
          readonly
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row >
      <vehicle-owner-view-card :items="localItem.owners" class="col-md-12"></vehicle-owner-view-card>
    </v-row>
    <v-row >
      <vehicle-user-view-card :items="localItem.users" class="col-md-12"></vehicle-user-view-card>
    </v-row>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop } from "vue-property-decorator";
import moment from "moment";
import { mapGetters } from "vuex";
import VehicleOwnerViewCard from "@/components/properties/vehicleOwnerViewCard.vue";
import VehicleUserViewCard from "@/components/properties/vehicleUserViewCard.vue";

@Component({
  components: {
    VehicleOwnerViewCard,
    VehicleUserViewCard,
  },
  computed: {
    ...mapGetters({
      dateFormat: "dateFormat",
      dateTimeFormat: "dateTimeFormat",
    }),
  },
})
export default class VehicleViewCard extends Vue {
  @Prop({ required: true }) item: any;
  dateFormat: any;
  dateTimeFormat: any;

  get localItem() {
    return this.item || { id: 0 };
  }

  get formattedFirstRegistrationDate() {
    return this.formatDate(this.localItem.firstRegistrationDate);
  }

  formatDate(value: Date) {
    if (value != null)
      return moment(value.toLocaleString()).format(this.dateFormat);
    else return value;
  }

  formatDateTime(value: Date) {
    if (value != null)
      return moment(value.toLocaleString()).format(this.dateTimeFormat);
    else return value;
  }
}
</script>