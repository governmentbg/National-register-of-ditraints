<template>
  <v-row>
    <v-col>
      <v-card>
        <v-card-title class="text-center">{{$t('Ново обявление за електронна публична продан')}}</v-card-title>
        <v-card-text>
          <v-stepper v-model="currentStep">
            <v-stepper-header>
              <v-stepper-step step="1" :complete="step1Completed">
                {{ $t("createDistraint.stepSearchProperty") }}
              </v-stepper-step>
              <v-divider></v-divider>
              <v-stepper-step step="2" :complete="step2Completed">
                {{ "Данни за търга" }}
              </v-stepper-step>
              <v-divider></v-divider>
              <v-stepper-step step="3" :complete="step3Completed">
                {{ $t("Създаване") }}
              </v-stepper-step>
            </v-stepper-header>

            <v-stepper-items>
              <v-stepper-content step="1">
                <search-property v-model="auction.items"></search-property>
                <div>
                  <v-btn
                    :disabled="!auction.items.length"
                    class="float-right"
                    color="info"
                    @click="
                      currentStep = 2;
                      step1Completed = true;
                    "
                    >{{ $t("createDistraint.btnStepContinue") }}</v-btn
                  >
                </div>
              </v-stepper-content>

              <v-stepper-content step="2">
                <announcement-props
                  v-model="auction"
                  ref="auctionProps"
                ></announcement-props>
                <div class="mt-3 w-100">
                  <v-btn class="float-right" color="info" @click="onStep2Next">
                    {{ $t("createDistraint.btnStepContinue") }}
                  </v-btn>

                  <v-btn
                    @click="
                      currentStep = 1;
                      step2Completed = false;
                    "
                  >
                    {{ $t("createDistraint.btnBack") }}
                  </v-btn>
                </div>
              </v-stepper-content>

              <v-stepper-content step="3">
                <Preview v-model="auction"></Preview>
                <v-col md="12" class="text-center">
                  <v-btn color="primary" @click="onSubmit">{{
                    $t("Създай")
                  }}</v-btn>
                </v-col>
                <v-col md="12">
                  <v-btn
                    @click="
                      currentStep = 2;
                      step3Completed = false;
                    "
                  >
                    {{ $t("createDistraint.btnBack") }}
                  </v-btn>
                </v-col>
              </v-stepper-content>
            </v-stepper-items>
          </v-stepper>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Component, Ref, Vue } from "vue-property-decorator";
import SearchProperty from "@/components/auction/propertySearch.announcement.vue";
import AnnouncementProps from "@/components/auction/props.announcement.vue";
import Preview from "@/components/auction/preview.announcement.vue";
import { AuctionAnnouncement, AuctionItem } from "@/models/autction.models";
import http from "@/services/http.service";

@Component({
  components: {
    SearchProperty,
    AnnouncementProps,
    Preview,
  },
})
export default class CreateAuction extends Vue {
  constructor() {
    super();
    this.auction = new AuctionAnnouncement();
  }

  private currentStep = 1;
  private step1Completed = false;
  private step2Completed = false;
  private step3Completed = false;
  private isSaving = false;

  @Ref()
  private auctionProps!: InstanceType<typeof AnnouncementProps>;

  private auction: AuctionAnnouncement;

  async onStep2Next() {
    if (await this.auctionProps.validate()) {
      this.currentStep = 3;
      this.step2Completed = true;
    }
  }

  onSubmit() {
    this.isSaving = true;
    const data = new FormData();

    data.append("Code", this.auction.code);
    data.append("Title", this.auction.title);
    data.append("PropertyType", this.auction.propertyType);
    data.append("StartPrice", this.auction.startPrice.toString());
    data.append("BidStep", this.auction.bidStep.toString());
    data.append("Description", this.auction.description);
    data.append("Order", this.auction.order);

    data.append(
      "RegisterStartDate",
      this.auction.registerStartDate
        ? this.auction.registerStartDate.toJSON()
        : ""
    );
    data.append(
      "RegisterEndDate",
      this.auction.registerEndDate ? this.auction.registerEndDate.toJSON() : ""
    );
    data.append(
      "AuctionStartDate",
      this.auction.auctionStartDate
        ? this.auction.auctionStartDate.toJSON()
        : ""
    );
    data.append(
      "AuctionEndDate",
      this.auction.auctionEndDate ? this.auction.auctionEndDate.toJSON() : ""
    );

    for (let index = 0; index < this.auction.items.length; index++) {
      const element = this.auction.items[index] as AuctionItem;

      data.append(`items[${index}].NRZId`, element.id ? element.id.toString() : '');
      data.append(`items[${index}].Title`, element.title);
      data.append(`items[${index}].NRZId`, element.description);
      data.append(`items[${index}].PropertyType`, this.auction.propertyType);
    }

    //attachments
    if (this.auction.attachments.length) {
      for (let index = 0; index < this.auction.attachments.length; index++) {
        const attachment = this.auction.attachments[index];
        data.append(`files`, attachment.file);
      }
    }

    http
      .post("/api/Announcements", data)
      .then(() => this.$router.push("/auctions"))
      .catch((err) => console.log(err));
  }
}
</script>

<style>
</style>