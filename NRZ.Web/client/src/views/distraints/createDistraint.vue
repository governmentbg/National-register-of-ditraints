<template>
  <div class="distraint">
    <div class="mb-3 text-center">
      <h3>{{ $t("createDistraint.title") }}</h3>
    </div>
    <div id="app">
      <v-app id="inspire">
        <v-stepper v-model="currentStep">
          <v-stepper-header>
            <v-stepper-step step="1" :complete="step1Completed">
              {{ $t("createDistraint.stepSearchProperty") }}
            </v-stepper-step>

            <v-divider></v-divider>

            <v-stepper-step step="2" :complete="step2Completed">
              {{ $t("createDistraint.stepPropertyData") }}
            </v-stepper-step>

            <v-divider></v-divider>

            <v-stepper-step step="3" :complete="step3Completed">
              {{ $t("createDistraint.stepDistraintData") }}
            </v-stepper-step>

            <!-- <v-divider></v-divider>

            <v-stepper-step step="4">
              {{ $t("createDistraint.stepSaveDistraint") }}
            </v-stepper-step> -->
          </v-stepper-header>

          <v-stepper-items>
            <v-stepper-content step="1">
              <div class="col-6">
                <v-select
                  :items="propertyTypeItems"
                  item-text="name"
                  item-value="code"
                  dense
                  :label="$t('createDistraint.propertyTypeLabel')"
                  v-model="propertyTypeSelected"
                  return-object
                >
                </v-select>
                <!-- <v-checkbox
                  v-if="isAircraft"
                  v-model="searchByOwner"
                  :label="$t('createDistraint.searchByOwnerLabel')"
                ></v-checkbox> -->
              </div>

              <div class="col-6">
                <v-text-field
                  v-model="suitNumber"
                  :label="$t('createDistraint.suitNumber')"
                ></v-text-field>
              </div>

              <div class="col-6">
                <ValidationObserver v-slot="{ invalid }">
                  <div>
                    <ValidationProvider
                      ref="observer"
                      v-slot="{ errors }"
                      :name="$t('createDistraint.searchHint')"
                      rules="min:3"
                    >
                      <v-text-field
                        v-model="propertyIdentifier"
                        append-icon="search"
                        :label="searchIdentifierLabel"
                        :error-messages="errors"
                        :hint="$t('createDistraint.searchMinLength')"
                        single-line
                      ></v-text-field>
                    </ValidationProvider>
                  </div>
                  <div>
                    <v-btn
                      @click="submitPropertySearch"
                      :color="'primary'"
                      :disabled="disableSearch() || invalid"
                      >{{ $t("createDistraint.btnSearch") }}</v-btn
                    >
                  </div>
                </ValidationObserver>
              </div>

              <div>
                <label>
                  {{ searchPropertiesDBResult }}
                </label>
              </div>
              <div>
                <label>
                  {{ searchPropertiesAdminResult }}
                </label>
              </div>
              <div>
                <properties-list
                  :propertyType="propertyTypeSelected.code"
                  :items="propertyItems"
                  :selectable="true"
                  :singleSelect="true"
                  v-on:selected-property-changed="
                    selectedPropertyChanged($event)
                  "
                ></properties-list>
              </div>

              <div class="col-6">
                <v-btn
                  color="primary"
                  @click="
                    currentStep = 2;
                    step1Completed = true;
                    resetErrorMessages();
                  "
                  :disabled="
                    disableStep1Continue(
                      propertyTypeSelected.code,
                      propertyItemData
                    )
                  "
                >
                  {{ $t("createDistraint.btnStepContinue") }}
                </v-btn>

                <v-btn text @click="backToDistraintsList">
                  {{ $t("createDistraint.btnStepCancel") }}
                </v-btn>
              </div>
            </v-stepper-content>

            <v-stepper-content step="2">
              <v-card class="mb-12">
                <other-property-view-card
                  :item="propertyItemData"
                  v-if="isOtherProperty && !isNewProperty"
                >
                </other-property-view-card>

                <other-property-edit-card
                  ref="otherPropertyEditCard"
                  :item="propertyItemData"
                  v-if="isNewProperty"
                >
                </other-property-edit-card>

                <vehicle-view-card :item="propertyItemData" v-if="isVehicle">
                </vehicle-view-card>

                <aircraft-view-card
                  :item="propertyItemData"
                  v-if="isAircraft"
                ></aircraft-view-card>

                <vessel-view-card
                  :item="propertyItemData"
                  v-if="isVessel"
                ></vessel-view-card>

                <agricultural-machine-view-card
                  :item="propertyItemData"
                  v-if="isAgriculturalMachine"
                ></agricultural-machine-view-card>
              </v-card>

              <v-btn color="primary" @click="validateNewOtherPropertyData()">
                {{ $t("createDistraint.btnStepContinue") }}
              </v-btn>

              <v-btn
                @click="
                  currentStep = 1;
                  step2Completed = false;
                  resetErrorMessages();
                "
              >
                {{ $t("createDistraint.btnBack") }}
              </v-btn>

              <v-btn text @click="backToDistraintsList">
                {{ $t("createDistraint.btnStepCancel") }}
              </v-btn>
            </v-stepper-content>

            <v-stepper-content step="3">
              <v-card class="mb-12">
                <distraint-edit-card
                  ref="distraintEditCard"
                  :item="distraintItemData"
                  :useRegix="useRegix"
                  :today="today"
                >
                </distraint-edit-card>
              </v-card>

              <v-btn color="primary" @click="save()">
                {{ $t("createDistraint.btnSave") }}
              </v-btn>

              <v-btn
                @click="
                  currentStep = 2;
                  step3Completed = false;
                "
              >
                {{ $t("createDistraint.btnBack") }}
              </v-btn>

              <v-btn text @click="backToDistraintsList">
                {{ $t("createDistraint.btnStepCancel") }}
              </v-btn>
            </v-stepper-content>
          </v-stepper-items>
        </v-stepper>

        <error-message
          :show="showError"
          :messages="errorMessages"
        ></error-message>
      </v-app>
    </div>
  </div>
