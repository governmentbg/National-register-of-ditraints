<template>
  <v-app-bar app clipped-right color="blue-grey" dark>
    <v-app-bar-nav-icon @click.stop="toggleSideMenu" v-if="isAuthenticated"></v-app-bar-nav-icon>
    <v-toolbar-title><a :href="baseUrl" class="homeLink">{{ $t('appTitle') }}</a></v-toolbar-title>
    <v-spacer></v-spacer>
    <div v-if="isAuthenticated">
      {{ getUserName }}
      <v-menu bottom left>
        <template v-slot:activator="{ on, attrs }">
          <v-btn dark icon v-bind="attrs" v-on="on">
            <v-icon>mdi-dots-vertical</v-icon>
          </v-btn>
        </template>
        <v-list>
          <v-list-item>
            <router-link to="/account/profile">{{ $t('common.profile') }}</router-link>
          </v-list-item>
          <v-list-item>
            <v-list-item-title @click="logout">{{ $t('common.logout') }}</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </div>
    <div v-else>
      <router-link to="/Login">{{ $t('menu.login') }}</router-link>
    </div>
  </v-app-bar>
</template>


<script lang="ts">
import {Component, Vue} from "vue-property-decorator";
import {mapActions, mapGetters, Payload} from "vuex";
import auth from "@/services/auth.service";

@Component({
  computed: {
    ...mapGetters("user", ["isAuthenticated", "getUserName"]),
  }
})
export default class MainNavbar extends Vue {
    //@Getter('basesUrl') baseUrl;
    baseUrl = '/';

  constructor() {
    super();
    this.baseUrl = this.$store.getters.baseUrl;
  }

  toggleSideMenu() {
    this.$store.dispatch("toggleSideMenu");
  }

  logout() {
    auth
        .logout()
        .then(() => {
          //this.$store.dispatch("user/removeUser");
          //this.$router.push("/");
          location.reload();
          localStorage.removeItem('appUser');
        })
        .catch((err) => console.log(err))
  }
}
</script>


<style lang="scss" scoped>
nav {
  height: 55px;
}

.homeLink {
    text-decoration: none;
}
</style>
