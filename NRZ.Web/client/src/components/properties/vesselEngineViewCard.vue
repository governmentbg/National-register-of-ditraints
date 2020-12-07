<template>
  <div class="vesselEngineView">
    <div>
      <div>
          <label class="px-3">{{ $t("propertiesList.vessels.engines.list") }}</label>
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
  computed: {
  },
})
export default class VesselEngineView extends Vue {
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
        text: this.$t("propertiesList.vessels.engines.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("propertiesList.vessels.engines.systemModification"),
        align: "start",
        sortable: false,
        value: "systemModification",
      },
      {
        text: this.$t("propertiesList.vessels.engines.engineNumber"),
        align: "start",
        sortable: false,
        value: "engineNumber",
      },
      {
        text: this.$t("propertiesList.vessels.engines.power"),
        align: "start",
        sortable: false,
        value: "power",
      },
      {
        text: this.$t("propertiesList.vessels.engines.type"),
        align: "start",
        sortable: false,
        value: "type",
      },
    ];
  }
}
</script>
