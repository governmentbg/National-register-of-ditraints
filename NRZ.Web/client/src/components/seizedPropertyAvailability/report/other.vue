<template>
    <div>
        <v-row>
            <v-col cols="6">
                <v-text-field :readonly="true" :label="$t('propertiesList.otherProperties.identifier')" :value="other.identifier"></v-text-field>
            </v-col>
            <v-col cols="6">
                <v-text-field :readonly="true" :label="$t('propertiesList.otherProperties.type')" :value="other.type"></v-text-field>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="12">
                <v-textarea :readonly="true" :label="$t('propertiesList.otherProperties.description')" :value="other.description"></v-textarea>
            </v-col>
        </v-row>
    </div>
</template>

<script>
import { Vue, Component, Prop } from "vue-property-decorator";
import OtherModel from "../../../models/seizedAvailabiityReport/other";
import http from "../../../services/http.service";

@Component({

})
export default class Vehicle extends Vue {
    @Prop({ required: true }) distraintId;

    other = new OtherModel();

    async loadData() {
        try {
            const result = await http.get(`/api/SeizedPropertyAvailabilityRequest/GetOtherInfo/${this.distraintId}`);
            this.other = new OtherModel(result.data);
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
