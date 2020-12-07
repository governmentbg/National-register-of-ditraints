<template>
  <div>
    <v-row :align="'baseline'" :justify="'center'">
      <v-col cols="8">
        <v-card>
          <v-card-title>{{ $t("userProfile.userRoles") }}</v-card-title>
          <v-card-text>
            <v-row :align="'baseline'" :justify="'center'">
              <message :type="alertType"
                       :messages="alertMessage"
                       :show="alertShow"
                       :duration="7000"
                       @change="alertShow = !alertShow"></message>
            </v-row>
            <v-autocomplete
                :items="roles"
                v-model="user.roles"
                :label="$t('userProfile.userRoles')"
                :loading="loading"
                item-text="name"
                return-object
                multiple>
              <template v-slot:selection="data">
                <v-chip
                    v-bind="data.attrs"
                    :input-value="data.selected"
                    close
                    @click="data.select"
                    @click:close="remove(data.index)"
                >
                  {{ data.item.name }}
                </v-chip>
              </template>
              <template v-slot:item="data">
                <template>
                  <v-list-item-content v-text="data.item.name"></v-list-item-content>
                </template>
              </template>
            </v-autocomplete>
          </v-card-text>
          <v-card-actions>
            <v-btn color="primary"
                   @click.stop="onSubmit">
              {{ $t("buttons.save") }}
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import {Vue, Component, Prop, Watch} from 'vue-property-decorator';
import httpService from "@/services/http.service";
import {TranslateResult} from "vue-i18n";
import UserModel from "@/models/user";
import Message from "@/components/common/message.vue";

@Component({
  components: {
    Message
  }
})

export default class UserRoles extends Vue {
  name = "userRoles";
  @Prop() userId!: string;

  //userRoles!: Array<string>;
  loading!: boolean;
  roles!: Array<any>
  alertShow!: boolean;
  alertMessage!: string | TranslateResult;
  alertType!: string;
  user!: UserModel;

  constructor() {
    super();
    this.roles = [];
    this.loading = false;
   // this.userRoles = [];
    this.alertShow = false;
    this.alertType = '';
    this.alertMessage = '';
    this.user = new UserModel();
  }

  onSubmit() {
    this.loading = true;
    httpService.put('api/roles/editUserRoles', this.user).then(response => {
      if (response) {
        this.loading = false;
        this.showSuccess(this.$t('success.successfullyEditedEntry'));
      }
    }).catch(error => {
      this.loading = false;
      if (error?.response?.data) {
        this.showError(error.response.data)
      } else {
        this.showError(error)
      }
    })
  }

  loadRoles() {
    this.loading = true;
    httpService.get('api/roles/getUserRoles').then(response => {
      if (response?.data) {

        this.roles = response.data;
      }
      this.loading = false;
    }).catch(error => {
      this.loading = false;
      if (error?.response?.data) {
        this.showError(error.response.data)
      } else {
        this.showError(error)
      }
    })
  }

  loadUserRoles() {
    this.loading = true;
    httpService.get(`api/roles/GetRolesByUserId/${this.userId}`).then(response => {
      if (response?.data) {
        this.user.roles = response.data;
      }
      this.loading = false;
    }).catch(error => {
      this.loading = false;
      if (error?.response?.data) {
        this.showError(error.response.data)
      } else {
        this.showError(error)
      }
    })
  }

  remove(index: number) {
    this.user.roles.splice(index, 1);
  }

  mounted() {
    this.loadRoles();
  }

  @Watch("userId", {immediate: true})
  onUserIdChange(newVal: any) {
    if (newVal) {
      this.userId = newVal;
      this.user.id = newVal;
      this.loadUserRoles();
    }
  }

  showError(error: string) {
    this.alertMessage = error;
    this.alertType = 'error'
    this.alertShow = true;
  }

  showSuccess(message: TranslateResult) {
    this.alertMessage = message;
    this.alertType = 'success'
    this.alertShow = true;
  }
}
</script>

<style scoped>

</style>