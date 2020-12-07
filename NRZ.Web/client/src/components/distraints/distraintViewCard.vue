<template>
  <div class="distraintView">
    <div>
      <error-message
        :show="showError"
        :messages="errorMessages"
      ></error-message>
    </div>
    <div>
      <v-row>
        <v-expansion-panels v-model="panel" multiple>
          <v-expansion-panel aria-expanded="true">
            <v-expansion-panel-header>{{
              $t("createDistraint.commonData")
            }}</v-expansion-panel-header>
            <v-expansion-panel-content>
              <v-row>
                <v-col cols="6" sm="3">
                  <v-text-field
                    v-model="item.suitNumber"
                    :label="$t('distraints.list.suitNumber')"
                    readonly
                  ></v-text-field>
                </v-col>
                <v-col cols="6" sm="3">
                  <v-text-field
                    v-model="item.createdByUserType"
                    :label="$t('distraints.list.createdByUserType')"
                    readonly
                  ></v-text-field>
                </v-col>
                <v-col cols="6" sm="3">
                  <v-text-field
                    v-model="item.createdByUserName"
                    :label="$t('distraints.list.createdByUserName')"
                    readonly
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="item.location"
                    :label="$t('distraints.list.location')"
                    readonly
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col cols="6" sm="3">
                  <v-text-field
                    v-model="formattedEnforcementDate"
                    :label="$t('distraints.list.enforcementDate')"
                    readonly
                  ></v-text-field>
                </v-col>
                <v-col cols="6" sm="3">
                  <v-text-field
                    v-model="formattedEnforcedAt"
                    :label="$t('distraints.list.enforcedAt')"
                    readonly
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col cols="6" sm="3">
                  <v-text-field
                    v-model="formattedRevocationDate"
                    :label="$t('distraints.list.revocationDate')"
                    readonly
                  ></v-text-field>
                </v-col>
                <v-col cols="6" sm="3">
                  <v-text-field
                    v-model="formattedRevokedAt"
                    :label="$t('distraints.list.revokedAt')"
                    readonly
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col cols="6" sm="3">
                  <v-text-field
                    v-model="formattedExemptionDate"
                    :label="$t('distraints.list.exemptionDate')"
                    readonly
                  ></v-text-field>
                </v-col>
                <v-col cols="6" sm="3">
                  <v-text-field
                    v-model="formattedExemptedAt"
                    :label="$t('distraints.list.exemptedAt')"
                    readonly
                  ></v-text-field>
                </v-col>
              </v-row>
            </v-expansion-panel-content>
          </v-expansion-panel>

          <v-expansion-panel aria-expanded="true">
            <v-expansion-panel-header>{{
              $t("distraints.list.inFavourOf")
            }}</v-expansion-panel-header>
            <v-expansion-panel-content>
              <regix-person-view-card
                :item="item.inFavourOfPerson"
                v-if="isInFavourOfPerson"
              ></regix-person-view-card>
              <regix-company-view-card
                :item="item.inFavourOfCompany"
                v-if="!isInFavourOfPerson"
              ></regix-company-view-card>
            </v-expansion-panel-content>
          </v-expansion-panel>

          <v-expansion-panel aria-expanded="true">
            <v-expansion-panel-header>{{
              $t("distraints.list.debtor")
            }}</v-expansion-panel-header>
            <v-expansion-panel-content>
              <regix-person-view-card
                :item="item.debtorPerson"
                v-if="isDebtorPerson"
              ></regix-person-view-card>
              <regix-company-view-card
                :item="item.debtorCompany"
                v-if="!isDebtorPerson"
              ></regix-company-view-card>
            </v-expansion-panel-content>
          </v-expansion-panel>

          <v-expansion-panel aria-expanded="true">
            <v-expansion-panel-header
              >{{ $t("distraints.propertyDataTitle") }}
              {{ item.propertyTypeName }}</v-expansion-panel-header
            >
            <v-expansion-panel-content>
              <other-property-view-card
                :item="item.otherProperty"
                v-if="isOtherProperty"
              >
              </other-property-view-card>

              <vehicle-view-card :item="item.vehicleProperty" v-if="isVehicle">
              </vehicle-view-card>

              <aircraft-view-card
                :item="item.aircraftProperty"
                v-if="isAircraft"
              ></aircraft-view-card>

              <vessel-view-card
                :item="item.vesselProperty"
                v-if="isVessel"
              ></vessel-view-card>

              <agricultural-machine-view-card
                :item="item.agriculturalMachineProperty"
                v-if="isAgriculturalMachine"
              ></agricultural-machine-view-card>
            </v-expansion-panel-content>
          </v-expansion-panel>
        </v-expansion-panels>
      </v-row>
    </div>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop } from "vue-property-decorator";
