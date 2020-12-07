<template>
    <div>
        <v-row>
            <v-col cols="4">
                <ValidationProvider
                    v-slot="{ errors }"
                    rules="required"
                    :name="$t('person.firstName')"
                >
                    <v-text-field
                        class="required"
                        :disabled="disabled"
                        :label="$t('person.firstName')"
                        :name="'firstName' + _uid"
                        v-model="person.firstName"
                        :error-messages="errors"
                    ></v-text-field>
                </ValidationProvider>
            </v-col>
            <v-col cols="4">
                <v-text-field
                    :disabled="disabled"
                    :label="$t('person.middleName')"
                    v-model="person.middleName"
                ></v-text-field>
            </v-col>
            <v-col cols="4">
                <ValidationProvider
                    v-slot="{ errors }"
                    rules="required"
                    :name="$t('person.lastName')"
                >
                    <v-text-field
                        class="required"
                        :disabled="disabled"
                        :label="$t('person.lastName')"
                        v-model="person.lastName"
                        :error-messages="errors"
                    ></v-text-field>
                </ValidationProvider>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="4">
                <v-text-field
                    :disabled="disabled"
                    :label="$t('person.phone')"
                    v-model="person.phone"
                ></v-text-field>
            </v-col>
            <v-col cols="4">
                <ValidationProvider
                    v-slot="{ errors }"
                    :rules="isEmailRequired ? 'required' : ''"
                    :name="$t('person.email')"
                >
                    <v-text-field
                        :class="isEmailRequired ? 'required' : ''"
                        :disabled="disabled"
                        :label="$t('person.email')"
                        v-model="person.email"
                        :error-messages="errors"
                    ></v-text-field>
                </ValidationProvider>
            </v-col>
            <v-col cols="4" class="removePaddingTop">
                <v-row>
                    <v-col cols="4">
                        <ValidationProvider
                            v-slot="{ errors }"
                            rules="required"
                            :name="$t('person.identificationType')"
                        >
                            <v-select
                                class="required"
                                :disabled="disabled"
                                :items="identificationTypes"
                                item-text="text"
                                item-value="id"
                                :label="$t('person.identificationType')"
                                v-model="person.identificationType"
                                :error-messages="errors"
                            >
                            </v-select>
                        </ValidationProvider>
                    </v-col>
                    <v-col cols="8">
                        <ValidationProvider
                            v-slot="{ errors }"
                            rules="required"
                            :name="$t('person.number')"
                        >
                            <v-text-field
                                class="required"
                                :disabled="disabled"
                                :label="$t('person.number')"
                                v-model="person.identificationNumber"
                                :error-messages="errors"
                            ></v-text-field>
                        </ValidationProvider>
                    </v-col>
                </v-row>
            </v-col>
        </v-row>
        <div v-if="showAddress && isAddressRequired === false">
            <v-checkbox :label="$t('person.populatingAddress')" v-model="showAddressComponent"></v-checkbox>
        </div>
        <div v-if="showAddress && showAddressComponent">
            <h5>{{ $t("common.address") }}</h5>
            <address-component v-model="person.address" :disabled="disabled" :is-required="showAddressComponent"></address-component> <!-- :is-required="isAddressRequired" -->
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import PersonModel from "../../models/person";
import AddressComponent from "@/components/address/address.vue";
import { IdentificationType } from "@/models/enums";
import { ValidationObserver, ValidationProvider } from "vee-validate";
import Address from "@/models/address";
import helper from "@/helpers/helper";
import AddressModel from "@/models/address";

@Component({
    components: {
        AddressComponent,
        ValidationProvider,
        ValidationObserver,
    },
})
export default class PersonComponent extends Vue {
    @Prop({default: true}) showAddress!: boolean;
    @Prop({required: true}) person!: PersonModel;
    @Prop({default: false}) disabled!: boolean;
    @Prop({default: true}) isAddressRequired!: AddressModel;
    @Prop({default: true}) isEmailRequired!: boolean;

    identificationTypes: object[];
    showAddressComponent = false; //this.showAddress;

    constructor() {
        super();
        // TODO: Get identification types from API
        this.identificationTypes = [
            {
                id: IdentificationType.EGN,
                text: "ЕГН", //this.$t('person.egn')
            },
            {
                id: IdentificationType.LN4,
                text: "ЛНЧ", //this.$t('person.ln4')
            },
        ];
    }

    @Watch('showAddressComponent')
    onShowAddressComponentChange() {
        if (this.showAddressComponent) {
            if (!this.person.address) {
                this.person.address = new AddressModel();
            }
        } else {
            this.person.address = null;
        }
    }

    mounted() {
        if (this.showAddress && this.isAddressRequired && this.person && this.person.address === null) {
            this.person.address = new Address();
        }

        if (this.isAddressRequired) {
            this.showAddressComponent = true;
        }

        if(!this.showAddress) {
            this.person.address = null;
        }

        helper.disableInput();
    }
}
</script>

<style scoped>
.removePaddingTop {
    padding-top: 0;
}
</style>
