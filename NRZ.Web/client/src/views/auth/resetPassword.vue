<template>
  <div>
    <v-row :align="'baseline'" :justify="'center'">
      <v-col cols="4">
        <v-card>
          <v-card-title>{{ $t("userProfile.changePassword") }}</v-card-title>
          <v-card-text>
            <v-row :align="'baseline'" :justify="'center'">
              <message :type="alertType"
                       :messages="alertMessage"
                       :show="alertShow"
                       :duration="7000"
                       @change="alertShow = !alertShow"></message>
            </v-row>
            <div v-if="loading">
              <v-row :align="'center'" :justify="'center'">
                <v-progress-circular :indeterminate="true" :color="'blue-grey'" :size="100"></v-progress-circular>
              </v-row>
            </div>
            <div v-if="!loading">
              <ValidationObserver ref="validator">
                <div class="mt-3">
                  <ValidationProvider v-slot="{ errors }" :name="$t('userProfile.newPassword')"
                                      :rules="`required|min:6|passwordPattern`">
                    <v-text-field :error-messages="errors"
                                  type="password"
                                  class="required"
                                  v-model="model.newPassword"
                                  :label="$t('userProfile.newPassword')"></v-text-field>
                  </ValidationProvider>
                </div>
                <div class="mt-3">
                  <ValidationProvider v-slot="{ errors }" :name="$t('userProfile.confirmPassword')"
                                      :rules="`required|min:6|compareFields:@${$t('userProfile.newPassword')}|passwordPattern`">
                    <v-text-field class="required"
                                  type="password"
                                  v-model="model.newPasswordConfirm"
                                  :error-messages="errors"
                                  :label="$t('userProfile.confirmPassword')"></v-text-field>
                  </ValidationProvider>
                </div>
              </ValidationObserver>
              <v-card-actions>
                <v-btn color="primary"
                       :disabled="alertShow"
                       @click.stop="onSubmit">
                  {{ $t("buttons.save") }}
                </v-btn>
              </v-card-actions>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import {Component, Vue, Ref} from "vue-property-decorator";
import {ValidationProvider, ValidationObserver} from "vee-validate";
import ForgottenPassword from "@/models/forgottenPassword";
import httpService from '../../services/http.service';
import {TranslateResult} from "vue-i18n";
import Message from "@/components/common/message.vue";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
    Message
  }
})

export default class ResetPassword extends Vue {
  @Ref() private validator: any;
  name = "resetPassword";
  isValid!: boolean;
  model!: ForgottenPassword;
  loading: boolean;
  alertShow!: boolean;
  alertMessage!: string | TranslateResult;
  alertType!: string;
  userId!: string;

  constructor() {
    super();
    this.isValid = false;
    this.model = new ForgottenPassword();
    this.alertShow = false;
    this.alertType = '';
    this.alertMessage = '';
    this.loading = false;
    this.userId = '';
  }

  async onSubmit() {
    this.isValid = await this.validator.validate();
    if (this.isValid) {
      this.userReset();
    }
  }

  userReset() {
    this.loading = true;
    httpService.post('api/account/resetPassword', this.model).then(() => {
      this.loading = false;
      this.$router.push('/resetPassword/success');
    }).catch(error => {
      this.loading = false;
      if (error?.response?.data) {
        this.showError(error.response.data)
      } else {
        this.showError(error)
      }
    })
  }

  showError(error: string) {
    this.alertMessage = error;
    this.alertType = 'error'
    this.alertShow = true;
  }

  showSuccess(message: TranslateResult) {
    this.alertMessage = message;
    this.alertType = 'success'
    this.alertShow = true;
  }

  mounted() {
    this.getParameters();
  }

  getParameters() {
    this.loading = true;
    if (this.$route.query) {
      this.model.userId = this.$route.query['userId'];
      this.model.token = this.$route.query['code'];
      httpService.get(`api/user/get/${this.model.userId}`).then(response => {
        if (response?.data) {
          this.model.email = response.data.email;
        }
        this.loading = false;
      }).catch(error => {
        this.loading = false;
        if (error?.response?.data) {
          this.showError(error.response.data)
        } else {
          this.showError(error)
        }
      })
    }
  }
}
</script>

<style scoped>

</style>