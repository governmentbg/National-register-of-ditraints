<template>
  <div class="distraintEdit">
    <div>
      <v-expansion-panels v-model="panel" multiple>
        <v-expansion-panel aria-expanded="true">
          <v-expansion-panel-header>{{
            $t("createDistraint.commonData")
          }}</v-expansion-panel-header>
          <v-expansion-panel-content>
            <ValidationObserver ref="validator">
              <v-row>
                <v-col cols="6" sm="3">
                  <ValidationProvider
                    v-slot="{ errors }"
                    :name="$t('distraints.list.suitNumber')"
                    rules="required"
                  >
                    <v-text-field
                      class="required"
                      v-model="localItem.suitNumber"
                      :label="$t('distraints.list.suitNumber')"
                      :error-messages="errors"
                    ></v-text-field>
                  </ValidationProvider>
                </v-col>
                <v-col cols="6" sm="3">
                  <v-menu
                    v-model="dateMenu"
                    :close-on-content-click="false"
                    :nudge-right="40"
                    transition="scale-transition"
                    offset-y
                    min-width="290px"
                  >
                    <template v-slot:activator="{ on, attrs }">
                      <ValidationProvider
                        v-slot="{ errors }"
                        :name="$t('distraints.list.enforcementDate')"
                        rules="required"
                      >
                        <v-text-field
                          class="required"
                          :label="$t('distraints.list.enforcementDate')"
                          prepend-icon="mdi-calendar"
                          readonly
                          v-bind="attrs"
                          v-on="on"
                          v-model="formattedEnforcementDate"
                          :error-messages="errors"
                        ></v-text-field>
                      </ValidationProvider>
                    </template>
                    <v-date-picker
                      v-model="localItem.enforcementDate"
                      @input="dateMenu = false"
                      :first-day-of-week="1"
                      :locale="language"
                      :max="maxDate"
                    ></v-date-picker>
                  </v-menu>
                </v-col>
                <v-col cols="6" sm="3">
                  <v-text-field
                    v-model="localItem.createdByUserType"
                    :label="$t('distraints.list.createdByUserType')"
                    readonly
                  ></v-text-field>
                </v-col>
                <v-col cols="6" sm="3">
                  <v-text-field
                    v-model="localItem.createdByUserName"
                    :label="$t('distraints.list.createdByUserName')"
                    readonly
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="localItem.location"
                    :label="$t('distraints.list.location')"
                  ></v-text-field>
                </v-col>
              </v-row>
            </ValidationObserver>
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel aria-expanded="true">
          <v-expansion-panel-header>{{
            $t("distraints.list.inFavourOf")
          }}</v-expansion-panel-header>
          <v-expansion-panel-content>
            <v-row>
              <v-col cols="6" sm="3">
                <v-container class="px-0" fluid>
                  <v-checkbox
                    v-model="isInFavourOfPerson"
                    :label="$t('personType.person')"
                  ></v-checkbox>
                </v-container>
              </v-col>
              <v-col cols="6" sm="3">
                <v-text-field
                  v-model="inFavourOfSearchIdentifier"
                  :label="$t('person.identifier')"
                  :readonly="!canUseRegix"
                ></v-text-field>
              </v-col>
              <v-col cols="6" sm="3">
                <v-btn
                  :disabled="disableInFavourOfRegix"
                  @click="validateInFavourOfInRegix()"
                >
                  {{ $t("person.validateInRegix") }}
                </v-btn>
                <label v-if="!canUseRegix">
                  {{ $t("common.useRegixNo") }}
                </label>
              </v-col>
              <v-col cols="6" sm="3">
                <label>
                  {{ invalidInFavourOfPerson }}
                </label>
              </v-col>
            </v-row>
            <regix-person-card
              ref="inFavourOfPersonCard"
              :item="localInFavourOfPerson"
              :isReadonlyCard="canUseRegix"
              v-if="isInFavourOfPerson"
            ></regix-person-card>
            <regix-company-card
              ref="inFavourOfCompanyCard"
              :item="localInFavourOfCompany"
              :isReadonlyCard="canUseRegix"
              v-if="!isInFavourOfPerson"
            ></regix-company-card>
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel aria-expanded="true">
          <v-expansion-panel-header>{{
            $t("distraints.list.debtor")
          }}</v-expansion-panel-header>
          <v-expansion-panel-content>
            <v-row>
              <v-col cols="6" sm="3">
                <v-container class="px-0" fluid>
                  <v-checkbox
                    v-model="isDebtorPerson"
                    :label="$t('personType.person')"
                  ></v-checkbox>
                </v-container>
              </v-col>
              <v-col cols="6" sm="3">
                <v-text-field
                  v-model="debtorSearchIdentifier"
                  :label="$t('person.identifier')"
                  :readonly="!canUseRegix"
                ></v-text-field>
              </v-col>
              <v-col cols="6" sm="3">
                <v-btn
                  :disabled="disableDebtorRegix"
                  @click="validateDebtorInRegix"
                >
                  {{ $t("person.validateInRegix") }}
                </v-btn>
                <label v-if="!canUseRegix">
                  {{ $t("common.useRegixNo") }}
                </label>
              </v-col>
              <v-col cols="6" sm="3">
                <label>
                  {{ invalidDebtorPerson }}
                </label>
              </v-col>
            </v-row>
            <regix-person-card
              ref="debtorPersonCard"
              :item="localDebtorPerson"
              :isReadonlyCard="canUseRegix"
              v-if="isDebtorPerson"
            ></regix-person-card>
            <regix-company-card
              ref="debtorCompanyCard"
              :item="localDebtorCompany"
              :isReadonlyCard="canUseRegix"
              v-if="!isDebtorPerson"
            ></regix-company-card>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
    </div>
    <div>
      <v-row>
        <error-message
          :show="showError"
          :messages="errorMessages"
        ></error-message>
      </v-row>
    </div>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { Component, Vue, Prop, Ref } from "vue-property-decorator";
