<template>
    <v-row>
        <v-row>
            <v-col cols="4">
                <v-text-field :readonly="true" :label="$t('propertiesList.properties.type')" :value="realEstate.type"></v-text-field>
            </v-col>
            <v-col cols="4">
                <v-text-field :readonly="true" :label="$t('propertiesList.properties.floor')" :value="realEstate.floor"></v-text-field>
            </v-col>
            <v-col cols="4">
                <v-text-field :readonly="true" :label="$t('propertiesList.properties.area')" :value="realEstate.area"></v-text-field>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="4">
                <v-text-field :readonly="true" :label="$t('propertiesList.properties.construction')" :value="realEstate.propertyConstructionType"></v-text-field>
            </v-col>
            <v-col cols="4">
                <v-text-field :readonly="true" :label="$t('propertiesList.properties.identifier')" :value="realEstate.identifier"></v-text-field>
            </v-col>
            <v-col cols="4">
                <v-text-field :readonly="true" :label="$t('propertiesList.properties.identifierType')" :value="realEstate.identifierType"></v-text-field>
            </v-col>
        </v-row>
        <v-row>
            <v-textarea :readonly="true" :label="$t('propertiesList.properties.description')" :value="realEstate.description"></v-textarea>
        </v-row>
    </v-row>
</template>

<script>
import { Vue, Component, Prop } from "vue-property-decorator";
import RealEstateModel from "../../../models/seizedAvailabiityReport/realestate";
import http from "../../../services/http.service";

@Component({

})
export default class Vehicle extends Vue {
    @Prop({ required: true }) distraintId;

    realEstate = new RealEstateModel();

    async loadData() {
        try {
            const result = await http.get(`/api/SeizedPropertyAvailabilityRequest/GetRealEstateInfo/${this.distraintId}`);
            this.realEstate = new RealEstateModel(result.data);
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