import { DistraintViewModel } from "@/models/distraint";
import { PropertyType } from "@/models/enums";
import OtherPropertyViewCard from "@/components/properties/otherPropertyViewCard.vue";
import VehicleViewCard from "@/components/properties/vehicleViewCard.vue";
import AircraftViewCard from "@/components/properties/aircraftViewCard.vue";
import VesselViewCard from "@/components/properties/vesselViewCard.vue";
import AgriculturalMachineViewCard from "@/components/properties/agriculturalMachineViewCard.vue";
import { mapGetters } from "vuex";
import moment from "moment";
import distraintService from "@/services/distraint.service";
import ErrorMessage from "@/components/common/errorMessage.vue";
import VueI18n, { TranslateResult } from "vue-i18n";
import RegixPersonViewCard from "@/components/person/personRegixViewCard.vue";
import RegixCompanyViewCard from "@/components/company/companyRegixViewCard.vue";

@Component({
  components: {
    OtherPropertyViewCard,
    VehicleViewCard,
    AircraftViewCard,
    VesselViewCard,
    AgriculturalMachineViewCard,
    ErrorMessage,
    RegixPersonViewCard,
    RegixCompanyViewCard,
  },
  computed: {
    ...mapGetters({
      dateFormat: "dateFormat",
      dateTimeFormat: "dateTimeFormat",
      language: "language",
    }),
  },
})
export default class DistraintViewCard extends Vue {
  @Prop({ required: true }) id: any;
  private item = new DistraintViewModel();
  private dateMenu = false;
  dateFormat: any;
  dateTimeFormat: any;
  language: any;
  private propertyItemData: any = {};
  private showError = false;
  private errorMessages: [VueI18n.TranslateResult] = [""];
  private panel = [0, 1, 2, 3];

  constructor() {
    super();
  }

  get isOtherProperty() {
    return this.item.propertyTypeCode === PropertyType.Other;
  }

  get isVehicle() {
    return this.item.propertyTypeCode === PropertyType.Vehicle;
  }

  get isAircraft() {
    return this.item.propertyTypeCode === PropertyType.Aircraft;
  }

  get isVessel() {
    return this.item.propertyTypeCode === PropertyType.Vessel;
  }

  get isAgriculturalMachine() {
    return this.item.propertyTypeCode === PropertyType.AgriForMachinery;
  }

  get formattedEnforcementDate() {
    if (this.item.enforcementDate)
      return this.formatDate(this.item.enforcementDate);
    return "";
  }

  get formattedEnforcedAt() {
    if (this.item.enforcedAt) return this.formatDateTime(this.item.enforcedAt);
    return "";
  }

  get formattedRevocationDate() {
    if (this.item.revocationDate)
      return this.formatDate(this.item.revocationDate);
    return "";
  }

  get formattedRevokedAt() {
    if (this.item.revokedAt) return this.formatDateTime(this.item.revokedAt);
    return "";
  }

  get formattedExemptionDate() {
    if (this.item.exemptionDate)
      return this.formatDate(this.item.exemptionDate);
    return "";
  }

  get formattedExemptedAt() {
    if (this.item.exemptedAt) return this.formatDateTime(this.item.exemptedAt);
    return "";
  }

  get isInFavourOfPerson() {
    return this.item.isInFavourOfPerson;
  }

  get isDebtorPerson() {
    return this.item.isDebtorPerson;
  }

  async mounted() {
    await this.loadDistraintItem();
  }

  async loadDistraintItem() {
    try {
      this.resetErrorMessages();
      const result = await distraintService.get(this.id);
      this.item = result as DistraintViewModel;
      console.log(this.item);
    } catch (error) {
      this.setErrorMessage(this.$t("errors.errorViewingDistraint"), null);
    }
  }

  setErrorMessage(generalError: TranslateResult, error: any) {
    console.log(error);
    this.showError = true;
    if (generalError) {
      this.errorMessages.push(generalError);
    }
    if (error) {
      this.errorMessages.push(`${error.response.data}`);
    }
  }

  resetErrorMessages() {
    this.showError = false;
    this.errorMessages = [""];
  }

  formatDate(value: Date) {
    if (value) return moment(value.toLocaleString()).format(this.dateFormat);
    else return "";
  }

  formatDateTime(value: Date) {
    if (value != null)
      return moment(value.toLocaleString()).format(this.dateTimeFormat);
    else return value;
  }
}
</script>