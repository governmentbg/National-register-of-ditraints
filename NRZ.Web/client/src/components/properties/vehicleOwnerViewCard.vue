<template>
  <div class="vehicleOwnerView">
    <div>
      <div>
          <label class="px-3">{{ $t("propertiesList.vehicles.owners.tableTitle") }}</label>
      </div>
      <v-data-table
        :headers="headers"
        :items="localItems"
        v-if="localItems && localItems.length > 0"
        :single-select="true"
        item-key="id"
        class="table-striped elevation-1"
      >
      </v-data-table>
    </div>
  </div>
</template>

<script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop } from "vue-property-decorator";

@Component({
  
})
export default class VehicleOwnerView extends Vue {
  @Prop({ required: true }) items: any;
  private headers: object[] = [];

  get localItems() {
    return this.items;
  }

  mounted() {
    this.headers = this.getHeaders();
  }


  getHeaders() {
    return [
      {
        text: this.$t("propertiesList.vehicles.owners.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("propertiesList.vehicles.owners.bulgarianCitizenPin"),
        align: "start",
        sortable: false,
        value: "bulgarianCitizenPin",
      },
      {
        text: this.$t("propertiesList.vehicles.owners.bulgarianCitizenFirstName"),
        align: "start",
        sortable: false,
        value: "bulgarianCitizenFirstName",
      },
      {
        text: this.$t("propertiesList.vehicles.owners.bulgarianCitizenMiddleName"),
        align: "start",
        sortable: false,
        value: "bulgarianCitizenMiddleName",
      },
      {
        text: this.$t("propertiesList.vehicles.owners.bulgarianCitizenLastName"),
        align: "start",
        sortable: false,
        value: "bulgarianCitizenLastName",
      },
      {
        text: this.$t("propertiesList.vehicles.owners.foreignCitizenPin"),
        align: "start",
        sortable: false,
        value: "foreignCitizenPin",
      },
      {
        text: this.$t("propertiesList.vehicles.owners.foreignCitizenPn"),
        align: "start",
        sortable: false,
        value: "foreignCitizenPn",
      },
      {
        text: this.$t("propertiesList.vehicles.owners.foreignCitizenNamesCyrillic"),
        align: "start",
        sortable: false,
        value: "foreignCitizenNamesCyrillic",
      },
      {
        text: this.$t("propertiesList.vehicles.owners.foreignCitizenNamesLatin"),
        align: "start",
        sortable: false,
        value: "foreignCitizenNamesLatin",
      },
      {
        text: this.$t("propertiesList.vehicles.owners.foreignCitizenNationality"),
        align: "start",
        sortable: false,
        value: "foreignCitizenNationality",
      },
      {
        text: this.$t("propertiesList.vehicles.owners.companyId"),
        align: "start",
        sortable: false,
        value: "companyId",
      },
      {
        text: this.$t("propertiesList.vehicles.owners.companyName"),
        align: "start",
        sortable: false,
        value: "companyName",
      },
      {
        text: this.$t("propertiesList.vehicles.owners.companyNameLatin"),
        align: "start",
        sortable: false,
        value: "companyNameLatin",
      },
    ];
  }
}
</script>
