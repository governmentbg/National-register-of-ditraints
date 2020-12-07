<template>
  <ValidationObserver ref="validator">
    <v-form @submit.prevent="onSubmit" autocomplete="false">
      <div class="form-group">
        <ValidationProvider
          v-slot="{ errors }"
          :name="$t('register.userName')"
          rules="required"
        >
          <v-text-field
            v-model="model.userName"
            :label="$t('register.userName')"
            :error-messages="errors"
            class="required"
          ></v-text-field>
        </ValidationProvider>
      </div>
      <div class="form-group">
        <ValidationProvider
          v-slot="{ errors }"
          :name="$t('register.email')"
          rules="required|email"
        >
          <v-text-field
            v-model="model.email"
            :label="$t('register.email')"
            :error-messages="errors"
            class="required"
          ></v-text-field>
        </ValidationProvider>
      </div>
      <div class="form-group">
        <ValidationProvider
          v-slot="{ errors }"
          :name="$t('register.firstName')"
          rules="required"
        >
          <v-text-field
            v-model="model.firstName"
            :label="$t('register.firstName')"
            :error-messages="errors"
            class="required"
          ></v-text-field>
        </ValidationProvider>
      </div>
      <div class="form-group">
        <ValidationProvider
          v-slot="{ errors }"
          :name="$t('register.middleName')"
          rules
        >
          <v-text-field
            v-model="model.middleName"
            :label="$t('register.middleName')"
            :error-messages="errors"
          ></v-text-field>
        </ValidationProvider>
      </div>
      <div class="form-group">
        <ValidationProvider
          v-slot="{ errors }"
          :name="$t('register.lastName')"
          rules="required"
        >
          <v-text-field
            v-model="model.lastName"
            :label="$t('register.lastName')"
            :error-messages="errors"
            class="required"
          ></v-text-field>
        </ValidationProvider>
      </div>
      <div class="form-group">
        <ValidationProvider
          v-slot="{ errors }"
          :name="$t('register.password')"
          rules="required"
        >
          <v-text-field
            v-model="model.password"
            :label="$t('register.password')"
            :error-messages="errors"
            class="required"
            :type="'password'"
            autocomplete="new-password"
          ></v-text-field>
        </ValidationProvider>
      </div>
      <div class="form-group">
        <ValidationProvider
          v-slot="{ errors }"
          :name="$t('register.repeatPassword')"
          rules="required"
        >
          <v-text-field
            v-model="model.repeatPassword"
            :label="$t('register.repeatPassword')"
            :error-messages="errors"
            :type="'password'"
            class="required"
          ></v-text-field>
        </ValidationProvider>
      </div>
      <div class="form-group text-center">
        <v-btn type="submit" :color="'primary'">{{
          $t("register.submit")
        }}</v-btn>
      </div>
    </v-form>
  </ValidationObserver>
</template>

<script lang="ts">
import { Component, Vue, Ref, Prop } from "vue-property-decorator";
import authService from "@/services/auth.service";
import { RegisterModel } from "@/models/auth.models";
import { ValidationProvider, ValidationObserver } from "vee-validate";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver
  }
})
export default class UserAndPassregister extends Vue {
  private model = new RegisterModel();
  @Ref("validator")
  private validator!: InstanceType<typeof ValidationObserver>;

  @Prop()
  userType!: string;

  async onSubmit() {
    const formValid = await this.validator.validate();

    if (formValid && this.userType) {
      try {
          this.model.userType = this.userType;
        const userData = await authService.register(this.model);
        localStorage.setItem('user', JSON.stringify(userData));
        this.$store.dispatch("user.setUser", userData);

        this.$router.push({ path: "/register/success" });
      } catch (error) {
        console.log("Login error ...");
      }
    }
  }
}
</script>

<style>
</style>