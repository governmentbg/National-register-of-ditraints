<template>
  <div class-text="center">
    <v-icon small @click="dialog = true"> mdi-delete </v-icon>
    <v-dialog v-model="dialog" max-width="550">
      <v-card>
        <v-layout justify-center>

        <v-card-title class="headline" style="color: danger">
          {{ $t("agriculturalMachinery.messages.confirmDeleteMessage") }}
        </v-card-title>
        </v-layout>
        <v-layout justify-center>
          <v-card-actions>
            <v-spacer></v-spacer>

            <v-btn color="primary" text @click="dialog = false">
              {{ $t("buttons.cancel") }}
            </v-btn>
            <v-btn
              color="error"
              text
              @click="(dialog = false), DeleteMachine(itemId)"
            >
              {{ $t("buttons.confirm") }}
            </v-btn>
          </v-card-actions>
        </v-layout>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts">
import httpService from "@/services/http.service";
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({
  components: {},
})
export default class DeleteAgriculturalMachinery extends Vue {
  @Prop({}) itemId!: any;

  private dialog: boolean;
  constructor() {
    super();
    this.dialog = false;
  }

  async DeleteMachine(id: number) {
    try {
      await httpService.delete(
        `/api/AgriculturalMachinery/DeleteMachineById/${id}`
      );

      this.$emit("saved-delete");
    } catch (err) {
      console.log(err);
    }
  }
}
</script>