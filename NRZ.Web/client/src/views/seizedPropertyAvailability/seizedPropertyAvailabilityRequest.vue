<template>
    <div v-if="loading">
        <v-progress-linear
            :active="loading"
            color="primary"
            :indeterminate="true"
            class="ma-0"
            height="4"
            style="top: -2px;"
        ></v-progress-linear>
    </div>
    <div v-else>
        <div class="mb-3 text-center">
            <h3>{{ $t("seizedPropertyAvailabilityRequest.title") }}</h3>
        </div>
        <ValidationObserver ref="validationObserver" v-if="showSearch">
            <div>
                <v-row :justify="'center'">
                    <v-col cols="12">
                        <v-card>
                            <v-card-title>{{ $t('seizedPropertyAvailabilityRequest.requesterPerson') }}</v-card-title>
                            <v-card-text>
                                <person-component :person="model.requester"
                                                  :disabled="isAuthenticated == true"
                                                  :show-address="isAuthenticated == false"></person-component> <!-- Requester -->
                                <v-row>
                                    <v-col cols="12">
                                        <ValidationProvider v-slot="{ errors }" rules="required" :name="$t('seizedPropertyAvailabilityRequest.inTheQualityOf')">
                                            <v-select
                                                class="required"
                                                v-model="model.inTheQualityOfPersonTypeCode"
                                                :items="requesterTypes"
                                                :item-text="'name'"
                                                :name="'inTheQualityOf' + _uid"
                                                :item-value="'code'"
                                                :error-messages="errors"
                                                :disabled="isAuthenticated === false"
                                                :label="$t('seizedPropertyAvailabilityRequest.inTheQualityOf')"
                                            ></v-select> <!-- В качеството на -->
                                        </ValidationProvider>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="4">
                                            <v-text-field
                                                :label="`${$t('company.representativeOf')}`"
                                                v-model="model.requesterCompanyRepresentative"
                                            ></v-text-field>
                                    </v-col>
                                    <v-col cols="4">
                                            <v-text-field
                                                :label="`${$t('company.eik')}`"
                                                v-model="model.requesterCompanyEik"
                                            ></v-text-field>
                                    </v-col>
                                    <v-col cols="4">
                                        <v-text-field
                                            :label="$t('company.companyCaseNumber')"
                                            v-model="model.requesterCompanyCaseNumber"
                                        ></v-text-field>
                                    </v-col>
                                </v-row>
                            </v-card-text>
                        </v-card>
                    </v-col>
                    <v-col cols="12">
                        <v-card>
                            <v-card-title>{{ $t('seizedPropertyAvailabilityRequest.checkedPerson') }}</v-card-title>
                            <v-card-text>
                                <div>
                                    <v-checkbox v-model="model.isCheckedPerson" :label="$t('person.physicalPerson')"></v-checkbox>
                                </div>
                                <person-component v-if="model.isCheckedPerson"
                                                  :person="model.checkedPerson"
                                                  :is-address-required="false"
                                                  :is-email-required="false"></person-component> <!-- CheckedPerson -->

                                <company-component v-else
                                                   :is-address-required="false"
                                                   :company="model.checkedCompany"></company-component>
                            </v-card-text>
                        </v-card>
                    </v-col>
                </v-row>
                <v-row align="center"
                       justify="space-around">
                    <v-btn color="primary" @click="searchSeizedProperty">{{ $t('seizedPropertyAvailabilityRequest.search') }}</v-btn>
                </v-row>
            </div>
        </ValidationObserver>
        <div v-else>
            <v-btn color="primary" @click="newSearch">{{ $t('seizedPropertyAvailabilityRequest.newSearch') }}</v-btn>

            <v-alert class="my-5" v-if="waitingForPayment" :type="waitingForPaymentAlertColor" outlined>{{ waitingForPaymentMessage }}</v-alert>
            <v-data-table v-else
                          :headers="resultListHeaders"
                          :items="resultItems"
                          :items-per-page="10">
                <template v-slot:item.controls="props">
                    <v-btn x-small color="primary" text depressed @click="showReport(props.item)">
                        {{ $t('seizedPropertyAvailabilityRequest.report') }}
                    </v-btn>
                </template>
            </v-data-table>
        </div>

        <report-dialog :show="showReport" :report="report"></report-dialog>
    </div>
