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
  private selected: any[] = [];

  get localItems() {
    return this.items;
  }

  get headers() {
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
        value: "title",
      },
      {
        text: this.$t("propertiesList.otherProperties.description"),
        align: "start",
        sortable: false,
        value: "description",
      },
    ];
  }

  @Watch("items")
  itemsChanged() {
    if (this.selected && this.selected.length > 0) this.selected = [];
  }

  @Watch("selected")
  selectedChanged() {
    this.$emit("selected-property-changed", this.selected);
  }
}
</script>
