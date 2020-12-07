<template>
  <v-card>
    <div class="text-center h3">{{ $t("auctions.announcements.display.title") }}</div>
    <preview :value="model"></preview>
    <v-row>
      <v-col class="text-center">
        <v-btn
          outlined
          color="success"
          :to="`/announcements/register/${model.id}`"
          >{{ $t("auctions.announcements.display.register") }}</v-btn
        >
      </v-col>
    </v-row>
  </v-card>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import preview from "@/components/auction/preview.announcement.vue";
import { AuctionAnnouncement } from "@/models/autction.models";
import http from "@/services/http.service";

@Component({
  components: {
    preview,
  },
})
export default class DisplayAuctionForRegister extends Vue {
  constructor() {
    super();
    this.model = new AuctionAnnouncement();
  }

  model: AuctionAnnouncement;

  mounted() {
    const { id } = this.$route.params;
    http
      .get<AuctionAnnouncement>(
        "/api/announcements/" + id,
        AuctionAnnouncement,
        true
      )
      .then((data: AuctionAnnouncement) => (this.model = data))
      .catch((err) => console.log(err));
  }
}
</script>

<style>
</style>