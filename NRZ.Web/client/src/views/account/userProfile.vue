<template>
  <div>
    <v-row :align="'baseline'" :justify="'center'">
      <v-col>
        <div>
          <h3 class="text-center">{{ $t("userProfile.title") }}</h3>
        </div>
        <v-card-actions class="mb-3">
          <v-btn color="primary" @click.stop="goBack">
            {{ $t("buttons.back") }}
          </v-btn>
        </v-card-actions>
        <v-tabs>
          <v-tab>
            <div class="pr-2">
              <v-icon>fa-user-edit</v-icon>
            </div>
            {{ $t("userProfile.personalData") }}
          </v-tab>
          <v-tab>
            <div class="pr-2">
              <v-icon>fas fa-user-lock</v-icon>
            </div>
            {{ $t('userProfile.changePassword') }}
          </v-tab>
          <v-tab v-if="isAdmin">
            <div class="pr-2" >
              <v-icon>fas fa-user-tag</v-icon>
            </div>
            {{ $t('userProfile.userRoles') }}
          </v-tab>
          <v-tab-item>
            <personal-data :user-id="userId" :is-admin="isAdmin"></personal-data>
          </v-tab-item>
          <v-tab-item>
            <change-password :user-id="userId" :is-admin="isAdmin"></change-password>
          </v-tab-item>
          <v-tab-item>
            <user-roles-component :user-id="userId"></user-roles-component>
          </v-tab-item>
        </v-tabs>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import {Component, Vue, Watch} from "vue-property-decorator";
import PersonalData from '../../components/auth/personalData.vue';
import ChangePassword from '../../components/auth/changePassword.vue'
import UserRolesComponent from "@/components/auth/userRoles.vue";
import {UserRoles} from '@/models/enums';


@Component({
  components: {
    PersonalData,
    ChangePassword,
    UserRolesComponent
  }
})
export default class UserProfile extends Vue {
  name = "userProfile"
  isAdmin!: boolean;
  userId!: string;

  constructor() {
    super();
    this.isAdmin = false;
    this.userId = '';
  }

  goBack() {
    window.history.back();
  }

  @Watch("userId",{immediate:true})
  onUserIdChange(newVal: any) {
    if (newVal) {
      this.userId = newVal;
      const userRoles = this.$store.getters["user/getUserRoles"].split(", ");
      if (userRoles.some((e: string) => e.toUpperCase() === UserRoles.SysAdmin)) {
        this.isAdmin = true;
      }
      else{
        this.isAdmin=false;
      }
    }
  }

  mounted() {
    this.userId = this.$route?.params?.id ?? this.$store.getters["user/getUserId"];
  }
}
</script>

<style scoped>

</style>