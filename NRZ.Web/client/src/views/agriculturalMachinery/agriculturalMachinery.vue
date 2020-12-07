<template>
  <div>
    <v-card>
      <v-card-title primary-title class="justify-center">
        <h2>{{ $t("agriculturalMachinery.title") }}</h2>
      </v-card-title>
      <v-card-title>
        <div id="createSnackbar" class="text-center" v-show="createSuccess">
          <v-snackbar
            color="success"
            outlined
            top
            right
            absolute
            v-model="createSuccess"
          >
            {{ $t("agriculturalMachinery.messages.successfullyCreated") }}
          </v-snackbar>
        </div>
        <div id="editSnackbar" class="text-center" v-show="editSuccess">
          <v-snackbar
            color="success"
            outlined
            top
            right
            absolute
            v-model="editSuccess"
          >
            {{ $t("agriculturalMachinery.messages.successfullyEdited") }}
          </v-snackbar>
        </div>
        <div id="deleteSnackbar" class="text-center" v-show="deleteSuccess">
          <v-snackbar
            color="success"
            outlined
            top
            right
            absolute
            v-model="deleteSuccess"
          >
            {{ $t('agriculturalMachinery.messages.successfullyDeleted')}}

          </v-snackbar>
        </div>
      </v-card-title>
      <v-row>
        <v-col :md="6">
          <v-dialog v-model="dialog" persistent max-width="800px">
            <template v-slot:activator="{ on, attrs }">
              <div style="margin-left: 10px">
                <v-btn color="primary" dark v-bind="attrs" v-on="on" mg>
                  {{ $t("agriculturalMachinery.createMachine") }}
                </v-btn>
              </div>
            </template>
            <v-card>
              <ValidationObserver ref="form" v-slot="{ invalid }">
                <v-card-title>
                  <span class="headline">{{
                    $t("agriculturalMachinery.registerMachine")
                  }}</span>
                </v-card-title>
                <v-card-text>
                  <v-container>
                    <v-row>
                      <v-col cols="12">
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                          :name="$t('agriculturalMachinery.registrationNumber')"
                        >
                          <v-text-field
                            v-model="model.registrationNumber"
                            :label="
                              $t('agriculturalMachinery.registrationNumber')
                            "
                            :error-messages="errors"
                          ></v-text-field>
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12">
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                          :name="$t('agriculturalMachinery.frameNumber')"
                        >
                          <v-text-field
                            v-model="model.frameNumber"
                            :label="$t('agriculturalMachinery.frameNumber')"
                            :required="true"
                            :error-messages="errors"
                          ></v-text-field>
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12">
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                          :name="$t('agriculturalMachinery.type')"
                        >
                          <v-text-field
                            v-model="model.type"
                            :label="$t('agriculturalMachinery.type')"
                            :required="true"
                            :error-messages="errors"
                          >
                          </v-text-field>
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12">
                        <v-select
                          :items="[$t('person.person'), $t('company.company')]"
                          :label="$t('agriculturalMachinery.typeOfOwner')"
                          v-model="chosenTypeOfOwner"
                        ></v-select>
                      </v-col>
                    </v-row>
                  </v-container>
                  <div v-if="chosenTypeOfOwner == $t('person.person')">
                    <span class="headline" style="color: black">
                      {{ $t("agriculturalMachinery.registerPerson") }}
                    </span>
                    <person :person="model.person"></person>
                  </div>
                  <div v-if="chosenTypeOfOwner == $t('company.company')">
                    <span class="headline" style="color: black">
                      {{ $t("agriculturalMachinery.registerCompany") }}
                    </span>
                    <company :company="model.company"></company>
                  </div>
                </v-card-text>
                <div class="center">
                  <error-message
                    :show="showError"
                    :messages="errorMessages"
                  ></error-message>
                </div>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="blue darken-1" text @click="ClearInput()">
                    {{ $t("agriculturalMachinery.close") }}
                  </v-btn>
                  <v-btn
                    color="blue darken-1"
                    text
                    :disabled="invalid || !chosenTypeOfOwner"
                    @click="createMachinery(), (dialog = false)"
                  >
                    {{ $t("agriculturalMachinery.create") }}
                  </v-btn>
                </v-card-actions>
              </ValidationObserver>
            </v-card>
          </v-dialog>
          <v-dialog v-model="showEdit" width="600">
            <edit-agricultural-machine
              :itemId="itemId"
              v-on:saved-edit="savedEdit($event)"
              v-on:close-edit="closeEdit($event)"
            ></edit-agricultural-machine>
          </v-dialog>
        </v-col>
        <v-col :md="6">
          <div style="margin-right: 10px">
            <v-text-field
              v-model="search"
              append-icon="mdi-magnify"
              :label="$t('buttons.search_v.text')"
              single-line
              hide-details
            ></v-text-field>
          </div>
        </v-col>
      </v-row>
      <v-row>
        <v-col md="12">
          <v-data-table
            :headers="headers"
            :items="machineries"
            :options.sync="options"
            :page.sync="page"
            :search="search"
            :footer-props="{
              itemsPerPageOptions: itemsPerPageOptions,
            }"
            :server-items-length="totalItems"
          >
            <template v-for="header in headers">
              {{
                (header.text = $t(
                  "agriculturalMachinery.headers." + header.value
                ))
              }}
            </template>
            <template v-slot:item="{ item }">
              <tr>
                <td>{{ item.registrationNumber }}</td>
                <td>{{ item.frameNumber }}</td>
                <td>{{ item.type }}</td>
                <td>{{ item.owner }}</td>
                <td>
                  <v-row>
                    <v-col :md="1">
                      <view-agricultural-machine :itemId="item.id">
                      </view-agricultural-machine>
                    </v-col>
                    <v-col :md="1">
                      <div id="editModal" class="text-center">
                        <v-icon @click="openEditModal(item.id)" small>
                          mdi-pencil
                        </v-icon>
                      </div>
                    </v-col>
                    <v-col :md="1">
                      <delete-agricultural-machine
                        :itemId="item.id"
                        v-on:saved-delete="savedDelete($event)"
                      >
                      </delete-agricultural-machine>
                    </v-col>
                  </v-row>
                </td>
              </tr>
            </template>
          </v-data-table>
        </v-col>
      </v-row>
    </v-card>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import data from "@/views/agriculturalMachinery/data.json";
