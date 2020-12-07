<template>
  <v-card>
    <v-card-title>
      <h1>{{ $t("systemLogs.title") }}</h1>
      <v-spacer></v-spacer>
      <v-layout>
        <template>
          <v-row>
            <v-col :cols="5.5">
              <v-spacer></v-spacer>
              <v-text-field
                v-model="search"
                :label="$t('systemLogs.search')"
                single-line
                hide-details
              >
              </v-text-field>
            </v-col>
            <v-col :cols="5.5">
              <v-menu
                ref="menu"
                v-model="menu"
                :close-on-content-click="false"
                :nudge-right="40"
                :return-value.sync="dateRange"
                transition="scale-transition"
                offset-y
                min-width="250px"
              >
                <template v-slot:activator="{ on }">
                  <v-text-field
                    v-model="dateRange"
                    :label="$t('systemLogs.dateRange')"
                    prepend-icon="event"
                    readonly
                    v-on="on"
                  ></v-text-field>
                </template>
                <v-date-picker v-model="dateRange" range>
                  <v-spacer></v-spacer>
                  <v-btn
                    color="primary"
                    @click="(menu = false), $refs.menu.save([])"
                    >Cancel</v-btn
                  >
                  <v-btn color="primary" @click="$refs.menu.save(dateRange)"
                    >OK</v-btn
                  >
                </v-date-picker>
              </v-menu>
            </v-col>
             <v-col :cols="1">
              <v-btn rounded @click="searchLogs(options), (options.page = 1)">
                <v-icon> mdi-magnify </v-icon>
              </v-btn>
            </v-col>
          </v-row>
        </template>
      </v-layout>
    </v-card-title>
    <template>
      <v-data-table
        :headers="headers"
        :items="systemLogs"
        :options.sync="options"
        :page.sync="page"
        :footer-props="{
          itemsPerPageOptions: itemsPerPageOptions,
          itemsPerPageText: $t('systemLogs.itemsPerPage'),
        }"
        :server-items-length="totalItems"
        class="elevation-1"
      >
        <template v-for="header in headers">
          {{ (header.text = $t("systemLogs.headers." + header.value)) }}
        </template>
        <template v-slot:item="{ item }">
          <tr>
            <td>{{ item.actionName }}</td>
            <td>{{ item.controller }}</td>
            <td class="truncate" :title="item.data">{{ item.data }}</td>
            <td>{{ item.id }}</td>
            <td>{{ item.message }}</td>
            <td>{{ item.requestMethod }}</td>
            <td>{{ item.result }}</td>
            <td>
              {{ moment(item.serverTimeUtc).format("DD.MM.YYYY HH:mm:ss") }}
            </td>
            <td>{{ item.userId }}</td>
            <td>{{ item.ip }}</td>
            <td>{{ item.duration }}</td>
            <td>{{ item.responseStatusCode }}</td>
          </tr>
        </template>
      </v-data-table>
    </template>
  </v-card>
</template>

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import data from "@/views/admin/data.json";
import moment from "moment";
import GridSearchModel from "@/models/gridSearchModel";
import httpService from "@/services/http.service";

@Component
export default class SystemLogs extends Vue {
  private response: object = {};
  private systemLogs: Array<object> = [];
  private headers: Array<object> = data;
  private itemsPerPageOptions: Array<number> = [5, 10, 15];
  private search: string;

  private dateRange: Array<Date>;
  private pageSize: number;
  private totalItems: number;
  private page: number;

  private options: object;
  private moment: object;

  private menu: boolean;

  constructor() {
    super();
    this.search = "";
    this.pageSize = 5;
    this.totalItems = 5;
    this.page = 1;

    this.options = { page: 1, itemsPerPage: 5 };

    this.moment = moment;
    this.dateRange = [];
    this.menu = false;
  }

  @Watch("options", { deep: true })
  onOptionsChange(value: object) {
    this.searchLogs(value);
  }

  async searchLogs(options: any) {
    //If options.sortBy is not specified it returns undefined, so the model on the server can't be mapped, if it is undefined, it must be changed to "".
    const sortBy = options.sortBy[0] || "";
    //Same here.
    const sortDesc = options.sortDesc[0] || false;

    const startDate = this.dateRange[0];
    const endDate = this.dateRange[1];

    const gridSearchModel = new GridSearchModel(
      this.search,
      sortBy,
      sortDesc,
      options.page,
      options.itemsPerPage,
      startDate,
      endDate
    );

    let response;

    try {
      response = await httpService.post(
        `/api/userlogs/Search`,
        gridSearchModel
      );
      this.systemLogs = response.data.items;
      this.page = response.data.page;
      this.pageSize = response.data.pageSize;
      this.totalItems = response.data.total;
    } catch (err) {
      console.log(err.message);
    }
  }
}
</script>

<style>
.truncate {
  max-width: 200px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>