import { mapGetters } from "vuex";
import moment from "moment";
import RegixPersonCard from "@/components/person/personRegixCard.vue";
import { RegixPersonModel } from "@/models/person";
import personService from "@/services/person.service";
import VueI18n, { TranslateResult } from "vue-i18n";
import ErrorMessage from "@/components/common/errorMessage.vue";
import RegixCompanyCard from "@/components/company/companyRegixCard.vue";
import { RegixCompanyModel } from "@/models/company";
import companyService from "@/services/company.service";
import { DistraintModel } from "@/models/distraint";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
    RegixPersonCard,
    RegixCompanyCard,
    ErrorMessage,
  },
  computed: {
    ...mapGetters({
      dateFormat: "dateFormat",
      language: "language",
    }),
  },
})
export default class DistraintEditCard extends Vue {
  @Prop({ required: true }) item!: DistraintModel;
  @Prop({ required: true }) useRegix!: boolean;
  @Prop({ required: true }) today!: Date;
  private dateMenu = false;
  dateFormat: any;
  language: any;
  @Ref("validator")
  private validator: any;
  @Ref("inFavourOfPersonCard")
  private inFavourOfPersonCard: any;
  @Ref("debtorPersonCard")
  private debtorPersonCard: any;
  private inFavourOfSearchIdentifier = "";
  private debtorSearchIdentifier = "";
  private showError = false;
  private errorMessages: [VueI18n.TranslateResult] = [""];
  private invalidInFavourOfPerson: VueI18n.TranslateResult = "";
  private invalidDebtorPerson: VueI18n.TranslateResult = "";
  @Ref("inFavourOfCompanyCard")
  private inFavourOfCompanyCard: any;
  @Ref("debtorCompanyCard")
  private debtorCompanyCard: any;
  private panel = [0, 1, 2];

  constructor() {
    super();
  }

  get localItem() {
    return this.item;
  }

  get maxDate() {
    if (this.today) 
    return (new Date(this.today.getFullYear(), this.today.getMonth(), this.today.getDate() + 1)).toISOString().slice(0, 10);

    const now = new Date();
    const t = new Date(now.getFullYear(), now.getMonth(), now.getDate() + 1);
    return t.toISOString().slice(0, 10);
  }

  get canUseRegix() {
    return this.useRegix;
  }

  get localInFavourOfPerson() {
    return this.item.inFavourOfPerson;
  }

  get localDebtorPerson() {
    return this.item.debtorPerson;
  }

  get localInFavourOfCompany() {
    return this.item.inFavourOfCompany;
  }

  get localDebtorCompany() {
    return this.item.debtorCompany;
  }

  get isInFavourOfPerson() {
    return this.item.isInFavourOfPerson;
  }

  set isInFavourOfPerson(newValue: boolean) {
    this.item.isInFavourOfPerson = newValue;
  }

  get isDebtorPerson() {
    return this.item.isDebtorPerson;
  }

  set isDebtorPerson(newValue: boolean) {
    this.item.isDebtorPerson = newValue;
  }

  get disableInFavourOfRegix() {
    if (
      this.canUseRegix &&
      this.inFavourOfSearchIdentifier != null &&
      this.inFavourOfSearchIdentifier.length > 0
    )
      return false;
    return true;
  }

