<template>
  <div class="eServicePaymentStatusHistory">
    <v-dialog v-model="doShow" persistent max-width="500">
      <v-card>
        <div>
          <div class="mb-3 text-center">
            <h5>{{ $t("ePayments.history.title") }}</h5>
          </div>
          <v-data-table
            :headers="headers"
            :items="items"
            v-if="items && items.length > 0"
            :single-select="true"
            item-key="id"
            class="table-striped elevation-1"
          >
            <template v-slot:[`item.updatedAt`]="{ item }">
              <span>{{ formatDateTime(item.updatedAt) }}</span>
            </template>

            <template v-slot:[`item.eServiceTime`]="{ item }">
              <span>{{ formatDateTime(item.eServiceTime) }}</span>
            </template>
          </v-data-table>
        </div>
        <div>
          <error-message
            :show="showError"
            :messages="errorMessages"
          ></error-message>
        </div>
        <v-card-actions>
          <v-btn color="primary" @click.stop="close()">{{
            $t("buttons.cancel")
          }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop, Ref } from "vue-property-decorator";
import { EServicePaymentStatusHistoryModel } from "@/models/ePayments";
import { mapGetters } from "vuex";
import moment from "moment";
import ePaymentService from "@/services/ePayment.service";
import ErrorMessage from "@/components/common/errorMessage.vue";
import VueI18n, { TranslateResult } from "vue-i18n";

@Component({
  components: {
    ErrorMessage,
  },
  computed: {
    ...mapGetters({
      dateTimeFormat: "dateTimeFormat",
      language: "language",
    }),
  },
})
export default class EServicePaymentStatusHistory extends Vue {
  @Prop({ default: false }) show: any;
  @Prop({ required: true }) requestId: any;
  dateTimeFormat: any;
  language: any;
  private headers: object[] = [];
  private items: EServicePaymentStatusHistoryModel[] = [];
  private showError = false;
  private errorMessages: [VueI18n.TranslateResult] = [""];

  constructor() {
    super();
  }

  mounted() {
    this.headers = this.getHeaders();
  }

  getHeaders() {
    return [
      {
        text: this.$t("ePayments.history.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("ePayments.history.statusCodeName"),
        align: "start",
        sortable: false,
        value: "statusCodeName",
      },
      {
        text: this.$t("ePayments.history.updatedAt"),
        align: "start",
        sortable: false,
        value: "updatedAt",
      },
      {
        text: this.$t("ePayments.history.eServiceTime"),
        align: "start",
        sortable: false,
        value: "eServiceTime",
      },
      {
        text: this.$t("ePayments.history.errors"),
        align: "start",
        sortable: false,
        value: "errors",
      },
    ];
  }

  async fetchItems() {
    try {
      this.resetErrorMessages();
      const result = await ePaymentService.getPaymentRequestHistory(
        this.requestId
      );
      this.items = result as EServicePaymentStatusHistoryModel[];
    } catch (error) {
      console.log("Error on listing payment history: " + error);
      this.setErrorMessage(this.$t("errors.errorLoadingPaymentHistory"), null);
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

  get doShow() {
    if (this.show) {
      this.fetchItems();
    }
    return this.show;
  }

  formatDateTime(value: Date) {
    if (value != null)
      return moment(value.toLocaleString()).format(this.dateTimeFormat);
    else return value;
  }

  close() {
    this.$emit("close-statusHistory");
  }
}
</script>