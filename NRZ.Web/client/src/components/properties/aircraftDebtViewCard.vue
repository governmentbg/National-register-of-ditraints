<template>
  <div class="aircraftDebtView">
    <div>
      <div>
          <label class="px-3">{{ $t("propertiesList.aircrafts.debts.tableTitle") }}</label>
      </div>
      <v-data-table
        :headers="headers"
        :items="localItems"
        v-if="localItems && localItems.length > 0"
        :single-select="true"
        item-key="id"
        class="table-striped elevation-1"
      >
        <template v-slot:[`item.inputDate`]="{ item }">
          <span>{{ formatDate(item.inputDate) }}</span>
        </template>

        <template v-slot:[`item.documentIncomingDate`]="{ item }">
          <span>{{ formatDate(item.documentIncomingDate) }}</span>
        </template>

        <template v-slot:[`item.documentExternalDate`]="{ item }">
          <span>{{ formatDate(item.documentExternalDate) }}</span>
        </template>

        <template v-slot:[`item.repaymentDate`]="{ item }">
          <span>{{ formatDate(item.repaymentDate) }}</span>
        </template>

        <template v-slot:[`item.repaymentDocumentIncomingDate`]="{ item }">
          <span>{{ formatDate(item.repaymentDocumentIncomingDate) }}</span>
        </template>

        <template v-slot:[`item.repaymentDocumentExternalDate`]="{ item }">
          <span>{{ formatDate(item.repaymentDocumentExternalDate) }}</span>
        </template>

        <template v-slot:[`item.isActive`]="{ item }">
          <span>{{ formatBoolean(item.isActive) }}</span>
        </template>
      </v-data-table>
    </div>
  </div>
</template>

<script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop } from "vue-property-decorator";
import moment from "moment";
import { mapGetters } from "vuex";

@Component({
  computed: {
    ...mapGetters({
      dateFormat: "dateFormat",
      dateTimeFormat: "dateTimeFormat",
    }),
  },
})
export default class AircraftDebtView extends Vue {
  @Prop({ required: true }) items: any;
  private headers: object[] = [];
  dateFormat: any;
  dateTimeFormat: any;

  get localItems() {
    return this.items;
  }

  mounted() {
    this.headers = this.getHeaders();
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

  formatBoolean(value: boolean) {
    if (value)
      return this.$t("common.yes");
    else return this.$t("common.no");
  }

  getHeaders() {
    return [
      {
        text: this.$t("propertiesList.aircrafts.debts.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.inputDate"),
        align: "start",
        sortable: false,
        value: "inputDate",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.debtTypeCode"),
        align: "start",
        sortable: false,
        value: "debtTypeCode",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.debtType"),
        align: "start",
        sortable: false,
        value: "debtType",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.isActive"),
        align: "start",
        sortable: false,
        value: "isActive",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.applicantIdentifier"),
        align: "start",
        sortable: false,
        value: "applicantIdentifier",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.applicantName"),
        align: "start",
        sortable: false,
        value: "applicantName",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.documentIncomingNumber"),
        align: "start",
        sortable: false,
        value: "documentIncomingNumber",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.documentIncomingDate"),
        align: "start",
        sortable: false,
        value: "documentIncomingDate",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.documentExternalNumber"),
        align: "start",
        sortable: false,
        value: "documentExternalNumber",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.documentExternalDate"),
        align: "start",
        sortable: false,
        value: "documentExternalDate",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.repaymentDate"),
        align: "start",
        sortable: false,
        value: "repaymentDate",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.debts.repaymentDocumentIncomingNumber"
        ),
        align: "start",
        sortable: false,
        value: "repaymentDocumentIncomingNumber",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.debts.repaymentDocumentIncomingDate"
        ),
        align: "start",
        sortable: false,
        value: "repaymentDocumentIncomingDate",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.debts.repaymentDocumentExternalNumber"
        ),
        align: "start",
        sortable: false,
        value: "repaymentDocumentExternalNumber",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.debts.repaymentDocumentExternalDate"
        ),
        align: "start",
        sortable: false,
        value: "repaymentDocumentExternalDate",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.repaymentNotes"),
        align: "start",
        sortable: false,
        value: "repaymentNotes",
      },
      {
        text: this.$t("propertiesList.aircrafts.debts.notes"),
        align: "start",
        sortable: false,
        value: "notes",
      },
    ];
  }
}
</script>
