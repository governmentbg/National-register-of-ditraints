<template>
    <v-row>
        <v-row>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vessels.bt')" :value="vessel.bt"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vessels.nt')" :value="vessel.nt"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vessels.maxLength')" :value="vessel.maxLength"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vessels.lengthBetweenPerpendiculars')" :value="vessel.lengthBetweenPerpendiculars"></v-text-field>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vessels.maxWidth')" :value="vessel.maxWidth"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vessels.waterplane')" :value="vessel.waterplane"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vessels.shipboardHeight')" :value="vessel.shipboardHeight"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vessels.deadweight')" :value="vessel.deadWeight"></v-text-field>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vessels.numberOfEngines')" :value="vessel.numberOfEngines"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vessels.enginesFuel')" :value="vessel.enginesFuel"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vessels.sumEnginePower')" :value="vessel.sumEnginePower"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vessels.bodyNumber')" :value="vessel.bodyNumber"></v-text-field>
            </v-col>
        </v-row>
    </v-row>
</template>

<script>
import { Vue, Component, Prop } from "vue-property-decorator";
import VesselModel from "../../../models/seizedAvailabiityReport/vessel";
import http from "../../../services/http.service";

@Component({

})
export default class Vehicle extends Vue {
    @Prop({ required: true }) distraintId;

    vessel = new VesselModel();

    async loadData() {
        try {
            const result = await http.get(`/api/SeizedPropertyAvailabilityRequest/GetVesselInfo/${this.distraintId}`);
            // TODO: format dates
            this.vessel = new VesselModel(result.data);
        } catch (error) {
            console.log(error);
            // TODO:
        }
    }

    mounted() {
        this.loadData();
    }
}
</script>
