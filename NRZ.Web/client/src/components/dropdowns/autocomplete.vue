<template>
  <div>
    <ValidationProvider
      v-slot="{ errors }"
      :name="label"
      :rules="required ? 'required' : ''"
    >
      <v-autocomplete
        :disabled="disabled"
        v-model="model"
        :items="filteredItems"
        :loading="isLoading"
        :search-input="search"
        clearable
        :item-text="text"
        :item-value="id"
        :label="label"
        :required="required"
        :error-messages="errors"
        :class="required ? 'required' : ''"
        @change="onChange"
      ></v-autocomplete>
    </ValidationProvider>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Watch, Prop } from "vue-property-decorator";
import Http from "@/services/http.service";
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { IAutocompleteComponentProp } from "@/interfaces/common";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
  },
})
export default class AutocompleteComponent extends Vue {
  @Prop({ default: false })
  required!: boolean;
  @Prop({ default: false })
  disabled!: boolean;
  @Prop()
  url: string | undefined;
  @Prop()
  label: string | undefined;
  @Prop({ default: "id" })
  id!: string;
  @Prop({ default: "name" })
  text!: string;
  @Prop()
  items!: any[];
  @Prop({ default: null })
  value!: string | number | null;
  @Prop({ default: null })
  additionalParams!: Array<IAutocompleteComponentProp> | null;

  model: string | number | null = "";
  search = "";
  isLoading = false;
  filteredItems: any[] = [];
  isInitialLoad = true;

  loadItems() {
    this.isLoading = true;

    if (this.url) {
      let query = "";

      if (this.additionalParams) {
        this.additionalParams.forEach((x) => {
          query += `&${x.name}=${x.value || ""}`;
        });
      }

      Http.get(`${this.url}?${query}`)
        .then((res) => res.data)
        .then((data) => {
          this.filteredItems = data;
          console.log("autocomplete change: ", this.model, data);
          const exist = this.filteredItems.some((x) => x.id == this.model);
          console.log(exist);

          if (!exist) {
            if (!this.isInitialLoad) {
              this.model = null;
              this.$emit("input", this.model);
            }
          }

          this.isInitialLoad = false;
        })
        .catch((err) => console.log(err))
        .then(() => {
          this.isLoading = false;
        });
    }
    this.isLoading = false;
  }

  onChange() {
    this.$emit("input", this.model);
  }

  @Watch("additionalParams", { immediate: true })
  onAdditionalParamsChange() {
    this.filteredItems = [];
    this.loadItems();
  }

  @Watch("value", { immediate: true })
  onValueChange() {
    this.model = this.value;
    this.onChange();
  }

  mounted() {
    if (this.value) {
      this.model = this.value;
    }

    if (!this.url) {
      this.filteredItems = [...this.items];
    } else {
      this.loadItems();
    }
  }
}
</script>

<style>
</style>
