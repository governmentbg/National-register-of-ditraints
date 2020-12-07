<template>
  <div>
    <ValidationObserver v-slot="{ invalid }">
      <v-row>
        <v-col md="12">
          <div class="text-h5">{{ $t('auctions.announcements.propertySearch.search') }}</div>
        </v-col>
        <v-col md="4">
          <v-select
            :items="propertyTypeItems"
            item-text="name"
            item-value="code"
            :label="$t('createDistraint.propertyTypeLabel')"
            v-model="propertyTypeSelected"
            return-object
          >
          </v-select>
        </v-col>

        <v-col md="4">
          <v-text-field
            v-model="suitNumber"
            :label="$t('createDistraint.suitNumber')"
          ></v-text-field>
        </v-col>

        <v-col md="4">
          <ValidationProvider
            ref="observer"
            v-slot="{ errors }"
            :name="$t('createDistraint.searchHint')"
            rules="min:3"
          >
            <v-text-field
              v-model="propertyIdentifier"
              append-icon="search"
              :label="$t('createDistraint.searchIdentifier')"
              :error-messages="errors"
              :hint="$t('createDistraint.searchMinLength')"
              single-line
            ></v-text-field>
          </ValidationProvider>
        </v-col>
        <v-col md="12">
          <div class="w-100 text-center">
            <v-btn
              @click="submitPropertySearch"
              :color="'primary'"
              :disabled="
                !propertyIdentifier ||
                invalid ||
                !propertyTypeSelected.code ||
                !suitNumber
              "
              >{{ $t("createDistraint.btnSearch") }}</v-btn
            >
          </div>
        </v-col>

        <v-col md="12" v-if="searchPropertiesDBResult">
          <label class="v-label">
            {{ searchPropertiesDBResult }}
          </label>
        </v-col>
        <v-col md="12" v-if="searchPropertiesAdminResult">
          <label class="v-label">
            {{ searchPropertiesAdminResult }}
          </label>
        </v-col>
        <v-col md="12">
          <properties-list
            :propertyType="propertyTypeSelected.code"
            :items="propertyItems"
            :singleSelect="!multiple"
            v-on:selected-property-changed="selectedPropertyChanged($event)"
          ></properties-list>
          <div class="v-messages theme--light error--text" role="alert"> <!--todo: enable validation-->
            <div class="v-messages__wrapper">
              <div class="v-messages__message">{{ errorMessages[0] }}</div>
            </div>
          </div>
        </v-col>
      </v-row>
    </ValidationObserver>
  </div>
</template>

<script lang="ts">
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { Vue, Component, Watch, Prop } from "vue-property-decorator";
import { PropertyTypeModel } from "@/models/nomenclature";

import {
  DistraintModel,
} from "@/models/distraint";
import {
  PropertyType,
} from "@/models/enums";
import {
  OtherPropertyModel,
  PropertySearchRequestModel,
  VehicleViewModel,
  AircraftViewModel,
  PropertyItemModel
} from "@/models/property";
import nomenclatureService from "@/services/nomenclature.service";
import propertyService from "@/services/property.service";
import propertyHelper from "@/helpers/property.helper";
import PropertiesList from "@/components/properties/propertiesList.vue";
import { namespace } from "vuex-class";
import VueI18n from "vue-i18n";

import { mapGetters } from "vuex";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
    PropertiesList,
  },
  computed: {
    ...mapGetters({
      dateFormat: "dateFormat",
      dateTimeFormat: "dateTimeFormat",
    }),
  },
})
export default class SearchProperty extends Vue {
  @Prop({ default: [] })
  value!: object[];

  @Prop({ default: false })
  multiple!: boolean;

 @Prop({ default: "" })
  errorMessages!: string[];

  private propertyTypeItems: PropertyTypeModel[] = [];
  private propertyTypeSelected: PropertyTypeModel = {
    code: "",
    name: "",
    nameEn: "",
    deactivated: false,
  };
  private propertyIdentifier = "";
  private propertyItems: object[] = [];
  private propertyItemData: PropertyItemModel[] = [];
  private searchPropertiesDBResult: VueI18n.TranslateResult = "";
  private searchPropertiesAdminResult: VueI18n.TranslateResult = "";

  private distraintItemData = new DistraintModel();
  private searchByOwner = false;
  private displayErrorMessage = false;

  get isOtherProperty() {
    return this.propertyTypeSelected.code === PropertyType.Other;
  }

  get isVehicle() {
    return this.propertyTypeSelected.code === PropertyType.Vehicle;
  }

  get isAircraft() {
    return this.propertyTypeSelected.code === PropertyType.Aircraft;
  }

  get suitNumber() {
    return this.distraintItemData.suitNumber;
  }

  set suitNumber(newValue: string) {
    this.distraintItemData.suitNumber = newValue;
  }

  async submitPropertySearch() {
    this.propertyItems = [];
    this.propertyItemData = [];
    this.searchPropertiesDBResult = "";
    this.searchPropertiesAdminResult = "";
    const typeCode = propertyHelper.getIdentifierTypeCode(
      this.propertyTypeSelected.code,
      this.searchByOwner
    );

    const request = new PropertySearchRequestModel({
      identifierTypeCode: typeCode,
      identifier: this.propertyIdentifier,
      suitNumber: this.suitNumber,
    });

    if (this.propertyIdentifier && this.propertyTypeSelected.code) {
      await this.searchForPropertyInDB(request);
    }
  }

  async searchForPropertyInDB(request: PropertySearchRequestModel) {
    try {
      this.searchPropertiesDBResult = this.$t(
        "createDistraint.searchingPropertiesInDB"
      );

      const result = await propertyService.searchForPropertyInDB(
        this.propertyTypeSelected.code,
        request
      );

      this.getPropertyItemsFromResult(result);

      if (this.propertyItems.length == 0) {
        this.searchPropertiesDBResult = this.$t(
          "createDistraint.noPropertiesFoundInDB"
        );
      } else {
        this.searchPropertiesDBResult = this.$t(
          "createDistraint.propertiesFoundInDB"
        );
      }
    } catch (error) {
      console.log("Error on searching properties in DB: " + error);
    }
  }

  getPropertyItemsFromResult(result: any) {
    if (this.propertyTypeSelected.code === PropertyType.Other)
      this.propertyItems = result as OtherPropertyModel[];
    else if (this.propertyTypeSelected.code === PropertyType.Vehicle)
      this.propertyItems = result as VehicleViewModel[];
    else if (this.propertyTypeSelected.code === PropertyType.Aircraft)
      this.propertyItems = result as AircraftViewModel[];
    else this.propertyItems = result as object[];
  }

  selectedPropertyChanged(newVal: any) {
    console.log("selected: ", newVal);

    this.propertyItemData = newVal;
    this.propertyItemData.forEach(item => item.propertyTypeCode = this.propertyTypeSelected.code );
    this.$emit('input', this.propertyItemData);
  }

  mounted() {
    this.fetchPropertyTypes();
  }

  @Watch("propertyTypeSelected")
  propertyTypeChanged() {
    this.propertyItems = [];
    this.propertyItemData = [];
  }

  async fetchPropertyTypes() {
    try {
      const result = await nomenclatureService.getPropertyTypesForDistraints() as PropertyTypeModel[];
      this.propertyTypeItems = result;
    } catch (error) {
      console.log("Error on getting property types: " + error);
    }
  }
}
</script>

<style>
</style>