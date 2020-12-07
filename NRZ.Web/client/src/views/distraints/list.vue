<template>
  <div class="distraints">
    <div class="mb-3 text-center">
      <h3>{{ $t("distraints.title") }}</h3>
      <ValidationObserver v-slot="{ invalid }">
        <form @submit.prevent="onSubmit">
          <div class="col-md-4 col-12 col-sm-6 offset-md-4 offset-sm-3">
            <ValidationProvider
              ref="observer"
              v-slot="{ errors }"
              :name="$t('distraints.searchHint')"
              rules="min:3"
            >
              <v-text-field
                v-model="searchText"
                append-icon="search"
                :label="$t('distraints.searchHint')"
                :error-messages="errors"
                :hint="$t('distraints.searchMinLength')"
                single-line
                v-on:keyup.enter="onClearSearch"
              ></v-text-field>
            </ValidationProvider>
          </div>
          <div>
            <v-btn
              type="submit"
              :color="'primary'"
              :disabled="!searchText || invalid"
              >{{ $t("distraints.btnSearch") }}</v-btn
            >
          </div>
        </form>
      </ValidationObserver>
    </div>
    <div>
      <v-row>
        <v-col cols="6" sm="3">
          <v-btn color="primary" dark @click="createDistraint">
            <v-icon small class="mr-2"> mdi-lock-open </v-icon>
            {{ $t("distraints.btnNew") }}
          </v-btn>
        </v-col>
      </v-row>
    </div>
    <div>
      <v-data-table
        :headers="headers"
        :items="items"
        :loading="loading"
        :options.sync="options"
        class="table-striped elevation-1"
      >
        <template v-slot:[`item.enforcementDate`]="{ item }">
          <span>{{ formatDate(item.enforcementDate) }}</span>
        </template>

        <template v-slot:[`item.enforcedAt`]="{ item }">
          <span>{{ formatDateTime(item.enforcedAt) }}</span>
        </template>

        <template v-slot:[`item.revocationDate`]="{ item }">
          <span>{{ formatDate(item.revocationDate) }}</span>
        </template>

        <template v-slot:[`item.revokedAt`]="{ item }">
          <span>{{ formatDateTime(item.revokedAt) }}</span>
        </template>

        <template v-slot:[`item.exemptionDate`]="{ item }">
          <span>{{ formatDate(item.exemptionDate) }}</span>
        </template>

        <template v-slot:[`item.exemptedAt`]="{ item }">
          <span>{{ formatDateTime(item.exemptedAt) }}</span>
        </template>

        <template v-slot:[`item.createdOn`]="{ item }">
          <span>{{ formatDateTime(item.createdOn) }}</span>
        </template>

        <template v-slot:[`item.actions`]="{ item }">
          <v-icon
            small
            class="mr-2"
            @click="viewDistraint(item)"
            :title="$t('distraints.list.view')"
          >
            mdi-eye
          </v-icon>
          <v-icon
            small
            class="mr-2"
            @click="enforceDistraint(item)"
            :title="$t('distraints.list.enforce')"
            v-if="canEnforceDistraint(item)"
          >
            mdi-lock
          </v-icon>
          <v-icon
            small
            class="mr-2"
            @click="revokeDistraint(item)"
            :title="$t('distraints.list.revoke')"
            v-if="canRevokeDistraint(item)"
          >
            mdi-undo-variant
          </v-icon>
          <v-icon
            small
            class="mr-2"
            @click="exemptDistraint(item)"
            :title="$t('distraints.list.exempt')"
            v-if="canExemptDistraint(item)"
          >
            mdi-lock-open</v-icon
          >
        </template>
      </v-data-table>
      <distraint-enforce
        :show="showDistraintEnforce"
        :item="selectedDistraint"
        :today="today"
        v-on:close-enforce="closeEnforceDistraint($event)"
        v-on:saved-enforce="savedEnforceDistraint($event)"
      ></distraint-enforce>
      <distraint-revoke
        :show="showDistraintRevoke"
        :item="selectedDistraint"
        :today="today"
        v-on:close-revoke="closeRevokeDistraint($event)"
        v-on:saved-revoke="savedRevokeDistraint($event)"
      ></distraint-revoke>
      <distraint-exempt
        :show="showDistraintExempt"
        :item="selectedDistraint"
        :today="today"
        v-on:close-exempt="closeExemptDistraint($event)"
        v-on:saved-exempt="savedExemptDistraint($event)"
      ></distraint-exempt>
    </div>
    <div>
      <error-message
        :show="showError"
        :messages="errorMessages"
      ></error-message>
    </div>
  </div>
