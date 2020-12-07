<template>
  <v-navigation-drawer v-model="sideMenuToggle" fixed temporary>
    <v-list dense v-for="component in components" :key="component.name">
      <div v-if="canView(component.roles)">
        <div v-if="component.hasChildren === 'false'">
          <v-list-item router-link :to="component.to">
            <v-list-item-icon>
              <v-icon>{{ component.icon }}</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title :title="$t(component.name)">{{
                $t(component.name)
              }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </div>
        <div v-else>
          <v-list-group>
            <v-icon slot="prependIcon">{{ component.icon }}</v-icon>
            <template v-slot:activator>
              <v-list-item-title>{{ $t(component.name) }}</v-list-item-title>
            </template>
            <v-list
              dense
              v-for="childrenComponent in component.children"
              :key="childrenComponent.name + _uid"
            >
              <v-list-item router-link :to="childrenComponent.to" v-if="canView(childrenComponent.roles)">
                <v-list-item-icon>
                  <v-icon>{{ childrenComponent.icon }}</v-icon>
                </v-list-item-icon>
                <v-list-item-content>
                  <v-list-item-title :title="$t(childrenComponent.name)">{{
                    $t(childrenComponent.name)
                  }}</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
            </v-list>
          </v-list-group>
        </div>
      </div>
    </v-list>
  </v-navigation-drawer>
</template>

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import { Action, Getter, Mutation } from "vuex-class";
import { namespace } from "vuex-class";
import data from "@/components/sideMenu/data.json";

@Component
export default class SideMenu extends Vue {
  @Getter
  private sideMenu!: boolean;
  private components: Array<object>;
  @Action
  private setSideMenu!: (payload: boolean) => void;

  constructor() {
    super();
    this.components = data;
  }

  private get sideMenuToggle(): boolean {
    return this.sideMenu;
  }

  private set sideMenuToggle(payload: boolean) {
    this.setSideMenu(payload);
  }

  private canView(roles: string[]) {
    let canView = false;

    if(roles && roles.length){
      for (let index = 0; index < roles.length; index++) {
        const isInRole = this.$store.getters['user/hasRole'](roles[index]);
        if (isInRole) {
          canView = true;
          break;
        }        
      }
    } else {
      canView = true;
    }

    return canView;
  }
}
</script>

<style>
/* side menu */
.v-list-group
  .v-list-group__header
  .v-list-item__icon.v-list-group__header__append-icon {
  width: fit-content;
  min-width: fit-content !important;
}
</style>