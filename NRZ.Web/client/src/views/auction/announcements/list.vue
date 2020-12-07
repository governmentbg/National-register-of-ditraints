<template>
  <v-row>
    <v-col>
      <v-card>
        <v-card-title>
          <h3 class="w-100 text-center">
            {{ $t('auctions.announcements.list.title') }}
          </h3>
        </v-card-title>
        <v-card-text>
          <div class="row" style="align-items: baseline">
            <div class="mb-3 col-md-4 col-sm-6 col-12">
              <v-text-field
                  v-model="search"
                  append-icon="mdi-magnify"
                  :label="$t('common.search_n')"
                  single-line
                  hide-details
              ></v-text-field>
            </div>
          </div>

          <div>
            <v-data-table
                :loading="isLoading"
                :headers="headers"
                :items="items"
                :search="search"
            >
              <template v-slot:[`item.registerStartDate`]="{ item }">
                {{ $moment(item.registerStartDate).format("DD.MM.YYYY HH:mm") }}
              </template>

              <template v-slot:[`item.registerEndDate`]="{ item }">
                {{ $moment(item.registerEndDate).format("DD.MM.YYYY HH:mm") }}
              </template>

              <template v-slot:[`item.auctionStartDate`]="{ item }">
                {{ $moment(item.auctionStartDate).format("DD.MM.YYYY HH:mm") }}
              </template>

              <template v-slot:[`item.auctionEndDate`]="{ item }">
                {{ $moment(item.auctionEndDate).format("DD.MM.YYYY HH:mm") }}
              </template>
              <template v-slot:[`item.actions`]="{ item }">
                <router-link :to="`/announcements/details/${item.id}`">
                  {{ $t("auctions.announcements.list.preview") }}
                </router-link>
              </template>
            </v-data-table>
          </div>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import {Vue, Component} from "vue-property-decorator";
import http from "@/services/http.service";

@Component
export default class List extends Vue {
  search = "";
  isLoading = false;
  private headers: object[] = [];
  items = [];

  getHeaders() {
    return [
      {text: this.$t('auctions.announcements.list.headers.code'), value: "code"},
      {text: this.$t('auctions.announcements.list.headers.title'), value: "title"},
      {text: this.$t('auctions.announcements.list.headers.startPrice'), value: "startPrice"},
      {text: this.$t('auctions.announcements.list.headers.actionOrder'), value: "order"},
      {text: this.$t('auctions.announcements.list.headers.propertyType'), value: "propertyType"},
      {
        text: this.$t('auctions.announcements.list.headers.registerStartDate'),
        value: "registerStartDate",
        filterable: false
      },
      {
        text: this.$t('auctions.announcements.list.headers.registerEndDate'),
        value: "registerEndDate",
        filterable: false
      },
      {
        text: this.$t('auctions.announcements.list.headers.auctionStartDate'),
        value: "auctionStartDate",
        filterable: false
      },
      {text: this.$t('auctions.announcements.list.headers.auctionEndDate'), value: "auctionEndDate", filterable: false},
      {text: this.$t('auctions.announcements.list.headers.actions'), value: "actions", sortable: false},
    ];
  }

  mounted() {
    this.headers = this.getHeaders();
    this.isLoading = true;
    http
        .get("/api/announcements/")
        .then((response) => response.data)
        .then((data) => (this.items = data))
        .catch((err) => console.log(err))
        .then(() => (this.isLoading = false));
  }
}
</script>

<style>
</style>