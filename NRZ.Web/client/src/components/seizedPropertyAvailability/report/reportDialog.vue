<template>
    <div class="text-center" v-if="report">
        <v-dialog v-model="show">
            <v-card class="reportToPrint">
                <v-card-title>Справка</v-card-title>
                <v-card-text>
                    <div v-if="report.propertyType">
                        <component :is="report.propertyTypeCode" :distraint-id="report.distraintId"></component> <!-- TODO: -->
                    </div>
                    <v-row>
                        <v-text-field :label="$t('distraints.list.debtor')" :readonly="true" :value="report.debtorName"></v-text-field>
                    </v-row>
                    <v-row>
                        <v-text-field :label="$t('createDistraint.propertyTypeLabel')" :readonly="true" :value="report.propertyType"></v-text-field>
                    </v-row>
                    <v-row>
                        <v-text-field :label="$t('distraints.list.enforcementDate')" :readonly="true" :value="report.enforcementDate"></v-text-field>
                    </v-row>
                    <v-row>
                        <v-text-field :label="$t('distraints.list.createdByUserType')" :readonly="true" :value="report.enforcementBy"></v-text-field>
                    </v-row>
                    <v-row>
                        <v-text-field :label="$t('distraints.list.enforcedAt')" :readonly="true" :value="report.enforcedAt"></v-text-field>
                    </v-row>
                    <v-row>
                        <v-text-field :label="$t('distraints.list.inFavourOf')" :readonly="true" :value="report.inFavorOf"></v-text-field>
                    </v-row>
                </v-card-text>
                <v-divider></v-divider>
                <v-card-actions>
                    <v-btn color="primary" @click="printReport">Разпечатай</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </div>
</template>

<script>
import { Vue, Component, Prop } from "vue-property-decorator";
import Aircraft from './aircraft';
import Vehicle from "./vehicle";
import Vessel from "./vessel";
import Other from './other';
import Realestate from './realestate';
import Agriformachinery from './agriformachinery';

@Component({
    components: {
        Aircraft,
        Vehicle,
        Vessel,
        Other,
        Realestate,
        Agriformachinery
    }
})
export default class ReportDialog extends Vue {
    @Prop({required: true}) show;
    @Prop({required: true}) report;//!: SeizedPropertyAvailabilityRequestReport;

    constructor() {
        super();
    }

    printReport() {
        window.print();
    }
}
</script>

<style scoped>
@media print {
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
}
</style>
