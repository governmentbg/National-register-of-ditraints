<template>
    <div>
        <v-data-table
                      :headers="resultListHeaders"
                      :items="resultItems"
                      :items-per-page="10">
            <template v-slot:item.controls="props">
                <v-btn x-small color="primary" text depressed @click="showReport(props.item)">
                    {{ $t('seizedPropertyAvailabilityRequest.report') }}
                </v-btn>
            </template>
        </v-data-table>

        <report-dialog :show="showReport" :report="report"></report-dialog>
    </div>
</template>

<script>
import { Vue, Component, Prop } from "vue-property-decorator";
import http from "../../services/http.service";
import SeizedPropertyAvailabilityRequestReport from "../../models/seizedPropertyAvailabilityRequestReport";
import ReportDialog from "../../components/seizedPropertyAvailability/report/reportDialog";

@Component({
    components: {
        ReportDialog
    }
})
export default class Vehicle extends Vue {
    @Prop({required: true}) id;

    resultItems = [];
    resultListHeaders = [];
    report = null;

    constructor() {
        super();

        // TODO: use $t for headers text
        this.resultListHeaders = [
            { text: 'distraintId', align: 'start', sortable: true, value: "distraint" },
            { text: 'Име', align: 'start', sortable: true, value: "name" },
            { text: 'Тип запорирано имущество', sortable: true, value: 'propertyTypeName' },
            { text: 'Статус', sortable: true, value: 'statusName' },
            { text: '', value: 'controls', sortable: false }
        ];
    }

    async loadData() {
        try {
            debugger;
            const result = await http.get(`/api/SeizedPropertyAvailabilityRequest/GetPaidReport/${this.id}`);
            // TODO: if is not paid show dura bura
            debugger;
            console.log(result.data);
            if (result.data) {
                this.resultItems = result.data;
            }
        } catch (error) {
            console.log(error);
            // TODO:
        }
    }

    async showReport(item) {
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

    mounted() {
        this.loadData();
    }
}
</script>