  get disableDebtorRegix() {
    if (
      this.canUseRegix &&
      this.debtorSearchIdentifier != null &&
      this.debtorSearchIdentifier.length > 0
    )
      return false;
    return true;
  }

  get formattedEnforcementDate() {
    return this.formatDate(this.localItem.enforcementDate);
  }

  formatDate(value: Date | undefined) {
    if (value) return moment(value.toLocaleString()).format(this.dateFormat);
    else return "";
  }

  public async validateInput() {
    let isInFavourOfValid = false;
    if (this.item.isInFavourOfPerson) {
      isInFavourOfValid = await this.inFavourOfPersonCard.validateInput();
    } else {
      isInFavourOfValid = await this.inFavourOfCompanyCard.validateInput();
    }

    let isDebtorValid = false;
    if (this.item.isDebtorPerson) {
      isDebtorValid = await this.debtorPersonCard.validateInput();
    } else {
      isDebtorValid = await this.debtorCompanyCard.validateInput();
    }

    const formValid = await this.validator.validate();
    return isInFavourOfValid && isDebtorValid && formValid;
  }

  resetInFavourOfDataBeforeSearch() {
    this.invalidInFavourOfPerson = "";
    this.resetErrorMessages();
  }

  resetDebtorDataBeforeSearch() {
    this.invalidDebtorPerson = "";
    this.resetErrorMessages();
  }

  resetErrorMessages() {
    this.showError = false;
    this.errorMessages = [""];
  }

  validateInFavourOfInRegix() {
    this.resetInFavourOfDataBeforeSearch();
    if (this.item.isInFavourOfPerson) {
      this.getPersonFromRegix(this.inFavourOfSearchIdentifier, false);
    } else {
      this.getCompanyFromRegix(this.inFavourOfSearchIdentifier, false);
    }
  }

  validateDebtorInRegix() {
    this.resetDebtorDataBeforeSearch();
    if (this.item.isDebtorPerson) {
      this.getPersonFromRegix(this.debtorSearchIdentifier, true);
    } else {
      this.getCompanyFromRegix(this.debtorSearchIdentifier, true);
    }
  }

  async getPersonFromRegix(identifier: string, isDebtor: boolean) {
    try {
      const result = await personService.getFromRegix(identifier);
      const person = result as RegixPersonModel;

      if (!isDebtor) {
        this.item.inFavourOfPerson = person;
        if (
          this.item.inFavourOfPerson.firstName == null &&
          this.item.inFavourOfPerson.lastName == null &&
          this.item.inFavourOfPerson.dateOfBirth == null
        ) {
          this.invalidInFavourOfPerson = this.$t("person.invalidRegixPerson");
        }
      } else {
        this.item.debtorPerson = person;
        if (
          this.item.debtorPerson.firstName == null &&
          this.item.debtorPerson.lastName == null &&
          this.item.debtorPerson.dateOfBirth == null
        ) {
          this.invalidDebtorPerson = this.$t("person.invalidRegixPerson");
        }
      }

      console.log(person);
    } catch (error) {
      //this.setErrorMessage(this.$t("errors.errorSearchingPersonInRegix"), null);
      if (!isDebtor) {
        this.invalidInFavourOfPerson = this.$t(
          "errors.errorSearchingPersonInRegix"
        );
      } else {
        this.invalidDebtorPerson = this.$t(
          "errors.errorSearchingPersonInRegix"
        );
      }
    }
  }

  async getCompanyFromRegix(identifier: string, isDebtor: boolean) {
    try {
      const result = await companyService.getFromRegix(identifier);
      const company = result as RegixCompanyModel;

      if (!isDebtor) {
        this.item.inFavourOfCompany = company;
        if (
          this.item.inFavourOfCompany.name == null &&
          this.item.inFavourOfCompany.uic == null &&
          this.item.inFavourOfCompany.legalFormAbbr == null
        ) {
          this.invalidInFavourOfPerson = this.$t("person.invalidRegixCompany");
        }
      } else {
        this.item.debtorCompany = company;
        if (
          this.item.debtorCompany.name == null &&
          this.item.debtorCompany.uic == null &&
          this.item.debtorCompany.legalFormAbbr == null
        ) {
          this.invalidDebtorPerson = this.$t("person.invalidRegixCompany");
        }
      }

      console.log(company);
    } catch (error) {
      if (!isDebtor) {
        this.invalidInFavourOfPerson = this.$t(
          "errors.errorSearchingCompanyInRegix"
        );
      } else {
        this.invalidDebtorPerson = this.$t(
          "errors.errorSearchingCompanyInRegix"
        );
      }
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
}
</script>