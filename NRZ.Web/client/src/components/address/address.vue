<template>
    <div class="row">
        <div class="col-md-4 col-12">
            <autocomplete
                :disabled="disabled"
                v-model="value.regionId"
                :url="'/api/nomenclature/regions'"
                :label="'Област'"
                :required="isRequired"
            ></autocomplete>
        </div>

        <div class="col-md-4 col-12">
            <autocomplete
                :disabled="disabled"
                v-model="value.municipalityId"
                :url="'/api/nomenclature/municipalities'"
                :label="'Община'"
                :required="isRequired"
                :additionalParams="municipalityParamsData"
            ></autocomplete>
        </div>
        <div class="col-md-4 col-12">
            <autocomplete
                :disabled="disabled"
                v-model="value.cityId"
                :url="'/api/nomenclature/cities'"
                :label="'Населено място'"
                :required="isRequired"
                :additionalParams="cityParamsData"
            ></autocomplete>
        </div>
        <div class="col-md-12 col-12">
            <ValidationProvider v-slot="{ errors }" :name="'Адрес'" :rules="isRequired ? 'required' : ''">
                <v-text-field
                    :disabled="disabled"
                    v-model="value.streetAddress"
                    :label="'Адрес'"
                    :required="isRequired"
                    :error-messages="errors"
                    :class="isRequired ? 'required' : ''"
                ></v-text-field>
            </ValidationProvider>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Watch, Prop } from "vue-property-decorator";
import autocomplete from "../dropdowns/autocomplete.vue";
import AddressModel from "@/models/address";
import { IAutocompleteComponentProp } from "@/interfaces/common";
import { ValidationProvider } from "vee-validate";

@Component({
    components: {
        autocomplete,
        ValidationProvider,
    },
})
export default class AddressComponent extends Vue {
    @Prop({required: false, default: () => new AddressModel()}) value!: AddressModel;
    //@Prop({required: true}) value!: AddressModel;
    @Prop({default: false}) disabled!: boolean;
    @Prop({default: true}) isRequired!: boolean;
    municipalityParams: Array<IAutocompleteComponentProp> = [];
    cityParams: Array<IAutocompleteComponentProp> = [];

    private municipalityParamsData: Array<IAutocompleteComponentProp> = [];
    private cityParamsData: Array<IAutocompleteComponentProp> = [];

    @Watch("value.regionId", {immediate: true})
    onRegionChange() {
        this.municipalityParamsData = [
            {name: "regionId", value: this.value.regionId},
        ];
    }

    @Watch("value.municipalityId", {immediate: true})
    onMunicipalityChange() {
        this.cityParamsData = [
            {name: "municipalityId", value: this.value.municipalityId},
        ];
    }
}
</script>

<style>
</style>
