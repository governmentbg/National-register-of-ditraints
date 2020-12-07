<template>
    <div>
        <v-row>
            <v-col cols="4">
                <ValidationProvider
                    v-slot="{ errors }"
                    rules="required"
                    :name="$t('company.name')"
                >
                    <v-text-field
                        class="required"
                        :disabled="disabled"
                        :label="`${$t('company.name')}`"
                        :error-messages="errors"
                        v-model="company.name"
                    ></v-text-field>
                </ValidationProvider>
            </v-col>
            <v-col cols="4">
                <ValidationProvider
                    v-slot="{ errors }"
                    rules="required"
                    :name="$t('company.eik')"
                >
                    <v-text-field
                        class="required"
                        :disabled="disabled"
                        :label="`${$t('company.eik')}`"
                        :error-messages="errors"
                        v-model="company.eik"
                    ></v-text-field>
                </ValidationProvider>
            </v-col>
            <v-col cols="4">
                <v-text-field
                    :disabled="disabled"
                    :label="$t('company.companyCaseNumber')"
                    v-model="company.companyCaseNumber"
                ></v-text-field>
            </v-col>
        </v-row>
        <div v-if="showAddress && isAddressRequired === false">
            <v-checkbox :label="$t('person.populatingAddress')" v-model="showAddressComponent"></v-checkbox>
        </div>
        <div v-if="showAddress && showAddressComponent">
            <h5>{{ $t("common.address") }}</h5>
            <address-component v-model="company.address" :disabled="disabled" :is-required="showAddressComponent"></address-component> <!-- :is-required="isAddressRequired" -->
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import AddressComponent from "../address/address.vue";
import CompanyModel from "@/models/company";
import { ValidationObserver, ValidationProvider } from "vee-validate";
import Address from "@/models/address";
import AddressModel from "@/models/address";

@Component({
    components: {
        AddressComponent,
        ValidationProvider,
        ValidationObserver,
    },
})
export default class CompanyComponent extends Vue {
    @Prop({default: true}) showAddress!: boolean;
    @Prop({default: new CompanyModel()}) company!: CompanyModel;
    @Prop({default: false}) disabled!: boolean;
    @Prop({default: true}) isAddressRequired!: AddressModel;

    showAddressComponent = false;

    constructor() {
        super();
    }

    @Watch('showAddressComponent')
    onShowAddressComponentChange() {
        if (this.showAddressComponent) {
            if (!this.company.address) {
                this.company.address = new AddressModel();
            }
        } else {
            this.company.address = null;
        }
    }

    mounted() {
        if (this.showAddress && this.isAddressRequired &&this.company && this.company.address === null) {
            this.company.address = new Address();
        } else {
            this.company.address = null;
        }

        if (this.isAddressRequired) {
            this.showAddressComponent = true;
        }
    }
}
</script>