import httpService from "@/services/http.service";
import AgriculturalMachineryCreationModel from "@/models/agriculturalMachinery";
import Person from "@/models/person";
import person from "@/components/person/person.vue";
import Company from "@/models/company";
import company from "@/components/company/company.vue";
import ErrorMessage from "@/components/common/errorMessage.vue";
import { ValidationObserver, ValidationProvider } from "vee-validate";
import VueI18n, { TranslateResult } from "vue-i18n";
import ViewAgriculturalMachine from "@/views/agriculturalMachinery/viewAgriculturalMachine.vue";
import AgriculturalMachinerySearch from "@/models/agriculturalMachinerySearch";
import EditAgriculturalMachine from "@/views/agriculturalMachinery/editAgriculturalMachinery.vue";
import DeleteAgriculturalMachine from "@/views/agriculturalMachinery/deleteAgriculturalMachine.vue";

@Component({
  components: {
    person,
    company,
    ErrorMessage,
    ValidationProvider,
    ValidationObserver,
    ViewAgriculturalMachine,
    EditAgriculturalMachine,
    DeleteAgriculturalMachine,
  },
})
export default class AgriculturalMachinery extends Vue {
  private headers: Array<object> = data;
  private itemsPerPageOptions: Array<number> = [5, 10, 15];
  private options: object;
  private page: number;
  private totalItems: number;
  private search: string;
  private dialog: boolean;
  private machineries: Array<object>;
  private chosenTypeOfOwner: string;
  private errorMessages: [VueI18n.TranslateResult] = [""];
  private showError = false;
  private showEdit = false;
  private createSuccess = false;
  private editSuccess = false;
  private deleteSuccess = false;
  private timeoutQuery: number;
  private itemId: number;

  private model: AgriculturalMachineryCreationModel;

  constructor() {
    super();
    this.model = new AgriculturalMachineryCreationModel({});
    this.machineries = [];

    this.totalItems = 5;
    this.search = "";
    this.options = { page: 1, itemsPerPage: 5 };
    this.page = 1;
    this.dialog = false;
    this.chosenTypeOfOwner = "";
    this.timeoutQuery = 0;
    this.itemId = 0;
  }

  async createMachinery() {
    try {
      await httpService.post("/api/AgriculturalMachinery/Create", this.model);

      this.ClearInput();

      this.createSuccess = true;
      this.GetAllMachineries(this.options);
    } catch (err) {
      this.setErrorMessage(err);
      console.log(err);
      this.dialog = true;
    }
  }

  async GetAllMachineries(options: any) {
    try {
      const searchModel = new AgriculturalMachinerySearch({});
      searchModel.page = options.page;
      searchModel.itemsPerPage = options.itemsPerPage;
      searchModel.searchString = this.search;
      searchModel.sortBy = options.sortBy[0] || "";
      searchModel.sortDesc = options.sortDesc[0] || false;

      const response = await httpService.post(
        "/api/AgriculturalMachinery/GetAllMachineries",
        searchModel
      );

      this.machineries = response.data.items;

      this.totalItems = response.data.total;
    } catch (err) {
      console.log(err);
    }
  }

  savedEdit() {
    this.showEdit = false;
    this.editSuccess = true;
    this.GetAllMachineries(this.options);
  }

  savedDelete() {
    this.deleteSuccess = true;
    this.GetAllMachineries(this.options);
  }

  closeEdit() {
    this.showEdit = false;
  }

  openEditModal(itemId: number) {
    this.showEdit = true;
    this.itemId = itemId;
  }

  ClearInput() {
    this.resetErrorMessage();
    this.dialog = false;
    this.chosenTypeOfOwner = "";
    this.model.person = null;
    this.model.company = null;
    this.model = new AgriculturalMachineryCreationModel({});

    //clearing all validation errors when the modal is closed
    (this.$refs.form as Vue & { reset: () => boolean }).reset();
  }

  resetErrorMessage() {
    this.showError = false;
    this.errorMessages = [""];
  }

  setErrorMessage(error: any) {
    this.showError = true;
    this.errorMessages.push(`${error.response.data}`);
  }

  @Watch("options", { deep: true })
  onOptionsChange() {
    this.GetAllMachineries(this.options);
  }

  @Watch("search")
  onSearchChange() {
    if (this.timeoutQuery !== 0) {
      window.clearTimeout(this.timeoutQuery);
    }
    this.timeoutQuery = window.setTimeout(() => {
      this.GetAllMachineries(this.options);
    }, 500);
  }

  @Watch("chosenTypeOfOwner")
  onOwnerTypeChange() {
    if (this.chosenTypeOfOwner === this.$t("person.person")) {
      this.model.person = new Person();
      this.model.company = null;
    } else {
      this.model.company = new Company();
      this.model.person = null;
    }
  }
}
</script>

<style scoped>
.center {
  margin: auto;
  width: 35%;
  padding: 10px;
}
</style> <td>
                  