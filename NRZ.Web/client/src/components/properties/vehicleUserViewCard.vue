<template>
  <div class="vehicleUserView">
    <div>
      <div>
          <label class="px-3">{{ $t("propertiesList.vehicles.users.tableTitle") }}</label>
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
export default class VehicleUserView extends Vue {
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
        text: this.$t("propertiesList.vehicles.users.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("propertiesList.vehicles.users.bulgarianCitizenPin"),
        align: "start",
        sortable: false,
        value: "bulgarianCitizenPin",
      },
      {
        text: this.$t("propertiesList.vehicles.users.bulgarianCitizenFirstName"),
        align: "start",
        sortable: false,
        value: "bulgarianCitizenFirstName",
      },
      {
        text: this.$t("propertiesList.vehicles.users.bulgarianCitizenMiddleName"),
        align: "start",
        sortable: false,
        value: "bulgarianCitizenMiddleName",
      },
      {
        text: this.$t("propertiesList.vehicles.users.bulgarianCitizenLastName"),
        align: "start",
        sortable: false,
        value: "bulgarianCitizenLastName",
      },
      {
        text: this.$t("propertiesList.vehicles.users.foreignCitizenPin"),
        align: "start",
        sortable: false,
        value: "foreignCitizenPin",
      },
      {
        text: this.$t("propertiesList.vehicles.users.foreignCitizenPn"),
        align: "start",
        sortable: false,
        value: "foreignCitizenPn",
      },
      {
        text: this.$t("propertiesList.vehicles.users.foreignCitizenNamesCyrillic"),
        align: "start",
        sortable: false,
        value: "foreignCitizenNamesCyrillic",
      },
      {
        text: this.$t("propertiesList.vehicles.users.foreignCitizenNamesLatin"),
        align: "start",
        sortable: false,
        value: "foreignCitizenNamesLatin",
      },
      {
        text: this.$t("propertiesList.vehicles.users.foreignCitizenNationality"),
        align: "start",
        sortable: false,
        value: "foreignCitizenNationality",
      },
      {
        text: this.$t("propertiesList.vehicles.users.companyId"),
        align: "start",
        sortable: false,
        value: "companyId",
      },
      {
        text: this.$t("propertiesList.vehicles.users.companyName"),
        align: "start",
        sortable: false,
        value: "companyName",
      },
      {
        text: this.$t("propertiesList.vehicles.users.companyNameLatin"),
        align: "start",
        sortable: false,
        value: "companyNameLatin",
      },
    ];
  }
}
</script>
