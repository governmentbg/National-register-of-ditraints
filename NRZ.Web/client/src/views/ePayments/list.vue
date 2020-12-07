<template>
  <div class="ePayments">
    <div class="mb-3 text-center">
      <h3>{{ $t("ePayments.title") }}</h3>
    </div>
    <div>
      <v-data-table
        :headers="headers"
        :items="items"
        :loading="loading"
        :options.sync="options"
        class="table-striped elevation-1"
      >
        <template v-slot:[`item.createdAt`]="{ item }">
          <span>{{ formatDateTime(item.createdAt) }}</span>
        </template>

        <template v-slot:[`item.updatedAt`]="{ item }">
          <span>{{ formatDateTime(item.updatedAt) }}</span>
        </template>

        <template v-slot:[`item.actions`]="{ item }">
          <v-icon
            small
            class="mr-2"
            @click="viewHistory(item)"
            :title="$t('ePayments.list.history')"
          >
            mdi-history
          </v-icon>
        </template>
      </v-data-table>
      <e-service-payment-status-history
        :show="showHistory"
        :requestId="selectedRequestId"
        v-on:close-statusHistory="closeHistory($event)"
      ></e-service-payment-status-history>
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
import { Component, Vue } from "vue-property-decorator";
import { EServicePaymentRequestModel } from "@/models/ePayments";
import ePaymentService from "@/services/ePayment.service";
import moment from "moment";
import { mapGetters } from "vuex";
import ErrorMessage from "@/components/common/errorMessage.vue";
import VueI18n, { TranslateResult } from "vue-i18n";
import EServicePaymentStatusHistory from "@/components/ePayments/eServicePaymentStatusHistory.modal.vue";

@Component({
  components: {
    ErrorMessage,
    EServicePaymentStatusHistory,
  },
  computed: {
    ...mapGetters({
      dateFormat: "dateFormat",
      dateTimeFormat: "dateTimeFormat",
    }),
  },
})
export default class EPaymentsList extends Vue {
  private items: EServicePaymentRequestModel[] = [];
  private loading = false;
  private totalItems = 0;
  private options: object = {};
  private headers: object[] = [];
  dateFormat: any;
  dateTimeFormat: any;
  private showError = false;
  private errorMessages: [VueI18n.TranslateResult] = [""];
  private showHistory = false;
  private selectedRequestId = 0;

  mounted() {
    this.headers = this.getHeaders();
    this.fetchItems();
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
      const result = await ePaymentService.list();
      this.items = result as EServicePaymentRequestModel[];
      this.loading = false;
    } catch (error) {
      console.log("Error on listing payments: " + error);
      this.setErrorMessage(this.$t("errors.errorLoadingPaymentsList"), null);
      this.loading = false;
    }
  }

  formatDateTime(value: Date) {
    if (value != null)
      return moment(value.toLocaleString()).format(this.dateTimeFormat);
    else return value;
  }

  getHeaders() {
    return [
      {
        text: this.$t("ePayments.list.actions"),
        align: "start",
        sortable: false,
        value: "actions",
      },
      {
        text: this.$t("ePayments.list.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("ePayments.list.eserviceTypeName"),
        align: "start",
        sortable: false,
        value: "eserviceTypeName",
      },
      {
        text: this.$t("ePayments.list.serviceRequestId"),
        align: "start",
        sortable: false,
        value: "serviceRequestId",
      },
      {
        text: this.$t("ePayments.list.paymentRequestId"),
        align: "start",
        sortable: false,
        value: "paymentRequestId",
      },
      {
        text: this.$t("ePayments.list.createdAt"),
        align: "start",
        sortable: false,
        value: "createdAt",
      },
      {
        text: this.$t("ePayments.list.statusName"),
        align: "start",
        sortable: false,
        value: "statusName",
      },
      {
        text: this.$t("ePayments.list.updatedAt"),
        align: "start",
        sortable: false,
        value: "updatedAt",
      },
    ];
  }

  viewHistory(item: EServicePaymentRequestModel) {
    this.showHistory = true;
    this.selectedRequestId = item.id;
  }

  closeHistory() {
    this.showHistory = false;
  }

}
</script>
