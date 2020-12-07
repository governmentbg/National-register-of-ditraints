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
                                :name="$t('propertiesList.properties.type')"
                                rules="required"
                            >
                                <v-select
                                    class="required"
                                    :label="$t('propertiesList.properties.type')"
                                    :error-messages="errors"
                                    v-model="model.type"
                                    :items="realEstateTypes"
                                    item-value="code"
                                    item-text="name"
                                >
                                </v-select>
                            </ValidationProvider>
                        </v-col>

                        <v-col sm="12" md="6">
                          <v-text-field
                              :label="$t('propertiesList.properties.floor')"
                              v-model="model.floor"
                          ></v-text-field>
                        </v-col>

                        <v-col sm="12" md="6">
                            <ValidationProvider
                                v-slot="{ errors }"
                                :name="$t('propertiesList.properties.area')"
                                rules="required"
                            >
                                <v-text-field
                                  type="number"
                                  min="0"
                                  class="required"
                                  :label="$t('propertiesList.properties.area')"
                                  :error-messages="errors"
                                  v-model.number="model.area"
                                ></v-text-field>
                            </ValidationProvider>
                        </v-col>
                        
                        <v-col sm="12" md="6">
                            <v-select
                                :label="$t('propertiesList.properties.construction')"
                                v-model="model.propertyConstructionTypeId"
                                :items="propertyConstructionTypes"
                                item-value="code"
                                item-text="name"
                            >
                            </v-select>
                        </v-col>

                        <v-col sm="12" md="6">
                          <v-text-field
                              :label="$t('propertiesList.properties.identifier')"
                              v-model="model.identifier"
                          ></v-text-field>
                        </v-col>

                        <v-col sm="12" md="6">
                            <v-select
                                :label="$t('propertiesList.properties.identifierType')"
                                v-model="model.identifierType"
                                :items="identifierTypes"
                                item-value="code"
                                item-text="name"
                            >
                            </v-select>
                        </v-col>

                        <v-col sm="12" md="12" v-if="model && model.address">
                            <address-component v-model="model.address" :is-required="false"></address-component>
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
import { Component, Vue, Ref } from 'vue-property-decorator';
import { PropertyModel } from '@/models/property';
import { Nomenclature } from '@/models/nomenclature';
import nomenclatureService from '@/services/nomenclature.service';
import service from '@/services/property.service';
import { ValidationProvider, ValidationObserver } from 'vee-validate';
import Editor from '@tinymce/tinymce-vue';
import AddressComponent from '@/components/address/address.vue';

@Component({
    components: {
        Editor,
        ValidationProvider,
        ValidationObserver,
        AddressComponent
    },
})
export default class PropertyEditor extends Vue {
    @Ref("validator")
    private validator!: InstanceType<typeof ValidationObserver>;

    private model = new PropertyModel();
    private realEstateTypes: Nomenclature[] = [];
    private propertyConstructionTypes: Nomenclature[] = [];
    private identifierTypes: Nomenclature[] = [
      new Nomenclature({ code: '1', name: 'Няма номер', nameEn: 'No number'}),
      new Nomenclature({ code: '2', name: 'Кадастрален номер', nameEn: 'Кадастрален номер'}),
      new Nomenclature({ code: '3', name: 'Планоснимачен номер', nameEn: 'ланоснимачен номер'})]

    private async fetchRealEstateTypes() {
        try {
            const result = await nomenclatureService.getRealEstateTypes();
            this.realEstateTypes = result as any;
        } catch (error) {
            console.log("Error on getting real estate types: " + error);
        }
    }

    private async fetchPropertyConstructionTypes() {
        try {
            const result = await nomenclatureService.getPropertyConstructionType();
            this.propertyConstructionTypes = result as any;
        } catch (error) {
            console.log("Error on getting property construction types: " + error);
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
            if(this.model.address && !this.model.address.regionId) {
              this.model.address = undefined;
            }
            console.log(this.model);
            try {
                const result = await service.create(this.model);
                this.$emit("save", result)
            } catch (error) {
                console.log("Error on create property: " + error);
            }
        }
    }

    private async mounted() {
        await this.fetchRealEstateTypes();
        await this.fetchPropertyConstructionTypes();
    }
}
</script>