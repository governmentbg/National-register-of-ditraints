<template>
    <v-card>
        <v-card-title>
            <span class="headline">Имущество/вещ</span>
        </v-card-title>
        <v-card-text>
            <ValidationObserver ref="validator">
                <v-container>
                    <v-row>
                        <v-col sm="12" md="6">
                            <ValidationProvider
                                v-slot="{ errors }"
                                :name="$t('propertiesList.otherProperties.name')"
                                rules="required"
                            >
                                <v-text-field
                                    class="required"
                                    :label="$t('propertiesList.otherProperties.name')"
                                    :error-messages="errors"
                                    v-model="model.identifier"
                                ></v-text-field>
                            </ValidationProvider>
                        </v-col>
                        <v-col sm="12" md="6">
                            <ValidationProvider
                                v-slot="{ errors }"
                                :name="$t('propertiesList.otherProperties.type')"
                                rules="required"
                            >
                                <v-select
                                    class="required"
                                    :label="$t('propertiesList.otherProperties.type')"
                                    :error-messages="errors"
                                    v-model="model.type"
                                    :items="assetTypes"
                                    item-value="code"
                                    item-text="name"
                                >
                                </v-select>
                            </ValidationProvider>
                        </v-col>
                        <v-col sm="12" md="12">
                            <div class="v-label pb-3">{{ $t('propertiesList.otherProperties.description') }}</div>
                            <editor
                                api-key="2ap5dccp95hpdv0izfqz5gniixbw769t58f1dldq3o3asabl"
                                v-model="model.description"
                                :init="{
                                    height: 300,
                                    menubar: false,
                                    plugins: [
                                        'advlist autolink lists link image charmap print preview anchor',
                                        'searchreplace visualblocks code fullscreen',
                                        'insertdatetime media table paste code help wordcount',
                                    ],
                                    toolbar:
                                        'undo redo | formatselect | bold italic backcolor | \
                            alignleft aligncenter alignright alignjustify | \
                            bullist numlist outdent indent | removeformat | help',
                                }"
                            />
                        </v-col>
                    </v-row>
                </v-container>
            </ValidationObserver>
        </v-card-text>
        <v-card-actions>
            <v-spacer></v-spacer>
            <slot name="customActions"></slot>
            <v-btn color="blue darken-1" text @click.stop="save">
                {{ $t("buttons.save") }}
            </v-btn>
        </v-card-actions>
    </v-card>
</template>

<script lang="ts">
import { Component, Vue, Ref } from "vue-property-decorator";
import { OtherPropertyModel } from "@/models/property";
import { Nomenclature } from "@/models/nomenclature";
import nomenclatureService from "@/services/nomenclature.service";
import service from "@/services/otherProperty.service";
import { ValidationProvider, ValidationObserver } from "vee-validate";
import Editor from "@tinymce/tinymce-vue";

@Component({
    components: {
        Editor,
        ValidationProvider,
        ValidationObserver,
    },
})
export default class AssetEditor extends Vue {
    @Ref("validator")
    private validator!: InstanceType<typeof ValidationObserver>;

    private model = new OtherPropertyModel();
    private assetTypes: Nomenclature[] = [];

    private async fetchAssetTypes() {
        try {
            const result = await nomenclatureService.getAssetTypes();
            this.assetTypes = result as any;
        } catch (error) {
            console.log("Error on getting asset types: " + error);
        }
    }

    private async validate() {
        const valid = await this.validator.validate();
        return valid;
    }

    private async save() {
        const isValid = await this.validate();
        if (isValid) {
            this.model.isManuallyAdded = true;
            console.log(this.model);
            try {
                const result = await service.create(this.model);
                this.$emit("save", result)
            } catch (error) {
                console.log("Error on create other property: " + error);
            }
        }
    }

    private async mounted() {
        await this.fetchAssetTypes();
    }
}
</script>