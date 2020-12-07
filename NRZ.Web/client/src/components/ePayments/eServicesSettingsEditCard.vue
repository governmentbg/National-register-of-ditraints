<template>
  <div class="eServicesSettingsEdit">
    <div>
      <v-row>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.serviceProviderName"
            :label="$t('eServicesSettings.serviceProviderName')"
          ></v-text-field>
        </v-col>

        <v-col cols="12" sm="6">
          <v-text-field
            v-model="localItem.serviceProviderBank"
            :label="$t('eServicesSettings.serviceProviderBank')"
          ></v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.serviceProviderBic"
            :label="$t('eServicesSettings.serviceProviderBic')"
          ></v-text-field>
        </v-col>

        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.serviceProviderIban"
            :label="$t('eServicesSettings.serviceProviderIban')"
          ></v-text-field>
        </v-col>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.currency"
            :label="$t('eServicesSettings.currency')"
          ></v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.seizedPropertyCertificateFee"
            :label="$t('eServicesSettings.seizedPropertyCertificateFee')"
          ></v-text-field>
        </v-col>

        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.seizedPropertyCertificateReason"
            :label="$t('eServicesSettings.seizedPropertyCertificateReason')"
          ></v-text-field>
        </v-col>

        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.seizedPropertyCertificateRequestExpirationDays"
            :label="
              $t(
                'eServicesSettings.seizedPropertyCertificateRequestExpirationDays'
              )
            "
          ></v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.seizedPropertyByOwnerReportFee"
            :label="$t('eServicesSettings.seizedPropertyByOwnerReportFee')"
          ></v-text-field>
        </v-col>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.seizedPropertyReportReason"
            :label="$t('eServicesSettings.seizedPropertyReportReason')"
          ></v-text-field>
        </v-col>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.seizedPropertyReportRequestExpirationDays"
            :label="
              $t('eServicesSettings.seizedPropertyReportRequestExpirationDays')
            "
          ></v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12" sm="6">
          <v-text-field
            v-model="localItem.administrativeServiceNotificationUrl"
            :label="
              $t('eServicesSettings.administrativeServiceNotificationUrl')
            "
          ></v-text-field>
        </v-col>
      </v-row>
    </div>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop, Ref } from "vue-property-decorator";
import { EServiceSettingsModel } from "@/models/eServicesSettings";

@Component({})
export default class EServicesSettingsEditCard extends Vue {
  @Prop({ required: true }) item: any;

  constructor() {
    super();
  }

  get localItem() {
    return this.item || new EServiceSettingsModel();
  }
}
</script>