<template>
  <div>
    <v-overlay :value="isLoading">
      <v-progress-circular indeterminate size="64"></v-progress-circular>
    </v-overlay>
    <display-component v-model="model"></display-component>
    <div v-if="canApprove">
      <v-card class="p-3">
        <v-row>
          <v-col md="12">
            <div>
              <h4>{{ $t("auctions.registrations.approve.status") }}</h4>
              <div>
                <v-alert
                    v-if="model.isApproved === null"
                    dense
                    text
                    type="warning"
                >{{ $t("auctions.registrations.approve.waitingApproval") }}
                </v-alert
                >
                <v-alert v-if="model.isApproved" dense text type="success">{{
                    $t("auctions.registrations.approve.approved")
                  }}
                </v-alert>
                <v-alert
                    v-if="model.isApproved == false"
                    dense
                    text
                    type="error"
                >{{ $t("auctions.registrations.approve.denied") }}
                  <div v-if="model.rejectReason">
                    <strong>{{ $t("auctions.registrations.approve.reason") }}</strong>
                    {{ model.rejectReason }}
                  </div>
                </v-alert>
              </div>
            </div>
          </v-col>
          <v-col md="12">
            <div class="d-flex justify-content-around">
              <v-btn
                  color="success"
                  @click="showApproveDialog = true"
                  v-if="!model.isApproved"
              >{{ $t("auctions.registrations.approve.approve") }}
              </v-btn
              >
              <v-btn
                  class=""
                  color="error"
                  @click="showRejectDialog = true"
                  v-if="model.isApproved === null || model.isApproved === true"
              >{{ $t("auctions.registrations.approve.deny") }}
              </v-btn
              >
            </div>
          </v-col>
        </v-row>
      </v-card>

      <v-dialog v-model="showApproveDialog" width="fit-content">
        <v-card>
          <v-card-title class="headline">
            {{ $t("auctions.registrations.approve.approveRegistration") }}
          </v-card-title>

          <v-card-text>
            {{ $t("") }}
          </v-card-text>

          <v-card-actions>
            <v-spacer></v-spacer>

            <v-btn color="success darken-1" text @click="onApprove">
              {{ $t("common.yes") }}
            </v-btn>

            <v-btn
                color="error darken-1"
                text
                @click="showApproveDialog = false"
            >
              {{ $t("common.no") }}
            </v-btn>
            <v-spacer></v-spacer>
          </v-card-actions>
        </v-card>
      </v-dialog>

      <v-dialog v-model="showRejectDialog" width="fit-content">
        <v-card>
          <v-card-title class="headline">
            {{ $t("auctions.registrations.approve.denyRegistration") }}
          </v-card-title>

          <v-card-text>
            <v-text-field
                v-model="rejectReason"
                :label="$t('auctions.registrations.approve.reason')"
            ></v-text-field>
          </v-card-text>

          <v-card-actions>
            <v-spacer></v-spacer>

            <v-btn color="success darken-1" text @click="onReject">
              {{ $t("common.yes") }}
            </v-btn>

            <v-btn
                color="error darken-1"
                text
                @click="showRejectDialog = false"
            >
              {{ $t("common.no") }}
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
      <v-snackbar v-model="showMessage" :timeout="3000" color="success">
        {{ $t("auctions.registrations.approve.editSuccess") }}
      </v-snackbar>
    </div>
  </div>
</template>

<script lang="ts">
import {Vue, Component} from "vue-property-decorator";
import RegistrationDisplayCompanent from "@/components/auction/display.registration.vue";
import {Registration} from "@/models/autction.models";
import http from "@/services/http.service";

@Component({
  components: {
    displayComponent: RegistrationDisplayCompanent,
  },
})
export default class RegistrationDisplay extends Vue {
  private model: Registration;
  private isLoading = true;
  private showApproveDialog = false;
  private showRejectDialog = false;
  private rejectReason = "";
  private showMessage = false;

  constructor() {
    super();
    this.model = new Registration();
  }

  get canApprove() {
    return this.model.announcementCreatedBy === this.$store.getters['user/getUserId'];
  }

  onApprove() {
    this.updateStatus(true);
    this.showApproveDialog = false;
  }

  onReject() {
    this.updateStatus(false);
    this.showRejectDialog = false;
  }

  updateStatus(approved: boolean) {
    this.isLoading = true;
    const data = {
      approved,
      rejectReason: this.rejectReason,
      entityId: this.model.id,
    };
    http
        .post("/api/AuctionRegister/Approve/", data)
        .then(() => {
          this.model.isApproved = data.approved;
          this.model.rejectReason = data.rejectReason;
          this.rejectReason = "";
          this.showMessage = true;
        })
        .catch((err) => console.log(err))
        .then(() => (this.isLoading = false));
  }

  mounted() {
    const {id} = this.$route.params;
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