<template>
  <div class="otherPropertyEdit">
    <div>
      <ValidationObserver ref="validator">
        <v-row>
          <v-col cols="12" sm="6">
            <ValidationProvider
              v-slot="{ errors }"
              :name="$t('propertiesList.otherProperties.identifier')"
              rules="required"
            >
              <v-text-field
                v-model="localItem.identifier"
                :label="$t('propertiesList.otherProperties.identifier')"
                :error-messages="errors"
                single-line
                class="required"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" sm="6">
            <ValidationProvider
              v-slot="{ errors }"
              :name="$t('propertiesList.otherProperties.description')"
              rules="required"
            >
              <v-text-field
                v-model="localItem.description"
                :label="$t('propertiesList.otherProperties.description')"
                :error-messages="errors"
                class="required"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>
      </ValidationObserver>
    </div>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { Component, Vue, Prop, Ref } from "vue-property-decorator";
import { OtherPropertyModel } from "@/models/property";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
  },
})
export default class OtherPropertyEditCard extends Vue {
  @Prop({ required: true }) item: any;
  @Ref("validator")
  private validator: any;

  constructor() {
    super();
  }

  get localItem() {
    return this.item || new OtherPropertyModel();
  }

  public async validateInput() {
    const formValid = await this.validator.validate();
    return formValid;
  }
}
</script>