</template>
`
<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import Address from "@/components/address/address.vue";
import PersonModel from '@/models/person';
import PersonComponent from "@/components/person/person.vue";
import CompanyComponent from "@/components/company/company.vue";
import SeizedPropertyAvailabilityRequestModel from "@/models/seizedPropertyAvailabilityRequest";
import { ValidationObserver, ValidationProvider } from "vee-validate";
import http from "@/services/http.service";
import seizedPropertyAvailabilityRequest from "@/models/seizedPropertyAvailabilityRequest";
import CompanyModel from "@/models/company";
import SeizedPropertyAvailabilityRequestReport from "@/models/seizedPropertyAvailabilityRequestReport";
import { RequesterType } from "@/models/enums";
import ReportDialog from "@/components/seizedPropertyAvailability/report/reportDialog.vue";

@Component({
    components: {
        ReportDialog,
        Address,
        PersonComponent,
        CompanyComponent,
        ValidationProvider,
        ValidationObserver,
    }
})
export default class SeizedPropertyAvailabilityRequest extends Vue {
    model: SeizedPropertyAvailabilityRequestModel;
    requesterTypes: [];
    resultItems: any[] = [];
    showSearch = true;
    loading = false;
    resultListHeaders: any[] = [];
    isAuthenticated = false;
    showReportDialog = false;
    report: SeizedPropertyAvailabilityRequestReport | null = null;
    waitingForPayment = false;
    waitingForPaymentMessage = '';
    waitingForPaymentAlertColor = 'success';

    constructor() {
        super();
        this.isAuthenticated = this.$store.getters['user/isAuthenticated'] || false;
        this.model = new SeizedPropertyAvailabilityRequestModel();
        this.requesterTypes = []; // typescript and tslint sucks...
        this.loadUserTypes();

        // TODO: use $t for headers text
        this.resultListHeaders = [
            { text: 'distraintId', align: 'start', sortable: true, value: "distraint" },
            { text: 'Име', align: 'start', sortable: true, value: "name" },
            { text: 'Тип запорирано имущество', sortable: true, value: 'propertyTypeName' },
            { text: 'Статус', sortable: true, value: 'statusName' },
            { text: '', value: 'controls', sortable: false }
        ];
    }

    async loadUserTypes(): Promise<any> {
        try {
            const result = await http.get('/api/Nomenclature/RequesterTypes');
            this.requesterTypes = result.data;
        } catch (error) {
            // TODO:
            console.error(error);
        }
    }

    searchSeizedProperty(): void {
        (this.$refs.validationObserver as InstanceType<typeof ValidationObserver>).validate().then((success) => {
            if (success) {
                this.loading = true;
                this.model.requester.address = null;
                http.post('/api/SeizedPropertyAvailabilityRequest/', this.model).then((result) => {
                    if (result.data.message) {
                        this.waitingForPayment = true;
                        this.waitingForPaymentAlertColor = result.data.isSuccessfull ? 'success' : 'error';
                        this.waitingForPaymentMessage = result.data.message;
                    } else {
                        this.resultItems = result.data.items;
                    }
                    this.showSearch = false;
                    this.loading = false;
                }).catch((error) => {
                    console.error(error);
                    this.loading = false;
                    // TODO:
                });
            }
        });
    }

    async showReport(item: any) {
        try {
            const result = await http.get(`/api/SeizedPropertyAvailabilityRequest/Report/${item.distraintId}`);
            this.report = new SeizedPropertyAvailabilityRequestReport(result.data);
            this.report.distraintId = item.distraintId;
            this.showReportDialog = true;
        } catch (error) {
            // TODO:
            console.error(error);
        }
    }

    newSearch() {
        this.model = new seizedPropertyAvailabilityRequest({ requester: this.model.requester });
        this.model.requester.address = null;
        this.waitingForPayment = false;
        this.waitingForPaymentMessage = '';
        this.showReportDialog = false;
        this.showSearch = true;
    }

    getRequesterPersonData() {
        if (this.isAuthenticated) {
            const userId = this.$store.getters["user/getUserId"];
            http.get(`/api/Person/GetPersonByUserId/${userId}`).then(result => {
                this.model.requester = new PersonModel(result.data);
                this.model.requester.address = null;
                const userType = this.$store.getters['user/getUserType'];
                this.model.inTheQualityOfPersonTypeCode = userType;
            }).catch(error => {
                console.error(error);
                // TODO:
            });
        } else {
            this.model = new seizedPropertyAvailabilityRequest();
            this.model.inTheQualityOfPersonTypeCode = RequesterType.APPLICANT;
        }
    }

    setupCheckedPerson() {
        if (this.model.isCheckedPerson) {
            this.model.checkedCompany = undefined;
            this.model.checkedPerson = new PersonModel();
        } else {
            this.model.checkedPerson = undefined;
            this.model.checkedCompany = new CompanyModel();
        }
    }

    @Watch('model.isCheckedPerson')
    onShowAddressComponentChange() {
        this.setupCheckedPerson();
    }

    mounted() {
        this.getRequesterPersonData();
    }
}
</script>

<style scoped>
/*@media print {
    .reportToPrint {
        background-color: white;
        height: 100%;
        width: 100%;
        position: fixed;
        top: 0;
        left: 0;
        margin: 0;
        padding: 15px;
        font-size: 14px;
        line-height: 18px;
    }
}*/
</style>