</template>



 <script lang="ts">
// @ is an alias to /src
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { Component, Vue, Watch, Ref } from "vue-property-decorator";
import { PropertyTypeModel } from "@/models/nomenclature";
import { PropertyType, DistraintStatus } from "@/models/enums";
import {
  OtherPropertyModel,
  PropertySearchRequestModel,
  VehicleViewModel,
  AircraftViewModel,
  VesselViewModel,
  AgriculturalMachineryViewModel,
} from "@/models/property";
import distraintService from "@/services/distraint.service";
import nomenclatureService from "@/services/nomenclature.service";
import propertyService from "@/services/property.service";
import { mapGetters } from "vuex";
import PropertiesList from "@/components/properties/propertiesList.vue";
import OtherPropertyEditCard from "@/components/properties/otherPropertyEditCard.vue";
import OtherPropertyViewCard from "@/components/properties/otherPropertyViewCard.vue";
import VehicleViewCard from "@/components/properties/vehicleViewCard.vue";
import AircraftViewCard from "@/components/properties/aircraftViewCard.vue";
import VesselViewCard from "@/components/properties/vesselViewCard.vue";
import AgriculturalMachineViewCard from "@/components/properties/agriculturalMachineViewCard.vue";
import DistraintEditCard from "@/components/distraints/distraintEditCard.vue";
import DistraintConfirmationCard from "@/components/distraints/distraintConfirmationCard.vue";
import ErrorMessage from "@/components/common/errorMessage.vue";
import { DistraintCreateModel, DistraintModel } from "@/models/distraint";
import propertyHelper from "@/helpers/property.helper";
import { namespace } from "vuex-class";
import VueI18n, { TranslateResult } from "vue-i18n";
const userNS = namespace("user");
import settingsService from "@/services/settings.service";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
    PropertiesList,
    OtherPropertyEditCard,
    OtherPropertyViewCard,
    VehicleViewCard,
    AircraftViewCard,
    VesselViewCard,
    AgriculturalMachineViewCard,
    DistraintEditCard,
    ErrorMessage,
  },
  computed: {
    ...mapGetters("user", ["getUserName", "getUserRoles"]),
  },
})
export default class CreateDistraint extends Vue {
  @userNS.Getter("getUserId") userId: any;
  @userNS.Getter("getUserName") userName: any;
  @userNS.Getter("getUserRoles") userRoles: any;
  private propertyTypeItems: PropertyTypeModel[] = [];
  private propertyTypeSelected: PropertyTypeModel = {
    code: "",
    name: "",
    nameEn: "",
    deactivated: false,
  };
  private propertyIdentifier = "";
  private propertyItems: object[] = [];
  private propertyItemData: any = { id: 0 };
  private currentStep = 1;
  private searchPropertiesDBResult: VueI18n.TranslateResult = "";
  private searchPropertiesAdminResult: VueI18n.TranslateResult = "";
  private step1Completed = false;
  private step2Completed = false;
  private step3Completed = false;
  private distraintItemData: any = new DistraintModel();
  @Ref("distraintEditCard")
  private distraintEditCard: any;
  @Ref("otherPropertyEditCard")
  private otherPropertyEditCard: any;
  private searchByOwner = false;
  private useRegix = false;
  private showError = false;
  private errorMessages: [VueI18n.TranslateResult] = [""];
  private today = new Date();

