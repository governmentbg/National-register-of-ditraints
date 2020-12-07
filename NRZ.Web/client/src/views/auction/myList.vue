<template>
  <v-row>
    <v-col>
      <v-card>
        <v-card-title>
          <h3 class="w-100 text-center">
            {{ $t("Търгове") }}
          </h3>
        </v-card-title>
        <v-card-text>
          <v-tabs v-model="tabs">
            <v-tab>{{ $t("Текущи") }}</v-tab>
            <v-tab>{{ $t("Предстоящи") }}</v-tab>
            <v-tab>{{ $t("Минали") }}</v-tab>
          </v-tabs>

          <v-tabs-items v-model="tabs">
            <v-tab-item>
              <div class="row" style="align-items: baseline">
                <div class="mb-3 col-md-4 col-sm-6 col-12">
                  <v-text-field
                    v-model="searchOngoing"
                    append-icon="mdi-magnify"
                    :label="$t('common.search_n')"
                    single-line
                    hide-details
                  ></v-text-field>
                </div>
              </div>
              <div>
                <v-data-table
                  :loading="isOngoingLoading"
                  :headers="ongoingHeaders"
                  :items="ongoingAuctions"
                  :search="searchOngoing"
                >
                  <template v-slot:[`item.announcement.code`]="{ item }">
                    <router-link
                      :to="`/announcements/details/${item.announcement.id}`"
                    >
                      {{ item.announcement.code }}
                    </router-link>
                  </template>
                  <template v-slot:[`item.startDate`]="{ item }">
                    {{ $moment(item.startDate).format("DD.MM.YYYY HH:mm") }}
                  </template>

                  <template v-slot:[`item.endDate`]="{ item }">
                    {{ $moment(item.endDate).format("DD.MM.YYYY HH:mm") }}
                  </template>
                  <template v-slot:[`item.actions`]="{ item }">
                    <router-link
                      :to="`/auctions/${item.id}`"
                      v-if="item.canParticipate"
                    >
                      {{ $t("Участвай") }}
                    </router-link>
                  </template>
                </v-data-table>
              </div>
            </v-tab-item>
            <v-tab-item>
              <div class="row" style="align-items: baseline">
                <div class="mb-3 col-md-4 col-sm-6 col-12">
                  <v-text-field
                    v-model="searchFuture"
                    append-icon="mdi-magnify"
                    :label="$t('common.search_n')"
                    single-line
                    hide-details
                  ></v-text-field>
                </div>
              </div>

              <div>
                <v-data-table
                  :loading="isFutureLoading"
                  :headers="futureHeaders"
                  :items="futureAuctions"
                  :search="searchFuture"
                >
                  <template v-slot:[`item.announcement.code`]="{ item }">
                    <router-link
                      :to="`/announcements/details/${item.announcement.id}`"
                    >
                      {{ item.announcement.code }}
                    </router-link>
                  </template>
                  <template v-slot:[`item.startDate`]="{ item }">
                    {{ $moment(item.startDate).format("DD.MM.YYYY HH:mm") }}
                  </template>

                  <template v-slot:[`item.endDate`]="{ item }">
                    {{ $moment(item.endDate).format("DD.MM.YYYY HH:mm") }}
                  </template>
                  <template v-slot:[`item.actions`]="{ item }">
                    <span>
                      {{
                        item.canParticipate ? $t("common.yes") : $t("common.no")
                      }}
                    </span>
                  </template>
                </v-data-table>
              </div>
            </v-tab-item>
            <v-tab-item>
              <div class="row" style="align-items: baseline">
                <div class="mb-3 col-md-4 col-sm-6 col-12">
                  <v-text-field
                    v-model="searchFinished"
                    append-icon="mdi-magnify"
                    :label="$t('common.search_n')"
                    single-line
                    hide-details
                  ></v-text-field>
                </div>
              </div>

              <div>
                <v-data-table
                  :loading="isFinishedLoading"
                  :headers="ongoingHeaders"
                  :items="finishedAuctions"
                  :search="searchFinished"
                >
                  <template v-slot:[`item.announcement.code`]="{ item }">
                    <router-link
                      :to="`/announcements/details/${item.announcement.id}`"
                    >
                      {{ item.announcement.code }}
                    </router-link>
                  </template>
                  <template v-slot:[`item.startDate`]="{ item }">
                    {{ $moment(item.startDate).format("DD.MM.YYYY HH:mm") }}
                  </template>

                  <template v-slot:[`item.endDate`]="{ item }">
                    {{ $moment(item.endDate).format("DD.MM.YYYY HH:mm") }}
                  </template>
                  <template v-slot:[`item.actions`]="{ item }">
                    <router-link
                      :to="`/auctions/result/${item.id}`"
                      target="_blank"
                    >
                      {{ $t("Резултат") }}
                    </router-link>
                  </template>
                </v-data-table>
              </div>
            </v-tab-item>
          </v-tabs-items>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Auction } from "@/models/autction.models";
import { Vue, Component } from "vue-property-decorator";
import http from "@/services/http.service";

@Component
export default class AuctionsList extends Vue {
  futureAuctions?: Auction[] = [];
  finishedAuctions?: Auction[] = [];
  ongoingAuctions?: Auction[] = [];
  isFutureLoading = true;
  isOngoingLoading = true;
  isFinishedLoading = true;

  searchFuture = "";
  searchFinished = "";
  searchOngoing = "";
  tabs = null;
  headers = [
    { text: "Код на обявлението", value: "announcement.code" },
    { text: "Обявление", value: "announcement.title" },
    { text: "Начало", value: "startDate", filterable: false },
    { text: "Край", value: "endDate", filterable: false },
    { text: "Начална цена", value: "startPrice" },
    { text: "Постигната цена", value: "currentPrice" },
  ];

  get ongoingHeaders() {
    return [
      ...this.headers,
      { text: "Действия", value: "actions", sortable: false },
    ];
  }

  get futureHeaders() {
    return [
      ...this.headers,
      { text: "Заявено участие", value: "actions", sortable: true },
    ];
  }

  get finishedHeaders() {
    return [
      ...this.headers,
      { text: "Заявено участие", value: "actions", sortable: true },
    ];
  }

  mounted() {
    const baseUrl = this.$store.getters.baseUrl;

    http
      .get<Auction>(`${baseUrl}/api/Auctions/future?creator=true`, Auction, true)
      .then((data: Auction[]) => {
        this.futureAuctions = data;
        this.isFutureLoading = false;
      });

    http
      .get<Auction>(`${baseUrl}/api/Auctions/ongoing?creator=true`, Auction, true)
      .then((data: Auction[]) => {
        this.ongoingAuctions = data;
        this.isOngoingLoading = false;
      });

    http
      .get<Auction>(`${baseUrl}/api/Auctions/finished?creator=true`, Auction, true)
      .then((data: Auction[]) => {
        this.finishedAuctions = data;
        this.isFinishedLoading = false;
      });
  }
}
</script>

<style>
</style>