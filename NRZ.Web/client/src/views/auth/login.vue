<template>
  <div class="w-100">
    <v-row :align="'baseline'" :justify="'center'">
      <v-col :md="4">
        <v-card>
          <v-card-title>{{$t('login.title')}}</v-card-title>
          <div class="p-3">
            <ValidationObserver ref="validator">
              <v-form @submit.prevent="onSubmit" v-model="formValid">
                <div class="form-group">
                  <ValidationProvider
                    v-slot="{ errors }"
                    :name="$t('login.email')"
                    rules="required"
                  >
                    <v-text-field
                      class="required"
                      v-model="model.userName"
                      :label="$t('login.userName')"
                      :error-messages="errors"
                    ></v-text-field>
                  </ValidationProvider>
                </div>
                <div class="form-group">
                  <ValidationProvider
                    v-slot="{ errors }"
                    :name="$t('login.password')"
                    rules="required"
                  >
                    <v-text-field
                      class="required"
                      v-model="model.password"
                      :label="$t('login.password')"
                      :error-messages="errors"
                      :type="'password'"
                    ></v-text-field>
                  </ValidationProvider>
                </div>
                <div class="form-group text-center">
                  <v-btn type="submit" :color="'primary'">{{$t('login.submit')}}</v-btn>
                </div>
              </v-form>
            </ValidationObserver>
            <div class="divider">
              <div class="spacer"></div>
              <div class="text mx-2">{{$t('login.or')}}</div>
              <div class="spacer"></div>
            </div>
            <div class="mb-3">
              <v-row :justify="'space-around'">
                <v-btn
                  :href="certLoginUrl()"
                  :outlined="true"
                  :color="'accent'"
                  :width="170"
                >{{$t('login.certificateLogin')}}</v-btn>
                <v-btn
                  @click="eAuthLogin"
                  :outlined="true"
                  :color="'info'"
                  :width="170"
                >{{$t('login.eAuthLogin')}}</v-btn>
              </v-row>
            </div>
            <div class="row">
              <div class="col-md-6 mb-2">
                <router-link to="/register">{{$t('login.newUser')}}</router-link>
              </div>
              <div class="col-md-6 mb-2">
                <router-link class="float-md-right" to="/forgottenPassword">{{$t('login.forgotPassword')}}</router-link>
              </div>
            </div>
          </div>
        </v-card>
      </v-col>
    </v-row>

    <form id="eauth-form" :action="eAuthUrl" method="post" ref="EAuthForm">
        <input 
        hidden
        ref="samlRequest"
        type="text"
        name="SAMLRequest"
        id="SAMLRequest">
    </form>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Ref } from "vue-property-decorator";
import authService from "@/services/auth.service";
import { LoginModel } from "@/models/auth.models";
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { namespace } from "vuex-class";
import UserModel from "@/models/user";
import { EAuthRequestModel } from '@/models/eAuthRequest.model';
import { Getter } from "vuex-class";
const userNS = namespace("user");

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
  },
})
export default class Login extends Vue {
  private model: LoginModel;
  private formValid: boolean;
  // eslint-disable-next-line
  @Ref() private validator: any;
  // eslint-disable-next-line
  @userNS.Action("setUser") setUser: any;
  // eslint-disable-next-line
  @userNS.Getter('isAuthenticated') isAuthenticated: any;

  @Getter
  private eAuthUrl!: string;


  constructor() {
    super();
    this.model = new LoginModel();
    this.formValid = false;
  }

  mounted() {
    if (this.isAuthenticated) {
      this.redirect();
    }
  }

  public async onSubmit() {
    this.formValid = await this.validator.validate();
    if (this.formValid) {
      await this.login(this.model);
    }
  }

  private async login(model: LoginModel) {
    try {
        const userData: UserModel = await authService.login(model);
        const user = userData;
        this.setUser(user);
        this.redirect();
      } catch (error) {
        console.log("Login error ...");
      }
  }

  certLoginUrl() {
    return this.$store.getters.baseUrl + "/certificate/login";
  }

  private eAuthLogin() {
      authService.eAuthLogin()
      .then((result: EAuthRequestModel) => {
        console.log(result);

        (document.getElementById('SAMLRequest') as HTMLInputElement)!.value = result.SAMLRequest == null ? '' : result.SAMLRequest;

        const form = (this.$refs.EAuthForm as Vue & { submit: () => boolean });
        if (form) {
          form.submit();
        }
      }).catch((error) => {
        console.log("EAuth resquest error ...", error);  
      });
  }

  redirect() {
    const returnUrl = this.$route.query.returnUrl || "/";
    this.$router.push({ path: returnUrl.toString() });
  }
}
</script>

<style lang="scss" scoped>
.divider {
  display: flex;
  justify-content: space-between;
  align-items: center;

  .spacer {
    flex: 1;
    height: 20px;
    border-top: 1px solid lightgrey;
  }

  .text {
    height: 40px;
  }
}

.btn {
  &:hover {
    color: #fff;
  }
  color: #fff;
}
</style>