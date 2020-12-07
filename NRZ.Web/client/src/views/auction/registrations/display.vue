<template>
  <div>
    <v-overlay :value="isLoading">
      <v-progress-circular indeterminate size="64"></v-progress-circular>
    </v-overlay>
    <display-component v-model="model"></display-component>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import RegistrationDisplayCompanent from "@/components/auction/display.registration.vue";
import { Registration } from "@/models/autction.models";
import http from "@/services/http.service";

@Component({
  components: {
    displayComponent: RegistrationDisplayCompanent,
  },
})
export default class RegistrationDisplay extends Vue {
  private model: Registration;
  private isLoading = true;

  constructor() {
    super();
    this.model = new Registration();
  }

  mounted() {
    const { id } = this.$route.params;
    http
      .get<Registration>("/api/AuctionRegister/Get/" + id, Registration, true)
      .then((data: Registration) => (this.model = data))
      .catch((err) => console.log(err))
      .then(() => (this.isLoading = false));
  }
}
</script>

<style>
</style>