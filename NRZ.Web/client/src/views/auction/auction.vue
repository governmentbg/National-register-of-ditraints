<template>
  <div>
    <v-card v-if="!isLoading">
      <v-card-title
        ><h2>{{ $t("auctions.auction.title") }}: {{ auction.announcement.title }}</h2>
      </v-card-title>
      <v-card-text>
        <h4>{{ $t("auctions.auction.saleItems") }}</h4>
        <properties-list
          :propertyType="propertyType.Other"
          :items="auction.announcement.items"
          :selectable="false"
        ></properties-list>
        <v-row v-if="auction && !auction.completed">
          <v-col cols="12" md="6" sm="12">
            <v-row>
              <v-col md="12">
                <h5>
                  {{ $t("auctions.auction.auctionEnd") }} <strong>{{ endTime }}</strong>
                </h5>
              </v-col>
              <v-col md="12">
                <h5>
                  {{ $t("auctions.auction.startPrice") }}
                  <strong>{{ auction.startPrice }}</strong>
                </h5>
              </v-col>
              <v-col md="12">
                <h5>
                  {{ $t("auctions.auction.step") }} <strong>{{ auction.bidStep }}</strong>
                </h5>
              </v-col>
            </v-row>
          </v-col>
          <v-col cols="12" md="6" sm="12" v-if="connected">
            <v-row>
              <v-col md="12">
                <h5>{{$t('auctions.auction.timeLeft')}}</h5>
                <div class="w-100"> 
                  <timer :target="auction.timeSpan"></timer>
                </div>
              </v-col>
              <v-col md="12">
                <h5>
                   {{ $t("auctions.auction.currentHighestPrice") }}
                  <strong
                    class="px-1 py-2"
                    :class="{
                      green: auction.currentPrice == lastBid,
                      red: auction.currentPrice > lastBid,
                    }"
                    >{{
                      auction.currentPrice ? (auction.currentPrice +  $t("auctions.auction.currency")) : $t("auctions.auction.noBidRegistered")
                    }}</strong
                  >
                </h5>
                <div
                  class="d-flex align-items-center justify-content-between"
                  style="width: 300px"
                >
                  <v-btn
                    class="mx-2"
                    fab
                    small
                    color="error"
                    @click="decreaseBidPrice"
                    :disabled="!canDecrease"
                  >
                    <v-icon> mdi-minus </v-icon>
                  </v-btn>
                  <v-text-field
                    type="number"
                    :hint="$t('auctions.auction.minimalPrice') + `${minBid}`"
                    :min="auction.minBid"
                    v-model="myBid"
                  ></v-text-field>
                  <v-btn
                    class="mx-2"
                    fab
                    small
                    color="info"
                    @click="increaseBidPrice"
                  >
                    <v-icon> mdi-plus </v-icon>
                  </v-btn>
                  <v-btn
                    color="success"
                    :disabled="!bidBtnActive"
                    @click="makeBid"
                    >{{ $t("auctions.auction.bid") }}</v-btn
                  >
                </div>
              </v-col>
            </v-row>
          </v-col>
          <v-col cols="12" md="6" sm="12">
            <h4>{{ $t("auctions.auction.logs") }}</h4>
            <div class="w-100">
              <div
                v-for="(log, index) in logs.slice().reverse()"
                :key="'log' + index"
              >
                {{
                  `[${$moment(log.date).format("DD.MM.YYY HH:mm:ss")}] ${
                    log.text
                  }`
                }}
              </div>
            </div>
          </v-col>
        </v-row>
        <v-row v-if="auction && auction.completed" class="text-center">
          <v-col md="12">
            <h4>{{ $t("auctions.auction.auctionOver") }}</h4>
          </v-col>
          <v-col md="12">
            <h5>
              {{ $t("auctions.auction.finalPrice") }}
              <strong>{{ auction.endPrice }} {{ $t("auctions.auction.currency") }}</strong>
            </h5>
          </v-col>
          <v-col md="12" v-if="auction.winner === currentUserId">
            <h5>
              {{ $t("auctions.auction.finalPrice") }}
              <strong>{{ auction.endPrice }} {{ $t("auctions.auction.currency") }}</strong>
            </h5>
          </v-col>
          <v-col md="12" v-if="auction.winner.userId === currentUserId">
            <v-alert dense text type="success" icon="fa fa-trophy">
              <h5>{{ $t("auctions.auction.auctionWinner") }}</h5>
              <div>
                {{ $t("auctions.auction.additionalInfo") }}
              </div>
            </v-alert>
          </v-col>
          <v-col md="12" v-if="auction.winner.userId != currentUserId">
            <v-alert dense text type="error" icon="fa fa-frown">
              <h5>{{ $t("auctions.auction.auctionLoser") }}</h5>
            </v-alert>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
    <v-overlay :value="isLoading">
      <v-progress-circular indeterminate size="64"></v-progress-circular>
    </v-overlay>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch } from "vue-property-decorator";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@aspnet/signalr";
