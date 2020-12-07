<template>
  <div>
    <v-row>
      <v-col md="12">
        <div class="text-h5">{{ $t('auctions.announcements.propertySearch.title') }}</div>
      </v-col>
      <v-col md="12" v-show="selectedItems.length">
        <properties-list
          :propertyType="propertyType.Other"
          :items="selectedItems"
          :selectable="true"
          :singleSelect="false"
          v-on:selected-property-changed="onAddedItemsSelect($event)"
        ></properties-list>
      </v-col>
      <v-col md="12">
        <v-btn
          outlined
          color="error"
          v-if="addedItemsSelected.length"
          @click="removeFromSelected"
          >{{ $t('auctions.announcements.propertySearch.removeChosen') }}</v-btn
        >
      </v-col>
      <v-col md="12">
        <v-alert
          outlined
          type="warning"
          border="left"
          v-if="!selectedItems.length"
          >{{
            $t('auctions.announcements.propertySearch.noAssets')
          }}</v-alert
        >
      </v-col>
    </v-row>
    <v-row>
      <v-col md="12">
        <property-search
          v-model="propertyItemData"
          :multiple="true"
        ></property-search>
      </v-col>
      <v-col md="12">
        <v-btn
          outlined
          color="success"
          v-if="propertyItemData.length"
          @click="addToSelected"
          >{{ $t('auctions.announcements.propertySearch.addChosen') }}</v-btn
        >
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { Vue, Component, Prop } from "vue-property-decorator";

import { PropertyType } from "@/models/enums";
import PropertiesList from "@/components/properties/propertiesList.vue";

import { mapGetters } from "vuex";
import PropertySearch from "@/components/properties/searchProperty.vue";
import { AuctionItem } from "@/models/autction.models";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
    PropertiesList,
    PropertySearch,
  },
  computed: {
    ...mapGetters({
      dateFormat: "dateFormat",
      dateTimeFormat: "dateTimeFormat",
    }),
    propertyType: () => PropertyType
  },
})
export default class AnnouncementPropertySearch extends Vue {
  @Prop({ default: [] })
  value!: object[];

  private propertyItemData: object[] = [];
  private selectedItems: object[] = [];
  private addedItemsSelected: object[] = [];

  selectedPropertyChanged(newVal: any) {
    this.propertyItemData = newVal;
  }

  onAddedItemsSelect(newVal: object[]) {
    this.addedItemsSelected = newVal;
  }

  addToSelected() {
    this.propertyItemData.forEach((x: any) => {
      const added = this.selectedItems.some((i: any) => {
        return i.id === x.id;
      });

      if (!added) {
        const item = {
          ...x,
          title: x.title || x.identifier
        }
        this.selectedItems.push(item);
      }
    });
    this.$emit("input", this.selectedItems);
  }

  pushToSelected(item: AuctionItem){
    if (item) {
      this.selectedItems.push(item);
    }
  }

  removeFromSelected() {
    this.selectedItems = this.selectedItems.filter((x: any) => {
      return !this.addedItemsSelected.some((i: any) => i.id === x.id);
    });

    this.$emit("input", this.selectedItems);
  }
}
</script>

<style>
</style>