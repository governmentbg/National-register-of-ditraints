<template>
  <div class="w-100">
    <v-row :align="'baseline'" :justify="'center'">
      <v-col :md="4">
        <v-card>
          <v-card-title>{{ $t("register.title") }}</v-card-title>
          <v-card-text>
            <div>
              <div class="form-group">
                <ValidationProvider
                  v-slot="{ errors }"
                  :name="$t('register.userType')"
                  rules="required"
                >
                  <v-select
                    class="required"
                    v-model="userType"
                    :items="userTypes"
                    item-text="name"
                    item-value="code"
                    :label="$t('register.userType')"
                    :error-messages="errors"
                  ></v-select>
                </ValidationProvider>
              </div>
              <div class="form-group" v-if="userType">
                <ValidationProvider
                  v-slot="{ errors }"
                  :name="$t('register.registerType')"
                  rules="required"
                >
                  <v-select
                    class="required"
                    v-model="registerType"
                    :items="userRegisterTypes"
                    item-text="name"
                    item-value="code"
                    :label="$t('register.registerType')"
                    :error-messages="errors"
                  ></v-select>
                </ValidationProvider>
              </div>
              <div v-if="registerType === 'USER'">
                <user-pass-register :userType="userType"></user-pass-register>
              </div>
              <div v-if="registerType === 'ESIGN' || registerType === 'EAUTH'">
                <div>
                  <!-- <v-btn :href="certRegisterUrl" :color="'primary'">{{
                    $t("register.certificateRegister")
                  }}</v-btn> -->

                  <ValidationObserver ref="validator">
                    <form
                      :action="certRegisterUrl"
                      method="GET"
                      @submit="onSubmit"
                    >
                      <div class="form-group" v-if="userType === 'CHSI' || userType === 'CHSIHelper'">
                        <ValidationProvider
                          v-slot="{ errors }"
                          :name="$t('register.chiNumber')"
                          rules="required"
                        >
                          <v-text-field
                            type="number"
                            v-model="chsiNumber"
                            :label="$t('register.chiNumber')"
                            :error-messages="errors"
                            name="chsiNumber"
                            class="required"
                          ></v-text-field>
                        </ValidationProvider>
                      </div>
                      <div class="form-group">
                        <ValidationProvider
                          v-slot="{ errors }"
                          :name="$t('register.email')"
                          rules="email"
                        >
                          <v-text-field
                            v-model="email"
                            :label="$t('register.email')"
                            :error-messages="errors"
                            name="email"
                          ></v-text-field>
                        </ValidationProvider>
                        <v-alert outlined type="warning" text>
                          {{ $t("notifications.certLoginOptionalEmail") }}
                        </v-alert>
                      </div>
                      <input type="hidden" v-model="userType" name="userType" />
                      <input type="hidden" v-model="language" name="lang" />
                      <div v-if="registerType === 'ESIGN'" class="text-center">
                        <v-btn type="submit" :color="'primary'">{{
                          $t("register.certificateRegister")
                        }}</v-btn>
                      </div>
                    </form>
                  </ValidationObserver>
                </div>
              </div>
              <div v-if="registerType === 'EAUTH'">
                <div class="text-center">
                  <v-btn @click="eAuthRegister" :color="'primary'">{{
                    $t("register.eAuthRegister")
                  }}</v-btn>
                </div>
              </div>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <v-overlay :value="overlay">
      <v-progress-circular
        indeterminate
        size="64"
      ></v-progress-circular>
    </v-overlay>

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
import { ValidationProvider, ValidationObserver } from "vee-validate";
import userAndPassRegisterComponent from "@/components/auth/usernameAndPass.register.vue";
import { Nomenclature } from "../../models/nomenclature";
import nomenclatureService from "@/services/nomenclature.service";
import { UserRegisterType, UserType } from "@/models/enums";
import { Getter } from "vuex-class";
import authService from "@/services/auth.service";
import { EAuthRequestModel } from '@/models/eAuthRequest.model';

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
    "user-pass-register": userAndPassRegisterComponent,
  },
})
export default class Register extends Vue {
  private userType: string | null = null;
  private userTypes: Nomenclature[] = [];
  private registerType: string | null = null;
  private registerTypes: Nomenclature[] = [];
  private email!: string;
  private chsiNumber!: number;
  private overlay = false;
  // eslint-disable-next-line
  @Ref() private validator: any;

  @Getter
  private language!: string;

  @Getter
  private eAuthUrl!: string;

  get certRegisterUrl() {
    return (
      this.$store.getters.baseUrl +
      "/certificate/register?userType=" +
      this.userType
    );
  }

  get userRegisterTypes() {
    let types = this.registerTypes;

    if (this.userType !== UserType.AUCPAR.toString()) {
      types = types.filter((x) => x.code !== UserRegisterType.USER.toString());

      if (this.registerType === UserRegisterType.USER) {
        this.registerType = null;
      }
    }

    return types;
  }

  public async onSubmit(e: Event) {
    const formValid = await this.validator.validate();
    if (!formValid) {
      e.preventDefault();
    } else {
      this.overlay = true;
    }
  }

  eAuthRegister() {
    authService.eAuthRegister(this.userType, this.chsiNumber, this.email, '')
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

  mounted() {
    nomenclatureService
      .getUserTypes()
      .then((result: Nomenclature[]) => {
        if (result) {
          this.userTypes = result;
        }
      })
      .catch((err: any) => console.log(err));

    nomenclatureService
      .getUserRegisterTypes()
      .then((result: Nomenclature[]) => {
        if (result) {
          this.registerTypes = result;
        }
      })
      .catch((err: any) => console.log(err));
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