  get isOtherProperty() {
    return this.propertyTypeSelected.code === PropertyType.Other;
  }

  get isVehicle() {
    return this.propertyTypeSelected.code === PropertyType.Vehicle;
  }

  get isAircraft() {
    return this.propertyTypeSelected.code === PropertyType.Aircraft;
  }

  get isVessel() {
    return this.propertyTypeSelected.code === PropertyType.Vessel;
  }

  get isAgriculturalMachine() {
    return this.propertyTypeSelected.code === PropertyType.AgriForMachinery;
  }

  get isNewProperty() {
    return (
      this.propertyTypeSelected.code === PropertyType.Other &&
      this.propertyItemData &&
      this.propertyItemData.id == 0
    );
  }

  get suitNumber() {
    return this.distraintItemData.suitNumber;
  }

  set suitNumber(newValue: string) {
    this.distraintItemData.suitNumber = newValue;
  }

  get searchIdentifierLabel() {
    if (
      this.propertyTypeSelected.code &&
      this.propertyTypeSelected.code == PropertyType.Other
    ) {
      return this.$t("propertiesList.otherProperties.identifier");
    } else if (
      this.propertyTypeSelected.code &&
      this.propertyTypeSelected.code == PropertyType.Aircraft
    ) {
      return this.$t("propertiesList.aircrafts.msnserialNumber");
    } else if (
      this.propertyTypeSelected.code &&
      this.propertyTypeSelected.code == PropertyType.Vehicle
    ) {
      return this.$t("propertiesList.vehicles.registrationNumber");
    } else if (
      this.propertyTypeSelected.code &&
      this.propertyTypeSelected.code == PropertyType.Vessel
    ) {
      return this.$t("propertiesList.vessels.ownerIdentifier");
    } else if (
      this.propertyTypeSelected.code &&
      this.propertyTypeSelected.code == PropertyType.AgriForMachinery
    ) {
      return this.$t("agriculturalMachinery.registrationNumber");
    }

    return this.$t("createDistraint.searchIdentifier");
  }

  disableSearch() {
    if (
      !this.propertyIdentifier ||
      !this.propertyTypeSelected.code ||
      (!this.suitNumber &&
        this.propertyTypeSelected.code != PropertyType.Other &&
        this.propertyTypeSelected.code != PropertyType.AgriForMachinery)
    )
      return true;
    return false;
  }

  disableStep1Continue(code: string, selectedItem: any) {
    if (
      code != PropertyType.Other &&
      (selectedItem == null || selectedItem.id == 0)
    )
      return true;
    else return false;
  }

