<template>
  <div>
    <v-row>
      <v-col>
        <v-card>
          <v-card-title>
            {{ $t("usersRoles.title") }}
            <v-spacer></v-spacer>
            <!-- <v-card-actions>
              <v-btn
                  color="primary"
                  class="mb-2"
                  dark
                  :disabled="alertShow"
                  @click="showCreate=!showCreate"
              >
                {{ $t('buttons.add.text') }}
              </v-btn>
            </v-card-actions> -->
          </v-card-title>
          <v-card-text>
            <v-row :align="'baseline'" :justify="'center'">
              <message
                :type="alertType"
                :messages="alertMessage"
                :show="alertShow"
                :duration="7000"
                @change="alertShow = !alertShow"
              ></message>
            </v-row>
            <div class="p-3">
              <v-data-table
                :headers="headers"
                :items="roles"
                :options.sync="options"
                :loading="loading"
              >
                <template v-for="header in headers">
                  {{ (header.text = $t("usersRoles.headers." + header.value)) }}
                </template>
                <template v-slot:item="{ item }">
                  <tr>
                    <td>{{ item.name }}</td>
                    <td>
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
                            @click="openUserRoleEdit(item)"
                          >
                            <v-icon dark class="btn-icon"
                              >fas fa-user-edit</v-icon
                            >
                          </v-btn>
                        </template>
                        <span>{{ $t("usersRoles.edit") }}</span>
                      </v-tooltip>
                    </td>
                  </tr>

                  <tr></tr>
                </template>
              </v-data-table>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <v-dialog v-model="showCreate" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">{{ $t("usersRoles.create") }}</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col cols="12">
                <ValidationObserver ref="validator">
                  <validation-provider
                    v-slot="{ errors }"
                    :name="$t('usersRoles.name')"
                    :rules="'required'"
                  >
                    <v-text-field
                      v-model="roleName"
                      :label="$t('usersRoles.name')"
                      :error-messages="errors"
                      @input="isUnique"
                    ></v-text-field>
                    <span v-if="exist && roleName != ''" class="text-danger">{{
                      $t("notifications.roleConflict")
                    }}</span>
                  </validation-provider>
                </ValidationObserver>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeCreate">
            {{ $t("buttons.cancel") }}
          </v-btn>
          <v-btn color="blue darken-1" text @click="save">
            {{ $t("buttons.save") }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="showEdit" max-width="1000px">
      <v-card>
        <v-card-title>
          <span class="headline">{{ $t("usersRoles.edit") }}</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col cols="12">
                <ValidationObserver ref="validator">
                  <validation-provider
                    v-slot="{ errors }"
                    :name="$t('usersRoles.name')"
                    :rules="'required'"
                  >
                    <v-text-field
                      :disabled="true"
                      v-model="selectedItem.name"
                      :label="$t('usersRoles.name')"
                      :error-messages="errors"
                      @input="isUnique"
                    ></v-text-field>
                    <span
                      v-if="exist && selectedItem.name != ''"
                      class="text-danger"
                      >{{ $t("notifications.roleConflict") }}</span
                    >
                  </validation-provider>
                  <v-autocomplete
                    :items="users"
                    v-model="selectedItem.users"
                    :label="$t('usersRoles.users')"
                    item-text="userName"
                    return-object
                    multiple
                  >
                    <template v-slot:selection="data">
                      <v-chip
                        v-bind="data.attrs"
                        :input-value="data.selected"
                        close
                        @click="data.select"
                        @click:close="removeUser(data.index)"
                      >
                        {{ data.item.userName }}
                      </v-chip>
                    </template>
                    <template v-slot:item="data">
                      <template>
                        <v-list-item-content
                          v-text="data.item.userName"
                        ></v-list-item-content>
                      </template>
                    </template>
                  </v-autocomplete>
                </ValidationObserver>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeEdit">
            {{ $t("buttons.cancel") }}
          </v-btn>
          <v-btn color="blue darken-1" text @click="saveEditedRole">
            {{ $t("buttons.save") }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Ref, Watch } from "vue-property-decorator";
import { DataOptions } from "vuetify/types/index";
import httpService from "@/services/http.service";
import { ValidationObserver, ValidationProvider } from "vee-validate";
import AspNetRole from "@/models/aspNetRole";
import UserModel from "@/models/user";
import Message from "@/components/common/message.vue";
import { TranslateResult } from "vue-i18n";

@Component({
  components: {
    ValidationObserver,
    ValidationProvider,
    Message,
  },
})
export default class UsersRoles extends Vue {
  @Ref() private validator: any;
  name = "usersRoles";
  showCreate!: boolean;
  showEdit!: boolean;
  loading!: boolean;
  roles!: Array<any>;
  roleName!: string;
  isValid!: boolean;
  exist!: boolean;
  selectedItem!: AspNetRole;
  users!: Array<any>;
  alertShow!: boolean;
  alertMessage!: string | TranslateResult;
  alertType!: string;

  constructor() {
    super();
    this.showCreate = false;
    this.showEdit = false;
    this.loading = false;
    this.roles = [];
    this.roleName = "";
    this.isValid = false;
    this.exist = false;
    this.selectedItem = new AspNetRole();
    this.users = [];
    this.alertShow = false;
    this.alertType = "";
    this.alertMessage = "";
  }

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
  headers = [
    {
      text: "UserRole",
      value: "userRole",
      align: "left",
      sortable: true,
      filterable: false,
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

  @Watch("options", { deep: true })
  onOptionsChange(value: DataOptions) {
    console.log(value);
  }

  loadItems() {
    this.loading = true;
    httpService
      .get("api/roles/getUserRoles")
      .then((response) => {
        if (response?.data) {
          this.roles = response.data;
        }
        this.loading = false;
      })
      .catch((error) => {
        this.loading = false;
        if (error?.response?.data) {
          this.showError(error.response.data);
        } else {
          this.showError(error);
        }
      })
      .then(() => {
        this.loadAllUsers();
      });
  }

  removeUser(index: number) {
    this.selectedItem.users.splice(index, 1);
  }

  closeCreate() {
    this.showCreate = false;
  }

  closeEdit() {
    this.showEdit = false;
    this.selectedItem = new AspNetRole();
  }

  async save() {
    this.isValid = await this.validator.validate();
    if (this.isValid && !this.exist) {
      this.loading = true;

      httpService
        .post("api/roles/createRole", { name: this.roleName })
        .then((response) => {
          if (response?.data) {
            this.showCreate = false;
            this.roleName = "";
            this.showSuccess(this.$t("success.successfullyCreatedEntry"));
            this.loadItems();
          }
        })
        .catch((error) => {
          if (error?.response?.data) {
            this.showError(error.response.data);
          } else {
            this.showError(error);
          }
        });
    }
  }

  openUserRoleEdit(item: any) {
    this.selectedItem = new AspNetRole(item);
    this.loadUsersByRole(item.name);
    this.showEdit = true;
  }

  async saveEditedRole() {
    this.isValid = await this.validator.validate();
    if (this.isValid && !this.exist) {
      this.loading = true;

      httpService
        .put("api/roles/editRole", this.selectedItem)
        .then((response) => {
          if (response) {
            this.showEdit = false;
            this.selectedItem = new AspNetRole();
            this.loading = false;
            this.showSuccess(this.$t("success.successfullyEditedEntry"));
            this.loadItems();
          }
        })
        .catch((error) => {
          this.loading = false;
          if (error?.response?.data) {
            this.showError(error.response.data);
          } else {
            this.showError(error);
          }
        });
    }
  }

  loadUsersByRole(name: string) {
    this.loading = true;
    httpService
      .get(`api/user/getUsersByRoleName/${name}`)
      .then((response) => {
        if (response?.data) {
          response.data.forEach((user: any) => {
            this.selectedItem.users.push(new UserModel(user));
          });
        }
        this.loading = false;
      })
      .catch((error) => {
        this.loading = false;
        if (error?.response?.data) {
          this.showError(error.response.data);
        } else {
          this.showError(error);
        }
      });
  }

  loadAllUsers() {
    this.loading = true;
    httpService
      .get(`api/user/get`)
      .then((response) => {
        if (response?.data) {
          response.data.forEach((user: any) => {
            this.users.push(new UserModel(user));
          });
        }
        this.loading = false;
      })
      .catch((error) => {
        this.loading = false;
        if (error?.response?.data) {
          this.showError(error.response.data);
        } else {
          this.showError(error);
        }
      });
  }

  isUnique() {
    if (
      this.roles.some(
        (item) =>
          item.name.toLowerCase().trim() === this.roleName.toLowerCase().trim()
      )
    ) {
      this.exist = true;
    } else {
      this.exist = false;
    }
  }

  showError(error: string) {
    this.alertMessage = error;
    this.alertType = "error";
    this.alertShow = true;
  }

  showSuccess(message: TranslateResult) {
    this.alertMessage = message;
    this.alertType = "success";
    this.alertShow = true;
  }

  @Watch("selectedItem.name")
  onSelectedItemChange(newVal: string) {
    if (newVal) {
      if (
        this.roles.some(
          (item) =>
            item.name.toLowerCase().trim() === newVal.toLowerCase().trim() &&
            this.selectedItem.id !== item.id
        )
      ) {
        this.exist = true;
      } else {
        this.exist = false;
      }
    }
  }

  mounted() {
    this.loadItems();
  }
}
</script>

<style scoped>
</style>