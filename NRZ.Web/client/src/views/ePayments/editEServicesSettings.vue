<template>
  <div class="eServiceSettings">
    <div class="mb-3 text-center">
      <h3>{{ $t("eServicesSettings.changeSettingsTitle") }}</h3>
    </div>
    <div>
      <e-services-settings-edit-card :item="settingsItemData">
      </e-services-settings-edit-card>
    </div>
    <div>
      <v-btn color="primary" @click="save()">
        {{ $t("buttons.save") }}
      </v-btn>
      <v-btn text @click="backToHome">
        {{ $t("buttons.close") }}
      </v-btn>
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
import { Component, Vue, Watch, Ref } from "vue-property-decorator";
import EServicesSettingsEditCard from "@/components/ePayments/eServicesSettingsEditCard.vue";
import ErrorMessage from "@/components/common/errorMessage.vue";
import { namespace } from "vuex-class";
import { mapGetters } from "vuex";
import VueI18n, { TranslateResult } from "vue-i18n";
const userNS = namespace("user");
import ePaymentService from "@/services/ePayment.service";
import { EServiceSettingsModel } from '@/models/eServicesSettings';

@Component({
  components: {
    EServicesSettingsEditCard,
    ErrorMessage,
  },
  computed: {
    ...mapGetters("user", ["getUserName", "getUserRoles"]),
  },
})
export default class EditEServicesSettings extends Vue {
  @userNS.Getter("getUserId") userId: any;
  @userNS.Getter("getUserName") userName: any;
  @userNS.Getter("getUserRoles") userRoles: any;
  private settingsItemData: any = new EServiceSettingsModel();
  private showError = false;
  private errorMessages: [VueI18n.TranslateResult] = [""];

  mounted() {
    // this.settingsItemData.createdByUserName = this.userName;
    // this.settingsItemData.createdByUserType = this.userRoles;
    this.loadSettings();
  }

  async loadSettings() {
    try {
      const result = await ePaymentService.loadSettings();
      this.settingsItemData = result as EServiceSettingsModel;
    } catch (error) {
      this.setErrorMessage(this.$t("errors.errorLoadingEServicesSettings"), null);
    }
  }

  resetErrorMessages() {
    this.showError = false;
    this.errorMessages = [""];
  }

  backToHome() {
    this.$router.push({ path: "/" });
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

  async save() {
    this.resetErrorMessages();

      console.log(this.settingsItemData);
      debugger;
      try {
        const result = await ePaymentService.changeSettings(this.settingsItemData);
        console.log("save result: " + result);

        this.backToHome();
      } catch (error) {
        this.setErrorMessage(this.$t("errors.errorChangingEServicesSettings"), null);
      }
  }
}
</script>