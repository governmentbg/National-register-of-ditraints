<template>
  <div class="RegixCompany">
    <div>
      <ValidationObserver ref="validator">
        <v-row>
          <v-col cols="6" sm="3">
            <ValidationProvider
              v-slot="{ errors }"
              :name="$t('regixCompany.name')"
              rules="required"
            >
              <v-text-field
                v-model="localItem.name"
                :label="$t('regixCompany.name')"
                :error-messages="errors"
                class="required"
                :readonly="isReadonly"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
          <v-col cols="6" sm="3">
            <v-text-field
              v-model="localItem.legalFormAbbr"
              :label="$t('regixCompany.legalFormAbbr')"
              :readonly="isReadonly"
            ></v-text-field>
          </v-col>
          <v-col cols="12" sm="6">
            <v-text-field
              v-model="localItem.legalFormName"
              :label="$t('regixCompany.legalFormName')"
              :readonly="isReadonly"
            ></v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="6" sm="3">
            <ValidationProvider
              v-slot="{ errors }"
              :name="$t('regixCompany.uic')"
              rules="required"
            >
              <v-text-field
                v-model="localItem.uic"
                :label="$t('regixCompany.uic')"
                :error-messages="errors"
                class="required"
                :readonly="isReadonly"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
          <v-col cols="6" sm="3">
            <v-select
              :items="companyStatusItems"
              item-text="name"
              item-value="code"
              dense
              :label="$t('regixCompany.statusName')"
              v-model="localItem.statusCode"    
              :readonly="isReadonly"          
            >
            </v-select>
          </v-col>
        </v-row>
      </ValidationObserver>
    </div>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { Component, Vue, Prop, Ref } from "vue-property-decorator";
import { RegixCompanyModel } from "@/models/company";
import { Nomenclature } from '@/models/nomenclature';
import nomenclatureService from "@/services/nomenclature.service";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
  },
})
export default class RegixCompanyCard extends Vue {
  @Prop({ required: true }) item: any;
  @Prop({ required: true }) isReadonlyCard: any;
  @Ref("validator")
  private validator: any;
  private companyStatusItems: Nomenclature[] = [];

  constructor() {
    super();
  }

  get localItem() {
    return this.item;
  }

  get isReadonly() {
    return this.isReadonlyCard;
  }

  mounted() {
    this.fetchCompanyStatusItems();
  }

  public async validateInput() {
    const formValid = await this.validator.validate();
    return formValid;
  }

    async fetchCompanyStatusItems() {
    try {
      const result = await nomenclatureService.getRegixCompanyStatuses();
      this.companyStatusItems = result as Nomenclature[];
    } catch (error) {
      //this.setErrorMessage(this.$t("errors.errorGettingPropertyTypes"), error);
    }
  }
}
</script>