  mounted() {
    this.distraintItemData.createdByUserName = this.userName;
    this.distraintItemData.createdByUserType = this.userRoles;
    this.fetchPropertyTypes();
    this.checkRegix();
    this.getToday();
  }

  @Watch("propertyTypeSelected")
  propertyTypeChanged() {
    this.propertyItems = [];
    this.propertyItemData = { id: 0 };
  }

  async checkRegix() {
    try {
      const result = await settingsService.getUseRegix();
      this.useRegix = result as boolean;
      console.log(this.useRegix);
    } catch (error) {
      console.log(error);
    }
  }

  async getToday() {
    try {
      const result = await settingsService.getToday();
      this.today = new Date(result as Date);
      console.log(this.today);
    } catch (error) {
      console.log(error);
    }
  }

  async fetchPropertyTypes() {
    try {
      const result = await nomenclatureService.getPropertyTypesForDistraints();
      this.propertyTypeItems = result as PropertyTypeModel[];
    } catch (error) {
      this.setErrorMessage(this.$t("errors.errorGettingPropertyTypes"), null);
    }
  }

  resetDataBeforeSearch() {
    this.propertyItems = [];
    this.propertyItemData = { id: 0 };
    this.searchPropertiesDBResult = "";
    this.searchPropertiesAdminResult = "";
    this.resetErrorMessages();
  }

  resetErrorMessages() {
    this.showError = false;
    this.errorMessages = [""];
  }

