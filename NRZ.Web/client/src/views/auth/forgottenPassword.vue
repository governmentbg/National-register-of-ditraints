<template>
  <div>
    <v-row :align="'baseline'" :justify="'center'">
      <v-col cols="4">
        <v-card>
          <v-card-title>{{ $t("login.forgotPassword") }}</v-card-title>
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
              <v-alert type="info">
                <p>{{ this.$t('resetPassword.forgottenPasswordHelp') }}</p>
              </v-alert>
              <ValidationObserver ref="validator">
                <div class="mt-5">
                  <ValidationProvider v-slot="{ errors }" :name="$t('users.headers.userName')"
                                      :rules="'required'">
                    <v-text-field class="required"
                                  type="text"
                                  v-model="user.userName"
                                  @input="checkUserName"
                                  :error-messages="errors"
                                  :label="$t('users.headers.userName')"></v-text-field>
                    <span v-if="!exist && user.userName!= ''" class="text-danger">{{ $t('notifications.userNotFound') }}</span>
                  </ValidationProvider>
                </div>
              </ValidationObserver>
              <v-card-actions>
                <v-btn color="primary"
                       :disabled="alertShow"
                       @click.stop="onSubmit">
                  {{ $t("buttons.submit.text") }}
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
import {Component, Vue, Ref, Watch} from "vue-property-decorator";
import {ValidationProvider, ValidationObserver} from "vee-validate";
import httpService from '../../services/http.service';
import {TranslateResult} from "vue-i18n";
import Message from "@/components/common/message.vue";
import UserModel from "@/models/user";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
    Message
  }
})

export default class ForgottenPassword extends Vue {
  name = "forgottenPassword";
  @Ref() private validator: any;

  isValid!: boolean;
  loading: boolean;
  alertShow!: boolean;
  alertMessage!: string | TranslateResult;
  alertType!: string;
  userName!: string;
  user!: UserModel;
  exist!: boolean;

  constructor() {
    super();
    this.alertShow = false;
    this.alertType = '';
    this.alertMessage = '';
    this.loading = false;
    this.user = new UserModel();
    this.exist = true;
    this.isValid = false;
  }

  checkUserName() {
    httpService.get(`api/user/getByUserName/${this.user.userName ?? ''}`).then(response => {
      if (response?.data) {
        this.exist = true;
        this.user = new UserModel(response.data)
      } else {
        this.exist = false;
      }
    }).catch(error => {
      if (error?.response?.data) {
        this.showError(error.response.data)
      } else {
        this.showError(error)
      }
    })
  }

  async onSubmit() {
    this.isValid = await this.validator.validate();
    if (this.isValid && this.exist) {
      this.sendRequest();
    }
  }

  sendRequest() {
    this.loading=true;
    httpService.post('api/account/sendResetPasswordMail', this.user).then(response => {
      if (response?.data) {
        this.loading=false;
        this.$router.push('/forgottenPassword/success');
      }
    }).catch(error => {
      this.loading=false;
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
}
</script>

<style scoped>

</style>