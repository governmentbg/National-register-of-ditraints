<template>
  <v-app id="inspire">
    <mainNav></mainNav>
    <sideMenu></sideMenu>
    <v-main>
      <v-container class fluid>
        <router-view :key="$route.fullPath" class="w-100 p-3" />
      </v-container>
    </v-main>
    <v-footer app color="primary" class="white--text" absolute>
      <div style="flex: 1">
        <v-img
          src="./assets/logos/EU_SEFdown.png"
          aspect-ratio="1"
          class="primary"
          max-height="150"
          :contain="true"
        ></v-img>
      </div>
      <div style="flex: 5" class="px-1">
        <p
          class="text-md-body-2 text-lg-body-2 text-xl-body-2 text-sm-caption text-caption"
        >
          Този проект е създаден в рамките на договор №
          BG-05SFOP001-1.002-0018-C01/18.04.2017 г., по проект: „Разработване и
          внедряване на електронна информационна система „Национален регистър на
          запорите“, финансиран от ОП „Добро управление”, чрез ЕСФ.
        </p>
        <p
          class="text-md-body-2 text-lg-body-2 text-xl-body-2 text-sm-caption text-caption text-center"
        >
          {{ $t("common.version") }}: {{ version }}
        </p>
      </div>

      <div style="flex: 1">
        <v-img
          src="./assets/logos/logo-bg-center-no-back.png"
          aspect-ratio="1"
          class="primary"
          max-height="150"
          :contain="true"
        ></v-img>
      </div>
    </v-footer>
  </v-app>
</template>

<script lang="ts">
import Vue from "vue";
import { mapGetters, mapActions } from "vuex";
import sideMenu from "@/components/sideMenu/sideMenu.vue";
import Component from "vue-class-component";
import mainNav from "@/components/navs/mainNav.vue";
import settingsService from "@/services/settings.service";

@Component({
  components: {
    sideMenu,
    mainNav,
  },
})
export default class App extends Vue {
  version = "";

  async loadVersion() {
    try {
      const result = await settingsService.getVersion();
      this.version = result as string;
    } catch (error) {
      console.log(error);
    }
  }

  mounted() {
    this.loadVersion();
  }
}
</script>

<style lang="scss">
body {
  margin: 0;
  padding: 0;
}
a.v-btn {
  text-decoration: none;
}

.v-input.required {
  label.v-label::after {
    content: "*";
    color: red !important;
  }
}

.required::after {
  content: "*";
  color: red !important;
}

.v-toolbar.v-app-bar a {
  color: #fff;
}

/* side menu */
.v-list-group
  .v-list-group__header
  .v-list-item__icon.v-list-group__header__append-icon {
  width: fit-content;
  min-width: fit-content !important;
}
/* end */

.loader {
    border: 8px solid lightgrey;
    border-top: 8px solid #607d8b ; /* primary */
    border-radius: 50%;
    width: 50px;
    height: 50px;
    margin: 50px auto;
    animation: spin 1s linear infinite;
}

.fit-content {
  width: fit-content;
  max-width: fit-content;
}
</style>
