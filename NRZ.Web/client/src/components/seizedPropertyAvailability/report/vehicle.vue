<template>
    <v-row>
        <v-row>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vehicles.vehicleType')" :value="vehicle.vehicleType"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vehicles.model')" :value="vehicle.model"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vehicles.color')" :value="vehicle.color"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vehicles.category')" :value="vehicle.category"></v-text-field>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vehicles.registrationNumber')" :value="vehicle.registrationNumber"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vehicles.firstRegistrationDate')" :value="vehicle.firstRegistrationDate"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vehicles.vin')" :value="vehicle.vin"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="`${$t('propertiesList.vehicles.vehicleDocumentNumber')} / ${$t('propertiesList.vehicles.vehicleDocumentDate')}`" :value="`${vehicle.vehicleDocumentNumber} / ${vehicle.vehicleDocumentDate}`"></v-text-field>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vehicles.maxPower')" :value="vehicle.maxPower"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vehicles.capacity')" :value="vehicle.capacity"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vehicles.fuel')" :value="vehicle.fuel"></v-text-field>
            </v-col>
            <v-col cols="3">
                <v-text-field :readonly="true" :label="$t('propertiesList.vehicles.environmentalCategory')" :value="vehicle.environmentalCategory"></v-text-field>
            </v-col>
        </v-row>
    </v-row>
</template>

<script>
import { Vue, Component, Prop } from "vue-property-decorator";
import VehicleModel from "../../../models/seizedAvailabiityReport/vehicle";
import http from "../../../services/http.service";

@Component({

})
export default class Vehicle extends Vue {
    @Prop({ required: true }) distraintId;

    vehicle = new VehicleModel();

    async loadData() {
        try {
            const result = await http.get(`/api/SeizedPropertyAvailabilityRequest/GetVehicleInfo/${this.distraintId}`);
            // TODO: format dates
            this.vehicle = new VehicleModel(result.data);
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
