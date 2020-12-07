<template>
  <div class="aircraftRegistrationView">
    <div>
      <div>
        <label>{{
          $t("propertiesList.aircrafts.registrations.tableTitle")
        }}</label>
      </div>
      <v-data-table
        :headers="headers"
        :items="localItems"
        v-if="localItems && localItems.length > 0"
        :single-select="true"
        item-key="id"
        class="table-striped elevation-1"
      >
        <template v-slot:[`item.registrationDate`]="{ item }">
          <span>{{ formatDate(item.registrationDate) }}</span>
        </template>

        <template v-slot:[`item.registrationDocumentDate`]="{ item }">
          <span>{{ formatDate(item.registrationDocumentDate) }}</span>
        </template>

        <template v-slot:[`item.deregistrationDate`]="{ item }">
          <span>{{ formatDate(item.deregistrationDate) }}</span>
        </template>

        <template v-slot:[`item.leasingDocumentDate`]="{ item }">
          <span>{{ formatDate(item.leasingDocumentDate) }}</span>
        </template>

        <template v-slot:[`item.leasingEndDate`]="{ item }">
          <span>{{ formatDate(item.leasingEndDate) }}</span>
        </template>

        <template v-slot:[`item.isLastRegistration`]="{ item }">
          <span>{{ formatBoolean(item.isLastRegistration) }}</span>
        </template>

        <template v-slot:[`item.registrationStatusIsActive`]="{ item }">
          <span>{{ formatBoolean(item.registrationStatusIsActive) }}</span>
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
export default class AircraftRegistrationView extends Vue {
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
    if (value) return this.$t("common.yes");
    else return this.$t("common.no");
  }

  getHeaders() {
    return [
      {
        text: this.$t("propertiesList.aircrafts.registrations.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("propertiesList.aircrafts.registrations.actNumber"),
        align: "start",
        sortable: false,
        value: "actNumber",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.registrationNumber"
        ),
        align: "start",
        sortable: false,
        value: "registrationNumber",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.registrationDate"
        ),
        align: "start",
        sortable: false,
        value: "registrationDate",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.registrationMark"
        ),
        align: "start",
        sortable: false,
        value: "registrationMark",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.isLastRegistration"
        ),
        align: "start",
        sortable: false,
        value: "isLastRegistration",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.registrationStatusIsActive"
        ),
        align: "start",
        sortable: false,
        value: "registrationStatusIsActive",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.registrationStatusCode"
        ),
        align: "start",
        sortable: false,
        value: "registrationStatusCode",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.registrationStatusName"
        ),
        align: "start",
        sortable: false,
        value: "registrationStatusName",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.registrationDocumentNumber"
        ),
        align: "start",
        sortable: false,
        value: "registrationDocumentNumber",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.registrationDocumentDate"
        ),
        align: "start",
        sortable: false,
        value: "registrationDocumentDate",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.registrationDocumentDescription"
        ),
        align: "start",
        sortable: false,
        value: "registrationDocumentDescription",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.deregistrationDate"
        ),
        align: "start",
        sortable: false,
        value: "deregistrationDate",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.deregistrationReason"
        ),
        align: "start",
        sortable: false,
        value: "deregistrationReason",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.deregistrationDescription"
        ),
        align: "start",
        sortable: false,
        value: "deregistrationDescription",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.deregistrationCountryCode"
        ),
        align: "start",
        sortable: false,
        value: "deregistrationCountryCode",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.deregistrationCountryName"
        ),
        align: "start",
        sortable: false,
        value: "deregistrationCountryName",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.leasingDocumentNumber"
        ),
        align: "start",
        sortable: false,
        value: "leasingDocumentNumber",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.leasingDocumentDate"
        ),
        align: "start",
        sortable: false,
        value: "leasingDocumentDate",
      },
      {
        text: this.$t("propertiesList.aircrafts.registrations.leasingEndDate"),
        align: "start",
        sortable: false,
        value: "leasingEndDate",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.leasingAgreement"
        ),
        align: "start",
        sortable: false,
        value: "leasingAgreement",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.leasingLessorPersonIdentifier"
        ),
        align: "start",
        sortable: false,
        value: "leasingLessorPersonIdentifier",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.leasingLessorPersonNames"
        ),
        align: "start",
        sortable: false,
        value: "leasingLessorPersonNames",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.leasingLessorEntityIdentifier"
        ),
        align: "start",
        sortable: false,
        value: "leasingLessorEntityIdentifier",
      },
      {
        text: this.$t(
          "propertiesList.aircrafts.registrations.leasingLessorEntityName"
        ),
        align: "start",
        sortable: false,
        value: "leasingLessorEntityName",
      },
    ];
  }
}
</script>
