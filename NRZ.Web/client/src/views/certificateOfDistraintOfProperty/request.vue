<template>
    <ValidationObserver ref="validationObserver">
        <div>
            <v-row :justify="'center'">
                <v-col cols="12">
                    <v-card>
                        <v-card-title>{{ $t('RequestForCertificateOfDistraintOfProperty.requestTitle') }}</v-card-title>
                        <v-card-text>
                            <person-component
                                v-if="isPerson"
                                :person="model.requester"
                                :disabled="isPersonDataFilled">
                            </person-component>
                            <v-row>
                                <v-col cols="12">
                                    <ValidationProvider
                                        v-slot="{ errors }"
                                        rules="required"
                                        :name="$t('RequestForCertificateOfDistraintOfProperty.as')">
                                        <v-select
                                            v-model="model.inTheQualityOfPersonTypeCode"
                                            :items="userTypes"
                                            :item-text="'name'"
                                            :name="'as' + _uid"
                                            :item-value="'code'"
                                            :error-messages="errors"
                                            :label="$t('RequestForCertificateOfDistraintOfProperty.as')"
                                        ></v-select>
<!--                                        <v-select
                                            v-if="!model.inTheQualityOfPersonTypeCode && !(isPerson && !isPersonDataFilled)"
                                            class="required"
                                            v-model="model.inTheQualityOfPersonTypeCode"
                                            :items="userTypes"
                                            :item-text="'name'"
                                            :name="'as' + _uid"
                                            :item-value="'code'"
                                            :error-messages="errors"
                                            :label="$t('RequestForCertificateOfDistraintOfProperty.as')"
                                        ></v-select>-->
                                    </ValidationProvider>
                                </v-col>
                            </v-row>
                            <company-component
                                v-if="!this.isPerson"
                                :company="model.requesterCompany"
                                :showAddress="true">
                            </company-component>
                            <v-col md="12">
                                <ValidationProvider
                                    :rules="`isPropertyChecked:${model.propertyItems}`"
                                    v-slot="{ errors }">
                                    <property-search
                                        v-model="model.propertyItems"
                                        :errorMessages="errors">
                                    </property-search>
                                </ValidationProvider>
                            </v-col>
                        </v-card-text>
                    </v-card>
                </v-col>
            </v-row>
            <v-row
                align="center"
                justify="space-around">
                <v-btn
                    :disabled="!this.model.propertyItems || this.model.propertyItems.length == 0"
                    color="primary"
                    @click="save">
                    {{ $t('RequestForCertificateOfDistraintOfProperty.makeRequest') }}
                </v-btn>
            </v-row>
        </div>
    </ValidationObserver>
</template>

