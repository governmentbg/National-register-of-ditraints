<template>
  <div class="requests">
      <h3>{{ $t("RequestForCertificateOfDistraintOfProperty.requestsListTitle") }}</h3>
      <v-data-table
        :headers="headers"
        :items="items"
        :loading="loading"
        :options.sync="options"
        class="table-striped elevation-1"
      >
        <template v-slot:[`item.createdOn`]="{ item }">
          <span>{{ formatDateTime(item.createdOn) }}</span>
        </template>
      </v-data-table>
  </div>
</template>

<script lang="ts">
// @ is an alias to /src
import { Component, Vue } from "vue-property-decorator";
import { mapGetters } from "vuex";
import axios from 'axios';
import { RequestViewModel } from "@/models/request";
import moment from "moment";

@Component({
  components: {
  },
  computed: {
    ...mapGetters({
      dateTimeFormat: "dateTimeFormat",
    }),
  },
})
export default class RequestForCertificateOfDistraintOfPropertyList extends Vue {
  private items: RequestViewModel[] = [];
  private loading = false;
  private options: object = {};
  private headers: object[] = [];
  dateTimeFormat: any;

  mounted() {
    this.headers = this.getHeaders();
    this.fetchItems();
  }

   formatDateTime(value: Date) {
    if (value != null)
      return moment(value.toLocaleString()).format(this.dateTimeFormat);
    else return value;
  }

  async fetchItems() {
      this.loading = true;
      axios.get('/api/requestForCertificateOfDistraintOfProperty/list')
                .then((response) => {
                    response.data.forEach((request: any) => {
                        this.items.push(new RequestViewModel({
                            id: request.id,
                            firstName: request.firstName,
                            middleName: request.middleName,
                            lastName: request.lastName,
                            personalIdentifierType: request.personalIdentifierType,
                            personalIdentifier: request.personalIdentifier,
                            region: request.region,
                            municipality: request.municipality,
                            city: request.city,
                            streetAddress: request.streetAddress,
                            phoneNumber: request.phoneNumber,
                            email: request.email,
                            userTypeCode: request.userType, // todo: get User type name drom DB
                            nameOfLegalEntity: request.nameOfLegalEntity,
                            identifierOfLegalEntity: request.identifierOfLegalEntity,
                            regionOfLegalEntity: request.regionOfLegalEntity,
                            municipalityOfLegalEntity: request.municipalityOfLegalEntity,
                            cityOfLegalEntity: request.cityOfLegalEntity,
                            streetAddressOfLegalEntity: request.streetAddressOfLegalEntity,
                            companyCaseNumber: request.companyCaseNumber,
                            propertyTypeName: request.propertyTypeName,
                            propertyTypeNameEn: request.propertyTypeNameEn,
                            propertyIdOtherProperty: request.propertyIdOtherProperty,
                            propertyIdVehicle: request.propertyIdVehicle,
                            propertyIdAircraft: request.propertyIdAircraft,
                            otherPropertyIdentifier: request.otherPropertyIdentifier,
                            vehicleRegistrationNumber: request.vehicleRegistrationNumber,
                            aircraftMsnserialNumber: request.aircraftMsnserialNumber,
                            createdOn: request.createdOn,
                            statusCode: request.statusCode,
                            statusName: request.statusName,
                            statusNameEn: request.statusNameEn
                        }));
                    });
                    
                    this.loading = false;
                }, (error) => {
                    console.log(error);
                    this.loading = false;
                });
  }

  getHeaders() {
    return [
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.id"),
        align: "start",
        sortable: false,
        value: "id",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.propertyId"),
        align: "start",
        sortable: false,
        value: "propertyId",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.propertyType"),
        align: "start",
        sortable: false,
        value: "propertyTypeName",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.propertyIdentifier"),
        align: "start",
        sortable: false,
        value: "propertyRegistrationNumber",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.firstName"),
        align: "start",
        sortable: false,
        value: "firstName",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.middleName"),
        align: "start",
        sortable: false,
        value: "middleName",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.lastName"),
        align: "start",
        sortable: false,
        value: "lastName",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.personalIdentifierType"),
        align: "start",
        sortable: false,
        value: "personalIdentifierType",
      },
            {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.personalIdentifier"),
        align: "start",
        sortable: false,
        value: "personalIdentifier",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.region"),
        align: "start",
        sortable: false,
        value: "region",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.municipality"),
        align: "start",
        sortable: false,
        value: "municipality",
      },

      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.city"),
        align: "start",
        sortable: false,
        value: "city",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.streetAddress"),
        align: "start",
        sortable: false,
        value: "streetAddress",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.phoneNumber"),
        align: "start",
        sortable: false,
        value: "phoneNumber",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.email"),
        align: "start",
        sortable: false,
        value: "email",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.userType"),
        align: "start",
        sortable: false,
        value: "userTypeName",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.nameOfLegalEntity"),
        align: "start",
        sortable: false,
        value: "nameOfLegalEntity",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.identifierOfLegalEntity"),
        align: "start",
        sortable: false,
        value: "identifierOfLegalEntity",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.regionOfLegalEntity"),
        align: "start",
        sortable: false,
        value: "regionOfLegalEntity",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.municipalityOfLegalEntity"),
        align: "start",
        sortable: false,
        value: "municipalityOfLegalEntity",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.cityOfLegalEntity"),
        align: "start",
        sortable: false,
        value: "cityOfLegalEntity",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.streetAddressOfLegalEntity"),
        align: "start",
        sortable: false,
        value: "streetAddressOfLegalEntity",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.companyCaseNumber"),
        align: "start",
        sortable: false,
        value: "companyCaseNumber",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.paymentStatusName"),
        align: "start",
        sortable: false,
        value: "statusName",
      },
      {
        text: this.$t("RequestForCertificateOfDistraintOfProperty.list.createdOn"),
        align: "start",
        sortable: false,
        value: "createdOn",
      },
    ];
  }
}
</script>
