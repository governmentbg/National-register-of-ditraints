<template>
  <div>
    <v-row>
      <v-col>
        <v-card>
          <v-card-title>
            {{ $t("users.title") }}
            <v-spacer></v-spacer>
            <v-text-field
                v-model="search"
                append-icon="mdi-magnify"
                :label="$t('users.search')"
                single-line
                hide-details
            ></v-text-field>
          </v-card-title>
          <div class="p-3">
            <v-data-table
                :headers="headers"
                :items="items"
                :search="search"
                :options.sync="options"
                :loading="loading"
            >
              <template v-for="header in headers">
                {{ (header.text = $t("users.headers." + header.value)) }}
              </template>
              <template v-slot:item="{ item }">
                <tr>
                  <td>{{ item.userName }}</td>
                  <td>{{ item.email }}</td>
                  <td>{{ $t(`common.${item.emailConfirmed.toString()}`) }}</td>
                  <td>{{ $t(`common.${(!item.deleted).toString()}`) }}</td>
                  <td>
                    {{ $t(`common.${(!item.confirmedByAdmin).toString()}`) }}
                  </td>
                  <td>
                    <v-tooltip bottom v-if="!item.deleted">
                      <template v-slot:activator="{ on, attrs }">
                        <v-btn
                            class="m-1 p-1"
                            color="error"
                            fab
                            dark
                            x-small
                            v-bind="attrs"
                            v-on="on"
                            @click="showPopup('showDeactivate', item.id)"
                        >
                          <v-icon dark class="btn-icon">
                            fas fa-user-times
                          </v-icon>
                        </v-btn>
                      </template>
                      <span>{{ $t("users.deactivate") }}</span>
                    </v-tooltip>

                    <v-tooltip bottom v-if="item.deleted">
                      <template v-slot:activator="{ on, attrs }">
                        <v-btn
                            class="m-1 p-1"
                            color="success"
                            fab
                            dark
                            x-small
                            v-bind="attrs"
                            v-on="on"
                            @click="showPopup('showActivate', item.id)"
                        >
                          <v-icon dark class="btn-icon">
                            fas fa-user-plus
                          </v-icon>
                        </v-btn>
                      </template>
                      <span>{{ $t("users.activate") }}</span>
                    </v-tooltip>

                    <v-tooltip bottom v-if="!item.confirmedByAdmin">
                      <template v-slot:activator="{ on, attrs }">
                        <v-btn
                            class="m-1 p-1"
                            color="teal"
                            fab
                            dark
                            x-small
                            v-bind="attrs"
                            v-on="on"
                            @click="showPopup('showApprove', item.id)"
                        >
                          <v-icon dark class="btn-icon"> fas fa-check</v-icon>
                        </v-btn>
                      </template>
                      <span>{{ $t("users.approve") }}</span>
                    </v-tooltip>
                    <v-tooltip bottom>
                      <template v-slot:activator="{ on, attrs }">
                        <v-btn
                            class="m-1 p-1"
                            color="success"
                            fab
                            dark
                            x-small
                            v-bind="attrs"
                            v-on="on"
                            @click="openUserProfile(item.id)"
                        >
                          <v-icon dark class="btn-icon">fas fa-user-edit</v-icon>
                        </v-btn>
                      </template>
                      <span>{{ $t("users.edit") }}</span>
                    </v-tooltip>
                  </td>
                </tr>
              </template>
            </v-data-table>
          </div>
        </v-card>
      </v-col>
    </v-row>
    <v-dialog v-model="showDeactivate" persistent width="fit-content">
      <v-card>
        <v-card-title>
          Сигурни ли сте, че искате да деактивирате този потребител?
        </v-card-title>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="success darken-1" text @click="deactivateUser">
            {{ $t("common.yes") }}
          </v-btn>
          <v-btn color="error darken-1" text @click="showDeactivate = false">
            {{ $t("common.no") }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="showActivate" persistent width="fit-content">
      <v-card>
        <v-card-title>
          Сигурни ли сте, че искате да активирате този потребител?
        </v-card-title>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="success darken-1" text @click="activateUser">
            {{ $t("common.yes") }}
          </v-btn>
          <v-btn color="error darken-1" text @click="showActivate = false">
            {{ $t("common.no") }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="showApprove" persistent width="fit-content">
      <v-card>
        <v-card-title>
          Сигурни ли сте, че искате да одобрите този потребител?
        </v-card-title>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="success darken-1" text @click="approveUser">
            {{ $t("common.yes") }}
          </v-btn>
          <v-btn color="error darken-1" text @click="showApprove = false">
            {{ $t("common.no") }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts">
import {Component, Vue, Watch} from "vue-property-decorator";
import {DataOptions} from "vuetify/types/index";
import http from "@/services/http.service";

@Component
export default class Users extends Vue {
  showActivate = false;
  showDeactivate = false;
  showApprove = false;
  selectedUserId = "";
  headers = [
    {
      text: "UserName",
      value: "userName",
      align: "left",
      sortable: true,
      filterable: false,
      groupable: true,
    },
    {
      text: "Email",
      value: "email",
      align: "left",
      sortable: true,
      filterable: true,
      groupable: true,
    },
    {
      text: "EmailConfirmed",
      value: "emailConfirmed",
      align: "left",
      sortable: true,
      filterable: true,
      groupable: true,
    },
    {
      text: "Active",
      value: "active",
      align: "left",
      sortable: true,
      filterable: true,
      groupable: true,
    },
    {
      text: "Approval",
      value: "waitingApproval",
      align: "left",
      sortable: true,
      filterable: true,
      groupable: true,
    },
    {
      text: "Actions",
      value: "actions",
      align: "left",
      sortable: false,
      filterable: false,
      groupable: false,
    },
  ];
  items = [];
  search = "";
  loading = false;
  totalItems = 0;
  options: DataOptions = {
    page: 1,
    itemsPerPage: 10,
    sortBy: [],
    sortDesc: [],
    groupBy: [],
    groupDesc: [],
    multiSort: false,
    mustSort: false,
  };

  @Watch("options", {deep: true})
  onOptionsChange(value: DataOptions) {
    console.log(value);
  }

  showPopup(name: string, userId: string) {
    (this as any)[name] = true;
    this.selectedUserId = userId;
  }

  openUserProfile(userId: string) {
    this.$router.push(`/account/profile/${userId}`)
  }

  loadData() {
    this.loading = true;
    http
        .get("/api/user/get")
        .then((res) => {

          console.log(res.data);


          this.items = res.data;
        })
        .catch((err) => console.log(err))
        .then(() => (this.loading = false));
  }

  mounted() {
    this.loadData();
  }

  approveUser() {
    this.showApprove = false;
    http
        .post("/api/user/approve/" + this.selectedUserId, {})
        .then((res) => this.loadData())
        .catch((err) => console.log(err))
        .then(() => (this.loading = false));
  }

  deactivateUser() {
    this.showDeactivate = false;
    http
        .post("/api/user/deactivate/" + this.selectedUserId, {})
        .then((res) => this.loadData())
        .catch((err) => console.log(err))
        .then(() => (this.loading = false));
  }

  activateUser() {
    this.showActivate = false;
    http
        .post("/api/user/activate/" + this.selectedUserId, {})
        .then((res) => this.loadData())
        .catch((err) => console.log(err))
        .then(() => (this.loading = false));
  }
}
</script>

<style lang="scss">
.v-icon {
  font-size: 150% !important;
}
</style>