<template>
  <div>
    <v-row :align="'baseline'" :justify="'center'">
      <v-col :md="4">
        <v-card>
          <v-card-title>{{ $t("emailConfirm.title") }}</v-card-title>
          <v-card-text>
            <div v-if="showSuccess">
              <v-alert outlined type="success" text>
                {{ $t("emailConfirm.successMessage") }}
              </v-alert>
              <v-alert outlined type="info" text v-if="!isApproved">
                {{ $t("emailConfirm.notApproved") }}
              </v-alert>
              <v-alert outlined type="info" text v-if="isApproved">
                {{ $t("emailConfirm.approved") }}
              </v-alert>
            </div>
            <div v-if="showError">
              <v-alert outlined type="error" text>
                {{ $t("emailConfirm.errorMessage") }}
              </v-alert>
            </div>
          </v-card-text>
        </v-card>
      </v-col>  
    </v-row>
  </div>
</template>

<script lang="ts">
import http from "axios-typescript-response";
import { Component, Vue, Ref } from "vue-property-decorator";
import httpService from "../../services/http.service";

@Component
export default class UserProfile extends Vue {
  showSuccess = false;
  showError = false;
  userType = "";
  isApproved = false;

  mounted() {
    const { userId, code } = this.$route.query;
    if (userId && code) {
      http
        .get(`/api/account/ConfirmEmail?userId=${userId}&code=${code}`)
        .then((response) => {
          const { userType, isApproved} = response.data;
          this.showSuccess = true;
          this.userType = userType;
          this.isApproved = isApproved;
        })
        .catch((err) => (this.showError = true));
    }
  }
}
</script>

<style>
</style>