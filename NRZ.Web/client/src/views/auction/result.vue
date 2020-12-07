<template>
  <v-card>
    <div class="text-center h3">{{ $t("Протокол за проведена електронна публична продан") }}</div>
    <v-row>
      <v-col >
        <h5>Обчвление</h5>
      </v-col>
    </v-row>
    <v-row>
      <v-col >
        <h5>Регистрации</h5>
      </v-col>
    </v-row>
    <v-row>
      <v-col >
        <h5>Резултат</h5>
      </v-col>
    </v-row>
  </v-card>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import preview from "@/components/auction/preview.announcement.vue";
import { Auction } from "@/models/autction.models";
import http from "@/services/http.service";

@Component({
  components: {
    preview,
  },
})
export default class DisplayAuction extends Vue {
  constructor() {
    super();
    this.model = new Auction();
  }

  model: Auction;

  loadAuctionData(id: string) {
    http
      .get<Auction>("/api/Auction/" + id, Auction, true)
      .then((data: Auction) => (this.model = data))
      .catch((err) => console.log(err));
  }

  mounted() {
    const { id } = this.$route.params;

    if (id != undefined) {
      this.loadAuctionData(id);
    } else {
      this.$router.back();
    }
  }
}
</script>

<style>
</style>