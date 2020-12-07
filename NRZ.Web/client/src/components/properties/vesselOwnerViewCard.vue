<template>
  <div class="vesselOwnerView">
    <div>
      <div>
          <label class="px-3">{{ $t("propertiesList.vessels.owners.list") }}</label>
      </div>
      <v-data-table
        :headers="headers"
        :items="localItems"
        v-if="localItems && localItems.length > 0"
        :single-select="true"
        item-key="id"
        class="table-striped elevation-1"
      >

        <template v-slot:[`item.isCompany`]="{ item }">
          <span>{{ formatBoolean(item.isCompany) }}</span>
        </template>

        <template v-slot:[`item.isUser`]="{ item }">
          <span>{{ formatBoolean(item.isUser) }}</span>
        </template>
      </v-data-table>
    </div>
  </div>
</template>

<script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop } from "vue-property-decorator";

@Component({
  computed: {
  },
})
export default class VesselOwnerView extends Vue {
  @Prop({ required: true }) items: any;
  private headers: object[] = [];

  get localItems() {
    return this.items;
  }

  mounted() {
    this.headers = this.getHeaders();
  }

  formatBoolean(value: boolean) {
    if (value)
      return this.$t("common.yes");
    else return this.$t("common.no");
  }

  getHeaders() {
    return [
      {
        text: this.$t("propertiesList.vessels.owners.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("propertiesList.vessels.owners.isCompany"),
        align: "start",
        sortable: false,
        value: "isCompany",
      },
      {
        text: this.$t("propertiesList.vessels.owners.companyName"),
        align: "start",
        sortable: false,
        value: "companyName",
      },
      {
        text: this.$t("propertiesList.vessels.owners.eik"),
        align: "start",
        sortable: false,
        value: "eik",
      },
      {
        text: this.$t("propertiesList.vessels.owners.personFirstName"),
        align: "start",
        sortable: false,
        value: "personFirstName",
      },
      {
        text: this.$t("propertiesList.vessels.owners.personMiddleName"),
        align: "start",
        sortable: false,
        value: "personMiddleName",
      },
      {
        text: this.$t("propertiesList.vessels.owners.personLastName"),
        align: "start",
        sortable: false,
        value: "personLastName",
      },
      {
        text: this.$t("propertiesList.vessels.owners.egn"),
        align: "start",
        sortable: false,
        value: "egn",
      },
      {
        text: this.$t("propertiesList.vessels.owners.imoNumber"),
        align: "start",
        sortable: false,
        value: "imoNumber",
      },
      {
        text: this.$t("propertiesList.vessels.owners.isUser"),
        align: "start",
        sortable: false,
        value: "isUser",
      },
      {
        text: this.$t("propertiesList.vessels.owners.address"),
        align: "start",
        sortable: false,
        value: "address",
      },
    ];
  }
}
</script>