</template>

<script lang="ts">
// @ is an alias to /src
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { Component, Vue } from "vue-property-decorator";
import { DistraintModel } from "@/models/distraint";
import distraintService from "@/services/distraint.service";
import moment from "moment";
import { mapGetters } from "vuex";
import { DistraintStatus } from "@/models/enums";
import DistraintEnforce from "@/components/distraints/enforce.modal.vue";
import DistraintRevoke from "@/components/distraints/revoke.modal.vue";
import DistraintExempt from "@/components/distraints/exempt.modal.vue";
import ErrorMessage from "@/components/common/errorMessage.vue";
import VueI18n, { TranslateResult } from "vue-i18n";
import settingsService from "@/services/settings.service";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
    DistraintEnforce,
    DistraintRevoke,
    DistraintExempt,
    ErrorMessage,
  },
  computed: {
    ...mapGetters({
      dateFormat: "dateFormat",
      dateTimeFormat: "dateTimeFormat",
    }),
  },
})
export default class DistraintsList extends Vue {
  private items: DistraintModel[] = [];
  private searchText = "";
  private loading = false;
  private totalItems = 0;
  private options: object = {};
  private headers: object[] = [];
  dateFormat: any;
  dateTimeFormat: any;
  private showDistraintEnforce = false;
  private showDistraintRevoke = false;
  private showDistraintExempt = false;
  private selectedDistraint = new DistraintModel();
  private showError = false;
  private errorMessages: [VueI18n.TranslateResult] = [""];
  private today = new Date();

  mounted() {
    this.headers = this.getHeaders();
    this.fetchItems();
    this.getToday();
  }

    async getToday() {
    try {
      const result = await settingsService.getToday();
      this.today = new Date(result as Date);
      console.log(this.today);
    } catch (error) {
      console.log(error);
    }
  }

  resetErrorMessages() {
    this.showError = false;
    this.errorMessages = [""];
  }

  setErrorMessage(generalError: TranslateResult, error: any) {
    console.log(error);
    this.showError = true;
    if (generalError) {
      this.errorMessages.push(generalError);
    }
    if (error) {
      this.errorMessages.push(`${error.response.data}`);
    }
  }

  async fetchItems() {
    try {
      this.loading = true;
      this.resetErrorMessages();
      const result = await distraintService.list();
      this.items = result as DistraintModel[];
      this.loading = false;
    } catch (error) {
      console.log("Error on listing distraints: " + error);
      this.setErrorMessage(this.$t("errors.errorLoadingDistraintsList"), null);
      this.loading = false;
    }
  }

  async searchItems() {
    try {
      this.loading = true;
      this.resetErrorMessages();
      const result = await distraintService.search(this.searchText);
      this.items = result as DistraintModel[];
      this.loading = false;
    } catch (error) {
      console.log("Error on searching distraints: " + error);
      this.setErrorMessage(this.$t("errors.errorSearchingForDistraints"), null);
      this.loading = false;
    }
  }

  formatDate(value: Date) {
    if (value != null)
      return moment(value.toLocaleString()).format(this.dateFormat);
    else return value;
  }

  formatDateTime(value: Date) {
    if (value != null)
      return moment(value.toLocaleString()).format(this.dateTimeFormat);
    else return value;
  }

  async onSubmit() {
    if (this.searchText) {
      await this.searchItems();
    }
  }

  async onClearSearch() {
    if (!this.searchText) {
      await this.fetchItems();
    }
  }