  async submitPropertySearch() {
    this.resetDataBeforeSearch();

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

      if (
        this.propertyItems.length == 0 &&
        this.propertyTypeSelected.code != PropertyType.Other
      ) {
        if (this.useRegix) {
          await this.searchForPropertyInAdministrator(request);
        } else {
          this.searchPropertiesAdminResult = this.$t("common.useRegixNo");
        }
      }
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
      this.setErrorMessage(this.$t("errors.errorSearchingPropertyInDB"), null);
    }
  }

  async searchForPropertyInAdministrator(request: PropertySearchRequestModel) {
    try {
      this.searchPropertiesAdminResult = this.$t(
        "createDistraint.searchingPropertiesInAdmin"
      );

      const result = await propertyService.searchForPropertyInAdministrator(
        this.propertyTypeSelected.code,
        request
      );

      this.getPropertyItemsFromResult(result);
      console.log(this.propertyItems);

      if (this.propertyItems.length == 0) {
        this.searchPropertiesAdminResult = this.$t(
          "createDistraint.noPropertiesFoundInAdministrator"
        );
      } else {
        this.searchPropertiesAdminResult = this.$t(
          "createDistraint.propertiesFoundInAdministrator"
        );
      }
    } catch (error) {
      this.setErrorMessage(
        this.$t("errors.errorSearchingPropertyInDataAdmin"),
        null
      );
    }
  }

  getPropertyItemsFromResult(result: any) {
    if (this.propertyTypeSelected.code === PropertyType.Other)
      this.propertyItems = result as OtherPropertyModel[];
    else if (this.propertyTypeSelected.code === PropertyType.Vehicle)
      this.propertyItems = result as VehicleViewModel[];
    else if (this.propertyTypeSelected.code === PropertyType.Aircraft)
      this.propertyItems = result as AircraftViewModel[];
    else if (this.propertyTypeSelected.code === PropertyType.Vessel)
      this.propertyItems = result as VesselViewModel[];
    else if (this.propertyTypeSelected.code === PropertyType.AgriForMachinery)
      this.propertyItems = result as AgriculturalMachineryViewModel[];
    else this.propertyItems = result as object[];
  }

  backToDistraintsList() {
    this.$router.push({ path: "/distraints/list" });
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

  selectedPropertyChanged(newVal: any) {
    this.propertyItemData = newVal[0];
  }

  async validateNewOtherPropertyData() {
    if (this.isOtherProperty && this.isNewProperty) {
      const isNewOtherPropertyValid = await this.otherPropertyEditCard.validateInput();
      if (isNewOtherPropertyValid == true) {
        this.currentStep = 3;
        this.step2Completed = true;
        console.log(this.propertyItemData);
      } else {
        console.log("Invalid new other property data, cannot continue!");
      }
    } else {
      if (this.propertyItemData && this.propertyItemData.id != 0) {
        this.currentStep = 3;
        this.step2Completed = true;
      } else console.log("Invalid property data, cannot continue!");
    }
  }

  async validateDistraintData() {
    const isDistraintValid = await this.distraintEditCard.validateInput();
    if (isDistraintValid == true) {
      console.log(this.distraintItemData);
      return true;
    } else {
      console.log("Invalid distraint data, cannot continue!");
      return false;
    }
  }

  async save() {
    this.resetErrorMessages();

    if (await this.validateDistraintData()) {
      const createModel = new DistraintCreateModel(this.distraintItemData);
      createModel.propertyTypeCode = this.propertyTypeSelected.code;
      createModel.searchedIdentifier = this.propertyIdentifier;
      createModel.isNewProperty = this.isNewProperty;
      createModel.propertyIdVehicle =
        this.propertyTypeSelected.code == PropertyType.Vehicle
          ? this.propertyItemData.id
          : undefined;
      createModel.propertyIdAircraft =
        this.propertyTypeSelected.code == PropertyType.Aircraft
          ? this.propertyItemData.id
          : undefined;
      createModel.propertyIdVessel =
        this.propertyTypeSelected.code == PropertyType.Vessel
          ? this.propertyItemData.id
          : undefined;
      createModel.propertyIdAgriForMachinery =
        this.propertyTypeSelected.code == PropertyType.AgriForMachinery
          ? this.propertyItemData.id
          : undefined;
      createModel.propertyIdRealEstate =
        this.propertyTypeSelected.code == PropertyType.RealEstate
          ? this.propertyItemData.id
          : undefined;
      createModel.propertyIdOtherProperty =
        this.propertyTypeSelected.code == PropertyType.Other
          ? this.propertyItemData.id
          : undefined;
      createModel.newOtherProperty = new OtherPropertyModel();
      createModel.vehicleProperty = new VehicleViewModel();
      createModel.aircraftProperty = new AircraftViewModel();
      createModel.vesselProperty = new VesselViewModel();
      createModel.agriculturalMachineProperty = new AgriculturalMachineryViewModel();

      if (createModel.isNewProperty == true) {
        createModel.newOtherProperty = new OtherPropertyModel(
          this.propertyItemData
        );
      } else if (
        this.propertyItemData.id == -1 &&
        this.propertyTypeSelected.code == PropertyType.Vehicle
      ) {
        createModel.vehicleProperty = new VehicleViewModel(
          this.propertyItemData
        );
        createModel.propertyIdVehicle = -1;
      } else if (
        this.propertyItemData.id == -1 &&
        this.propertyTypeSelected.code == PropertyType.Aircraft
      ) {
        createModel.aircraftProperty = new AircraftViewModel(
          this.propertyItemData
        );
        createModel.propertyIdAircraft = -1;
      } else if (
        this.propertyItemData.id == -1 &&
        this.propertyTypeSelected.code == PropertyType.Vessel
      ) {
        createModel.vesselProperty = new VesselViewModel(this.propertyItemData);
        createModel.propertyIdVessel = -1;
      } else if (
        this.propertyItemData.id == -1 &&
        this.propertyTypeSelected.code == PropertyType.AgriForMachinery
      ) {
        createModel.agriculturalMachineProperty = new AgriculturalMachineryViewModel(
          this.propertyItemData
        );
        createModel.propertyIdAgriForMachinery = this.propertyItemData.id;
      }

      createModel.statusCode = DistraintStatus.Enforced;
      createModel.createdBy = this.userId;
      createModel.enforcedBy = this.userId;

      console.log(createModel);
      try {
        const result = await distraintService.create(createModel);
        console.log("save result: " + result);

        this.backToDistraintsList();
      } catch (error) {
        this.setErrorMessage(this.$t("errors.errorCreatingDistraint"), null);
      }
    }
  }
}
</script>