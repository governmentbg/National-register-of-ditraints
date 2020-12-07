<template>
  <div class="propertiesList">
    <div>
      <v-data-table
        :headers="headers"
        :items="localItems"
        class="table-striped elevation-1"
        v-if="localItems && localItems.length > 0"
        :single-select="singleSelect"
        item-key="id"
        :show-select="selectable"
        v-model="selected"
      >
      </v-data-table>
    </div>
  </div>
</template>

<script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop, Watch } from "vue-property-decorator";
import { PropertyType } from "@/models/enums";

@Component
export default class PropertiesList extends Vue {
  @Prop({ required: true }) propertyType!: string;
  @Prop({ required: true }) items: any;
  @Prop({ default: true }) singleSelect!: boolean;
  @Prop({ default: true }) selectable!: boolean;
  @Prop({ default: false }) manuallyAdded!: boolean;
  private selected: any[] = [];

  get localItems() {
    return this.items;
  }

  get headers() {
    if (this.manuallyAdded) {
      // Ръчно добавен без търсене в НРЗ
      return this.getHeadersForOtherProperty();
    }

    if (this.propertyType === PropertyType.Other)
      return this.getHeadersForOtherProperty();
    else if (this.propertyType === PropertyType.Vehicle)
      return this.getHeadersForVehicle();
    else if (this.propertyType === PropertyType.Aircraft)
      return this.getHeadersForAircraft();
    else if (this.propertyType === PropertyType.Vessel)
      return this.getHeadersForVessel();
    else if (this.propertyType === PropertyType.AgriForMachinery)
      return this.getHeadersForAgriculturalMachinery();
    else return [];
  }

  @Watch("items")
  itemsChanged() {
    if (this.selected && this.selected.length > 0) this.selected = [];
  }

  @Watch("selected")
  selectedChanged() {
    this.$emit("selected-property-changed", this.selected);
  }

  getHeadersForOtherProperty() {
    return [
      {
        text: this.$t("propertiesList.otherProperties.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("propertiesList.otherProperties.identifier"),
        align: "start",
        sortable: false,
        value: "identifier",
      },
      {
        text: this.$t("propertiesList.otherProperties.description"),
        align: "start",
        sortable: false,
        value: "description",
      },
    ];
  }

  getHeadersForVehicle() {
    return [
      {
        text: this.$t("propertiesList.vehicles.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("propertiesList.vehicles.registrationNumber"),
        align: "start",
        sortable: false,
        value: "registrationNumber",
      },
      {
        text: this.$t("propertiesList.vehicles.vehicleType"),
        align: "start",
        sortable: false,
        value: "vehicleType",
      },
      {
        text: this.$t("propertiesList.vehicles.model"),
        align: "start",
        sortable: false,
        value: "model",
      },
      {
        text: this.$t("propertiesList.vehicles.tradeDescription"),
        align: "start",
        sortable: false,
        value: "tradeDescription",
      },
      {
        text: this.$t("propertiesList.vehicles.color"),
        align: "start",
        sortable: false,
        value: "color",
      },
      {
        text: this.$t("propertiesList.vehicles.category"),
        align: "start",
        sortable: false,
        value: "category",
      },
    ];
  }

  getHeadersForAircraft() {
    return [
      {
        text: this.$t("propertiesList.aircrafts.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("propertiesList.aircrafts.msnserialNumber"),
        align: "start",
        sortable: false,
        value: "msnserialNumber",
      },
      {
        text: this.$t("propertiesList.aircrafts.producerName"),
        align: "start",
        sortable: false,
        value: "producerName",
      },
      {
        text: this.$t("propertiesList.aircrafts.producerCountryCode"),
        align: "start",
        sortable: false,
        value: "producerCountryCode",
      },
      {
        text: this.$t("propertiesList.aircrafts.producerCountryName"),
        align: "start",
        sortable: false,
        value: "producerCountryName",
      },
      {
        text: this.$t("propertiesList.aircrafts.airCategoryCode"),
        align: "start",
        sortable: false,
        value: "airCategoryCode",
      },
      {
        text: this.$t("propertiesList.aircrafts.airCategoryName"),
        align: "start",
        sortable: false,
        value: "airCategoryName",
      },
      {
        text: this.$t("propertiesList.aircrafts.icao"),
        align: "start",
        sortable: false,
        value: "icao",
      },
      {
        text: this.$t("propertiesList.aircrafts.modelName"),
        align: "start",
        sortable: false,
        value: "modelName",
      },
    ];
  }

  getHeadersForVessel() {
    return [
      {
        text: this.$t("propertiesList.vessels.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("propertiesList.vessels.registrationData.type"),
        align: "start",
        sortable: false,
        value: "registrationData.type",
      },
      {
        text: this.$t("propertiesList.vessels.registrationData.shipName"),
        align: "start",
        sortable: false,
        value: "registrationData.shipName",
      },
      {
        text: this.$t(
          "propertiesList.vessels.registrationData.registrationNumber"
        ),
        align: "start",
        sortable: false,
        value: "registrationData.registrationNumber",
      },
      {
        text: this.$t(
          "propertiesList.vessels.registrationData.registrationPort"
        ),
        align: "start",
        sortable: false,
        value: "registrationData.registrationPort",
      },
      {
        text: this.$t("propertiesList.vessels.registrationData.status"),
        align: "start",
        sortable: false,
        value: "registrationData.statusName",
      },
      {
        text: this.$t("propertiesList.vessels.owners.companyName"),
        align: "start",
        sortable: false,
        value: "owners[0].companyName",
      },
      {
        text: this.$t("propertiesList.vessels.owners.eik"),
        align: "start",
        sortable: false,
        value: "owners[0].eik",
      },
      {
        text: this.$t("propertiesList.vessels.owners.personFirstName"),
        align: "start",
        sortable: false,
        value: "owners[0].personFirstName",
      },
      {
        text: this.$t("propertiesList.vessels.owners.personMiddleName"),
        align: "start",
        sortable: false,
        value: "owners[0].personMiddleName",
      },
      {
        text: this.$t("propertiesList.vessels.owners.personLastName"),
        align: "start",
        sortable: false,
        value: "owners[0].personLastName",
      },
      {
        text: this.$t("propertiesList.vessels.owners.egn"),
        align: "start",
        sortable: false,
        value: "owners[0].egn",
      },
    ];
  }

  getHeadersForAgriculturalMachinery() {
    return [
      {
        text: this.$t("agriculturalMachinery.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("agriculturalMachinery.registrationNumber"),
        align: "start",
        sortable: false,
        value: "registrationNumber",
      },
      {
        text: this.$t("agriculturalMachinery.frameNumber"),
        align: "start",
        sortable: false,
        value: "frameNumber",
      },
      {
        text: this.$t("agriculturalMachinery.type"),
        align: "start",
        sortable: false,
        value: "type",
      },
    ];
  }
}
</script>
