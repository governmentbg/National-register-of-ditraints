<template>
  <div class="w-100">
    <v-row :align="'baseline'" :justify="'center'">
      <v-col :md="4">
        <v-card>
          <v-card-title>{{ $t("login.title") }}</v-card-title>
          <div class="p-3">
            <div v-if="showError">
              <div class="alert alert-danger">
                {{ $t("notifications.clientCertLoginError") }}
              </div>
              <div class="alert alert-danger" v-if="errorMessage">
                {{ errorMessage }}
              </div>
              <router-link to="/login">{{
                $t("register.backToLogin")
              }}</router-link>
            </div>
          </div>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { TranslateResult } from "vue-i18n";
import { namespace } from "vuex-class";
import UserModel from '@/models/user';
const userNS = namespace("user");

@Component
export default class LoginResult extends Vue {
  showError = false;
  errorMessage: TranslateResult = "";
  @userNS.Action("setUser") setUser!: (user: UserModel) => void;

  mounted() {
    if (this.$route.query.auth) {
      const param = this.$route.query.auth.toString();
      if (param) {
        const userInfo = JSON.parse(param);
        const user = new UserModel(userInfo);
        this.setUser(user);
        this.$router.push({ path: "/" });
      }
    } else if (this.$route.query.error) {
      this.showError = true;
      if (this.$route.query.message) {
        this.errorMessage = this.$t(
          "notifications." + this.$route.query.message
        );
      } else if (this.$route.query.messageTxt) {
        this.errorMessage = this.$route.query.messageTxt.toString();
      }
    }
  }
}
</script>

<style>
</style>