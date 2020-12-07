<template>
    <v-row>
        <v-col cols="3">
            <v-text-field :readonly="true" :label="$t('agriculturalMachinery.registrationNumber')" :value="model.registrationNumber"></v-text-field>
        </v-col>
        <v-col cols="3">
            <v-text-field :readonly="true" :label="$t('agriculturalMachinery.frameNumber')" :value="model.frameNumber"></v-text-field>
        </v-col>
        <v-col cols="3">
            <v-text-field :readonly="true" :label="$t('agriculturalMachinery.type')" :value="model.type"></v-text-field>
        </v-col>
        <v-col cols="3">
            <v-text-field :readonly="true" :label="$t('agriculturalMachinery.headers.owner')" :value="model.owner"></v-text-field>
        </v-col>
    </v-row>
</template>

<script>
import { Vue, Component, Prop } from "vue-property-decorator";
import AgriformachineryModel from "../../../models/seizedAvailabiityReport/agriformachinery";
import http from "../../../services/http.service";

@Component({

})
export default class Vehicle extends Vue {
    @Prop({ required: true }) distraintId;

    model = new AgriformachineryModel();

    async loadData() {
        try {
            const result = await http.get(`/api/SeizedPropertyAvailabilityRequest/GetAgriformachineryInfo/${this.distraintId}`);
            this.model = new AgriformachineryModel(result.data);
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