import { Auction } from "@/models/autction.models";
import http from "@/services/http.service";
import PropertiesList from "@/components/properties/propertiesShortDisplayList.vue";
import Timer from '@/components/timers/timer.vue';
import { PropertyType } from "@/models/enums";

@Component({
  components: {
    PropertiesList,
    Timer
  },
  computed: {
    propertyType: () => PropertyType,
  },
})
export default class AuctionView extends Vue {
  private auctionId?: number;
  auction: Auction | null = null;
  connection: HubConnection;
  connected = false;
  isLoading = true;
  myBid = 0;
  lastBid = 0;
  logs: Array<object> = [];

  get minBid() {
    if (this.auction!.currentPrice) {
      return this.auction!.currentPrice + this.auction!.bidStep;
    } else {
      return this.auction!.startPrice + this.auction!.bidStep;
    }
  }
  get bidBtnActive() {
    return this.myBid && this.myBid >= this.minBid;
  }

  get hubUrl() {
    return this.$store.getters.baseUrl + "/auctions/hub";
  }
  get canDecrease() {
    const tempBid = this.myBid - this.auction!.bidStep;
    const canDecrease =
      tempBid - (this.auction!.currentPrice || this.auction!.startPrice) >=
      this.auction!.bidStep;
    return canDecrease;
  }

  get currentUserId() {
    return this.$store.getters["user/getUserId"];
  }

  constructor() {
    super();

    this.connection = new HubConnectionBuilder()
      .withUrl(this.hubUrl, {
        accessTokenFactory: () => this.$store.getters["user/getAuthToken"],
      })
      .configureLogging(LogLevel.Information)
      .build();

    this.connection.onclose(() => {
      this.log("Connection closed ....");
      this.connected = false;
      this.log("Reconnectiong ...");
      this.connect();
    });

    this.connection.on("HighestBid", this.onHighestBidChange);
    this.connection.on("InvalidBid", this.onInvalidBid);
    this.connection.on("TimeExtend", this.onTimeExtend);
    this.connection.on("InvalidAuction", this.onInvalidAuction);
    this.connection.on("AuctionEnd", this.onAuctionEnd);
  }

  @Watch("myBid")
  onBidChange(newVal: number | string) {
    if (typeof newVal == "string") {
      this.myBid = parseFloat(newVal);
    }
  }

  log(text: string) {
    this.logs.push({
      date: new Date(),
      text,
    });
  }

  increaseBidPrice() {
    this.myBid += this.auction!.bidStep;
  }

  decreaseBidPrice() {
    if (this.canDecrease) {
      this.myBid -= this.auction!.bidStep;
    }
  }

  connect() {
    if (this.auction != null) {
      this.log("starting connection...");
      this.connection
        .start()
        .then(() => {
          console.log("connected...");
          this.log("connected...");
          this.connected = true;
          this.connection.invoke("AddToGroup", this.auction?.id);
        })
        .catch((err: any) => {
          this.log(`[ERROR] Failed to connect with hub. ${err}`)
        });
    }
  }

  get endTime() {
    return this.$moment(this.auction!.endDate).format("DD.MM.YYYY HH:mm");
  }

  loadAuctionData(id: string) {
    http
      .get<Auction>(
        this.$store.getters.baseUrl + "/api/auctions/" + id,
        Auction,
        true
      )
      .then((data: Auction) => {
        this.auction = data;
        const price = this.auction.currentPrice || this.auction.startPrice;
        this.myBid = price + this.auction.bidStep;
        this.connect();
      })
      .catch((error) => console.log(error))
      .then(() => (this.isLoading = false));
  }

  makeBid() {
    this.connection
      .send("Bid", this.auction!.id, this.myBid, new Date())
      .then(() => this.log("Send bid..."));
    this.lastBid = this.myBid;
  }

  onHighestBidChange(bid: number) {
    this.auction!.currentPrice = bid;
    this.log(`New highest bid ${bid} received ...`);
  }

  onInvalidBid(msg: string) {
    this.log("Invalid bid: " + msg);
  }

  onTimeExtend(date: Date) {
    this.auction!.endDate = date;
    this.log("Time extended to: " + this.$moment(date).format("DD.MM.YYYY HH:mm"));
  }

  onInvalidAuction() {
    console.log("Invalid auction");
    this.log("Not allowed to participate in auction ID=" + this.auction!.id);
  }

  onAuctionEnd(data: any) {
    if (data.auctionId == this.auction!.id) {
      this.auction!.completed = true;
      this.auction!.winner = data.winnerId;
      this.auction!.endPrice = data.maxPrice;
    }
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

<style lang="scss">
.green {
  background: rgb(114, 235, 168);
  border-radius: 5px;
  color: white;
}

.red {
  background: rgb(235, 114, 114);
  border-radius: 5px;
  color: white;
}
</style>