  getHeaders() {
    return [
      {
        text: this.$t("distraints.list.actions"),
        align: "start",
        sortable: false,
        value: "actions",
      },
      {
        text: this.$t("distraints.list.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("distraints.list.statusName"),
        align: "start",
        sortable: false,
        value: "statusName",
      },
      {
        text: this.$t("distraints.list.propertyId"),
        align: "start",
        sortable: false,
        value: "propertyId",
      },
      {
        text: this.$t("distraints.list.propertyTypeName"),
        align: "start",
        sortable: false,
        value: "propertyTypeName",
      },
      {
        text: this.$t("distraints.list.propertyRegNumber"),
        align: "start",
        sortable: false,
        value: "propertyRegNumber",
      },
      {
        text: this.$t("distraints.list.createdOn"),
        align: "start",
        sortable: false,
        value: "createdOn",
      },

      {
        text: this.$t("distraints.list.inFavourOf"),
        align: "start",
        sortable: false,
        value: "inFavourOf",
      },
            {
        text: this.$t("distraints.list.identifier"),
        align: "start",
        sortable: false,
        value: "inFavourOfIdentifier",
      },
      {
        text: this.$t("distraints.list.suitNumber"),
        align: "start",
        sortable: false,
        value: "suitNumber",
      },
      {
        text: this.$t("distraints.list.debtor"),
        align: "start",
        sortable: false,
        value: "debtor",
      },
            {
        text: this.$t("distraints.list.identifier"),
        align: "start",
        sortable: false,
        value: "debtorIdentifier",
      },
      {
        text: this.$t("distraints.list.location"),
        align: "start",
        sortable: false,
        value: "location",
      },
      {
        text: this.$t("distraints.list.enforcementDate"),
        align: "start",
        sortable: false,
        value: "enforcementDate",
      },
      {
        text: this.$t("distraints.list.enforcedAt"),
        align: "start",
        sortable: false,
        value: "enforcedAt",
      },
      {
        text: this.$t("distraints.list.revocationDate"),
        align: "start",
        sortable: false,
        value: "revocationDate",
      },
      {
        text: this.$t("distraints.list.revokedAt"),
        align: "start",
        sortable: false,
        value: "revokedAt",
      },
      {
        text: this.$t("distraints.list.exemptionDate"),
        align: "start",
        sortable: false,
        value: "exemptionDate",
      },
      {
        text: this.$t("distraints.list.exemptedAt"),
        align: "start",
        sortable: false,
        value: "exemptedAt",
      },
    ];
  }

  viewDistraint(item: DistraintModel) {
    this.$router.push({ path: "/distraints/view/" + item.id });
  }

  canEnforceDistraint(item: DistraintModel) {
    if (!item || item.statusCode != DistraintStatus.Registered) return false;
    return true;
  }
  enforceDistraint(item: DistraintModel) {
    this.showDistraintEnforce = true;
    this.selectedDistraint = item;
  }

  closeEnforceDistraint() {
    this.showDistraintEnforce = false;
  }

  savedEnforceDistraint() {
    this.showDistraintEnforce = false;
    this.fetchItems();
  }

  canRevokeDistraint(item: DistraintModel) {
    if (!item || item.statusCode != DistraintStatus.Enforced) return false;
    return true;
  }

  revokeDistraint(item: DistraintModel) {
    this.showDistraintRevoke = true;
    this.selectedDistraint = item;
  }

  closeRevokeDistraint() {
    this.showDistraintRevoke = false;
  }

  savedRevokeDistraint() {
    this.showDistraintRevoke = false;
    this.fetchItems();
  }

  canExemptDistraint(item: DistraintModel) {
    if (!item || item.statusCode != DistraintStatus.Enforced) return false;
    return true;
  }

  exemptDistraint(item: DistraintModel) {
    this.showDistraintExempt = true;
    this.selectedDistraint = item;
  }

  closeExemptDistraint() {
    this.showDistraintExempt = false;
  }

  savedExemptDistraint() {
    this.showDistraintExempt = false;
    this.fetchItems();
  }
  createDistraint() {
    this.$router.push({ path: "/distraints/create" });
  }
}
</script>
