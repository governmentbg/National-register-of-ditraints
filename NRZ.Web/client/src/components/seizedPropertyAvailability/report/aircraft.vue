<template>
    <v-row>
        <v-row>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="`${$t('propertiesList.aircrafts.producerName')} / ${$t('propertiesList.aircrafts.producerNameEn')}`" :value="`${aircraft.producerName} / ${aircraft.producerNameEn}`"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.aircrafts.producerCountryCode')" :value="aircraft.producerCountryCode"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.aircrafts.producerCountryName')" :value="aircraft.producerCountryName"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.aircrafts.airCategoryCode')" :value="aircraft.airCategoryCode"></v-text-field>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.aircrafts.airCategoryName')" :value="aircraft.airCategoryName"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.aircrafts.icao')" :value="aircraft.icao"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.aircrafts.msnserialNumber')" :value="aircraft.msnSerialNumber"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="`${$t('propertiesList.aircrafts.modelName')} / ${$t('propertiesList.aircrafts.modelNameEn')}`" :value="`${aircraft.modelName} / ${aircraft.modelNameEn}`"></v-text-field>
            </v-col>
        </v-row>
    </v-row>
</template>

<script>
import { Component, Prop, Vue } from "vue-property-decorator";
import http from "../../../services/http.service";
import AircraftModel from "../../../models/seizedAvailabiityReport/aircraft";

@Component({
})
export default class Aircraft extends Vue {
    @Prop({ required: true }) distraintId;

    aircraft = new AircraftModel();

    async loadData() {
         try {
             const result = await http.get(`/api/SeizedPropertyAvailabilityRequest/GetAircraftInfo/${this.distraintId}`);
             this.aircraft = new AircraftModel(result.data);
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
