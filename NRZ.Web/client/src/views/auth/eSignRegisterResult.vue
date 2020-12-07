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
            <div v-if="showEmail">
              <ValidationObserver ref="validator">
                <form :action="certRegisterUrl" method="GET" @submit="submit">
                  <div class="mb-3 alert alert-warning">
                    <i class="fas fa-exclamation-circle"></i>
                    &nbsp;
                    {{ $t("notifications.certLoginRequiredEmail") }}
                  </div>
                  <div class="form-group">
                    <ValidationProvider
                      v-slot="{ errors }"
                      :name="$t('register.email')"
                      rules="required|email"
                    >
                      <v-text-field
                        v-model="email"
                        :label="$t('register.email')"
                        :error-messages="errors"
                        class="required"
                        name="email"
                      ></v-text-field>
                    </ValidationProvider>
                  </div>
                  <v-btn type="submit" :color="'primary'">{{
                    $t("buttons.submit.text")
                  }}</v-btn>
                </form>
              </ValidationObserver>
            </div>
          </div>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Ref } from "vue-property-decorator";
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { TranslateResult } from "vue-i18n";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
  },
})
export default class LoginResult extends Vue {
  showEmail = false;
  showError = false;
  showWarning = false;
  errorMessage: TranslateResult = "";
  warningMessage = "";
  email = "";

  @Ref() private validator: any;

  get certRegisterUrl() {
    return this.$store.getters.baseUrl + "/certificate/register";
  }

  async submit(e: Event) {
    const formValid = await this.validator.validate();
    if (!formValid) {
      e.preventDefault();
    }
  }

  mounted() {
    const {error, warning, requireEmail, warningMessage, message, number} = this.$route.query;

    if (error) {
      this.showError = true;
      if (message) {
        this.errorMessage = this.$t(
          "notifications." + message
        );
      }
    } else if (requireEmail) {
      this.showEmail = true;
    } else {
      let successRoute = "/register/success";
      if (warning) {
        successRoute += `?warning=true&warningMessage=${warningMessage}&number=${number}`;
      }
      this.$router.push({ path: successRoute });
    }
  }
}
</script>

<style>
</style>