<script lang="ts">
    import { Component, Vue, Watch, Ref } from "vue-property-decorator";
    import { Nomenclature } from "@/models/nomenclature";
    import RequestForCertificateOfDistraintOfPropertyModel from "@/models/requestForCertificateOfDistraintOfProperty";
    import { Request } from "@/models/request";
    import AddressModel from "@/models/address";
    import nomenclatureService from "@/services/nomenclature.service";
    import VueI18n from "vue-i18n";
    import { AuthType, UserType, IdentificationType } from "@/models/enums";
    import PropertySearch from "@/components/properties/searchProperty.vue";
    import PersonComponent from "@/components/person/person.vue";
    import CompanyComponent from "@/components/company/company.vue";
    import axios from 'axios';
    import { ValidationObserver, ValidationProvider } from "vee-validate";
    import { namespace } from "vuex-class";
    const userNS = namespace("user");

    @Component({
        components: {
            PropertySearch,
            ValidationProvider,
            ValidationObserver,
            PersonComponent,
            CompanyComponent
        },
    })

    export default class RequestForCertificateOfDistraintOfProperty extends Vue {
        model: RequestForCertificateOfDistraintOfPropertyModel;
        @userNS.Getter("getUserId") userId: any;
        @userNS.Getter("getUserType") userType: any;
        @userNS.Getter("getAuthType") authType: any;

        constructor() {
            super();
            this.model = new RequestForCertificateOfDistraintOfPropertyModel();
        }

        private userTypes: object[] = [];
        private isPerson = false;
        private isPersonDataFilled = false;

        async fetchUserTypes() {
            try {
                const result = await nomenclatureService.getUserTypes();
                this.userTypes = result;
            } catch (error) {
                console.log("Error on getting user types: " + error);
            }
        }

        async fetchPerson() {
            axios.get('/api/person/GetPersonByUserId/' + this.userId)
                .then((response) => {
                    console.log(response);
                    this.model.requester = response.data;
                    this.model.requester.address = new AddressModel();
                }, (error) => {
                    console.log(error);
                });
        }

        async save(): Promise<void> {
            let isPersonalIdentifierTypeLNCh = null;
            if (this.model.requester.identificationType == IdentificationType.LN4) {
                isPersonalIdentifierTypeLNCh = true;
            } else if (this.model.requester.identificationType == IdentificationType.EGN) {
                isPersonalIdentifierTypeLNCh = false;
            }
            const request = new Request(
                {
                    firstName: this.model.requester.firstName,
                    middleName: this.model.requester.middleName,
                    lastName: this.model.requester.lastName,
                    personalIdentifier: this.model.requester.identificationNumber,
                    regionId: this.model.requester.address!.regionId,
                    municipalityId: this.model.requester.address!.municipalityId,
                    cityId: this.model.requester.address!.cityId,
                    streetAddress: this.model.requester.address!.streetAddress,
                    phoneNumber: this.model.requester.phone,
                    email: this.model.requester.email,
                    userTypeCode: this.model.inTheQualityOfPersonTypeCode,
                    nameOfLegalEntity: this.model.requesterCompany.name,
                    identifierOfLegalEntity: this.model.requesterCompany.eik,
                    regionIdOfLegalEntity: this.model.requesterCompany.address!.regionId,
                    municipalityIdOfLegalEntity: this.model.requesterCompany.address!.municipalityId,
                    cityIdOfLegalEntity: this.model.requesterCompany.address!.cityId,
                    streetAddressOfLegalEntity: this.model.requesterCompany.address!.streetAddress,
                    companyCaseNumber: this.model.requesterCompany.companyCaseNumber,
                    propertyTypeCode: this.model.propertyItems[0].propertyTypeCode,
                    propertyId: this.model.propertyItems[0].id,
                    isPersonalIdentifierTypeLNCh: isPersonalIdentifierTypeLNCh
                }
            );

            (this.$refs.validationObserver as InstanceType<typeof ValidationObserver>).validate().then((success) => {
                if (success) {
                    axios.post('/api/requestForCertificateOfDistraintOfProperty/create', request)
                        .then((response) => {
                            if (response.data.message) {
                                // Има съобщение от регистрацията на ел.плащане
                                console.log(response.data.message);
                                this.$router.push("/");
                            } else {
                                if (this.userId) {
                                    this.$router.push("list");
                                } else {
                                    this.$router.push("/");
                                }  
                            }
  
                            console.log(response);
                        }, (error) => {
                            console.log(error);
                        });
                }
            });
        }

        mounted() {
            this.model.requester.address = new AddressModel();
            if (this.userId) { // logged in
                this.fetchUserTypes();
                this.model.inTheQualityOfPersonTypeCode = this.userType;
                if (this.userType == UserType.AUCPAR && this.authType == AuthType.USER) {
                    this.isPerson = true;
                    this.isPersonDataFilled = true;
                    this.fetchPerson();
                }
            } else {
                this.isPerson = true;
                this.isPersonDataFilled = false;
                const requesterType = new Nomenclature({
                    code: "REQUESTER",
                    deactivated: false,
                    name: "Заявител",
                    nameEn: "Requester"
                });
                this.userTypes.push(requesterType);
                this.model.inTheQualityOfPersonTypeCode = requesterType.code;
            }
        }
    }
</script>

<style scoped>
    .disabled {
        pointer-events: none;
    }
</style>
