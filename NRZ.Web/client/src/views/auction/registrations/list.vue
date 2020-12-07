<template>
  <v-row>
    <v-col>
      <v-card>
        <v-card-title>
          <h3 class="w-100 text-center">
            {{ $t("auctions.registrations.list.title") }}
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
              <template v-slot:[`item.isApproved`]="{ item }">
                {{ $t(`common.${item.isApproved ? "yes" : "no"}`) }}
              </template>
              <template v-slot:[`item.participant`]="{ item }">
                {{
                  `${item.participant.firstName} ${item.participant.lastName}`
                }}
              </template>
              <template v-slot:[`item.announcementCode`]="{ item }">
                <router-link
                  :to="'/announcements/display/' + item.announcementId"
                  >{{ item.announcementCode }}</router-link
                >
              </template>
              <template v-slot:[`item.actions`]="{ item }">
                <router-link
                  :to="`/announcements/registration/approve/${item.id}`"
                >
                  {{ $t("auctions.registrations.list.preview") }}
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
import { Vue, Component } from "vue-property-decorator";
import http from "@/services/http.service";

@Component
export default class List extends Vue {
  search = "";
  isLoading = false;
  items = [];

  get headers() {
    return [
      {
        text: this.$t("auctions.registrations.list.headers.uniqueNumber"),
        value: "uniqueNumber",
      },
      {
        text: this.$t("auctions.registrations.list.headers.participant"),
        value: "participant",
      },
      {
        text: this.$t("auctions.registrations.list.headers.announcementCode"),
        value: "announcementCode",
      },
      {
        text: this.$t("auctions.registrations.list.headers.announcementTitle"),
        value: "announcementTitle",
      },
      {
        text: this.$t("auctions.registrations.list.headers.isApproved"),
        value: "isApproved",
      },
      {
        text: this.$t("auctions.registrations.list.headers.actions"),
        value: "actions",
        sortable: false,
      },
    ];
  }

  mounted() {
    this.isLoading = true;
    const { id } = this.$route.params;

    let url = "/api/AuctionRegister/";

    if (id) {
      url += `${id}/`;
    }

    url += "Creator/" + this.$store.getters["user/getUserId"];

    http
      .get(url)
      .then((response) => response.data)
      .then((data) => {
        if (data && data.length) {
          this.items = data;
        }
      })
      .catch((err) => console.log(err))
      .then(() => (this.isLoading = false));
  }
}
</script>

<style>
</style>