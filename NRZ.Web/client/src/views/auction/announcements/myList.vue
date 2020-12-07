<template>
  <v-row>
    <v-col>
      <v-card>
        <v-card-title>
          <h3 class="w-100 text-center">
            {{ $t('auctions.announcements.myList.title')}}
          </h3>
        </v-card-title>
        <v-card-text>
          <div class="row" style="align-items: baseline">
            <div class="mb-3 col-md-4 col-sm-6 col-12">
              <v-btn color="primary" to="/announcements/create"
                >{{ $t('auctions.announcements.myList.create') }}</v-btn>
            </div>
            <v-spacer></v-spacer>
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
              <template v-slot:[`item.registrationsCount`]="{ item }">
                <span v-if="item.registrationsCount === 0">{{
                  item.registrationsCount
                }}</span>
                <router-link
                  :to="`/announcements/registrations/${item.id}`"
                  v-if="item.registrationsCount > 0"
                  >{{ item.registrationsCount }}</router-link
                >
              </template>
              <template v-slot:[`item.actions`]="{ item }">
                <v-btn
                  class="m-1 p-1"
                  color="info"
                  fab
                  dark
                  x-small
                  :to="`/announcements/display/${item.id}`"
                  :title="$t('auctions.announcements.myList.details')"
                >
                  <v-icon dark class="btn-icon">fas fa-eye</v-icon>
                </v-btn>
                <v-btn
                  class="m-1 p-1"
                  color="success"
                  fab
                  dark
                  x-small
                  :to="`/announcements/edit/${item.id}`"
                  :title="$t('auctions.announcements.myList.edit')"
                >
                  <v-icon dark class="btn-icon">fas fa-edit</v-icon>
                </v-btn>
                <v-btn
                  class="m-1 p-1"
                  color="error"
                  fab
                  dark
                  x-small
                  :title="$t('auctions.announcements.myList.delete')"
                  @click="onDelete(item.id)"
                >
                  <v-icon dark class="btn-icon">fas fa-trash</v-icon>
                </v-btn>
              </template>
            </v-data-table>
          </div>
        </v-card-text>
      </v-card>
      <v-dialog v-model="showDeleteDialog" persistent width="fit-content">
        <v-card>
          <v-card-title class="headline">
            {{ $t('auctions.announcements.myList.deleteAnnouncement') }}
          </v-card-title>
          <v-card-text>
            <v-alert> </v-alert>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="success darken-1" text @click="deleteItem">
              {{ $t("common.yes") }}
            </v-btn>
            <v-btn
              color="error darken-1"
              text
              @click="showDeleteDialog = false"
            >
              {{ $t("common.no") }}
            </v-btn>
            <v-spacer></v-spacer>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import http from "@/services/http.service";

@Component
export default class List extends Vue {
  private search = "";
  private isLoading = false;
  private showDeleteDialog = false;
  private selectedItemId?: number;
  private headers: object[] = [];
  items = [];

  getHeaders(){
    return[
      { text: this.$t('auctions.announcements.list.headers.code'), value: "code" },
      { text: this.$t('auctions.announcements.list.headers.title'), value: "title" },
      { text: this.$t('auctions.announcements.list.headers.startPrice'), value: "startPrice" },
      { text: this.$t('auctions.announcements.list.headers.registerStartDate'), value: "registerStartDate", filterable: false },
      { text: this.$t('auctions.announcements.list.headers.registerEndDate'), value: "registerEndDate", filterable: false },
      { text: this.$t('auctions.announcements.list.headers.auctionStartDate'), value: "auctionStartDate", filterable: false },
      { text: this.$t('auctions.announcements.list.headers.auctionEndDate'), value: "auctionEndDate", filterable: false },
      {
        text: this.$t('auctions.announcements.list.headers.registersCount'),
        value: "registrationsCount",
        filterable: false,
      },
      { text: this.$t('auctions.announcements.list.headers.actions'), value: "actions", sortable: false },
    ];
  }

  onDelete(itemId: number) {
    this.selectedItemId = itemId;
    this.showDeleteDialog = true;
  }

  deleteItem() {
    this.showDeleteDialog = false;
    this.isLoading = true;
    http
      .delete("/api/announcements/" + this.selectedItemId)
      .then((response) => this.loadItems())
      .catch((err) => console.log(err))
      .then(() => {
        this.isLoading = false;
        this.selectedItemId = undefined;
      });
  }

  loadItems() {
    this.isLoading = true;
    http
      .get("/api/announcements/user/" + this.$store.getters["user/getUserId"])
      .then((response) => response.data)
      .then((data) => (this.items = data))
      .catch((err) => console.log(err))
      .then(() => (this.isLoading = false));
  }

  mounted() {
    this.headers = this.getHeaders();
    this.loadItems();
  }
}
</script>

<style>
</style>