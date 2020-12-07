<template>
  <v-card>
    <v-card-title class="justify-center">
      <span style="color: black" class="headline">{{
        $t("agriculturalMachinery.editMachine")
      }}</span>
    </v-card-title>
    <v-card-text>
      <v-row>
        <v-text-field
          v-model="machine.registrationNumber"
          :label="$t('agriculturalMachinery.registrationNumber')"
        >
        </v-text-field>
      </v-row>
      <v-row>
        <v-text-field
          v-model="machine.frameNumber"
          :label="$t('agriculturalMachinery.frameNumber')"
        >
        </v-text-field>
      </v-row>
      <v-row>
        <v-text-field
          v-model="machine.type"
          :label="$t('agriculturalMachinery.type')"
        >
        </v-text-field>
      </v-row>
    </v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn color="primary" text @click.stop="closeEditDialog()">
        {{ $t("buttons.cancel") }}
      </v-btn>
      <v-btn color="primary" text @click.stop="editItem(machine, itemId)">
        {{ $t("buttons.save") }}
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
import httpService from "@/services/http.service";
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import AgriculturalMachineryCreationModel from "@/models/agriculturalMachinery";

@Component({
  components: {},
})
export default class EditAgriculturalMachinery extends Vue {
  @Prop({}) itemId!: any;

  private machine: AgriculturalMachineryCreationModel;
  private ownedByPerson: boolean;

  constructor() {
    super();
    this.ownedByPerson = false;
    this.machine = new AgriculturalMachineryCreationModel({});
    this.getItem(this.itemId);
  }

    @Watch("itemId")
  onItemIdChange(itemId: number) {
    this.getItem(itemId);
  }

  async getItem(itemId: number) {
    try {
      const response = await httpService.get(
        `/api/AgriculturalMachinery/GetMachineById/${itemId}`
      );

      this.machine = response.data;
    } catch (err) {
      console.log(err);
    }
  }

  closeEditDialog(){
    this.$emit("close-edit");
  }

  async editItem(machine: any, itemId: number) {
    try {
      const machineDetails = {
        id: itemId,
        registrationNumber: this.machine.registrationNumber,
        frameNumber: this.machine.frameNumber,
        type: this.machine.type,
      };

      await httpService.put(
        `/api/AgriculturalMachinery/EditMachine`,
        machineDetails
      );
      this.$emit("saved-edit");
    } catch (err) {
      console.log(err);
    }
  }
}
</script>
