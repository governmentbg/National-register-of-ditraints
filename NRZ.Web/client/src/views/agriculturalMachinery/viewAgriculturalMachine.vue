<template>
  <div class="text-center">
    <v-dialog v-model="showDialog" width="800">
      <template v-slot:activator="{ on }">
        <v-icon v-on="on" small @click="viewItem(itemId)"> mdi-eye </v-icon>
      </template>
      <v-card v-if="loadedData">
        <v-card-title class="justify-center">
          <span style="color: black" class="headline">{{
            $t("agriculturalMachinery.machine")
          }}</span>
        </v-card-title>
        <v-card-text>
          <v-row>
            <v-col :md="4">
              <v-text-field
                v-model="machine.registrationNumber"
                :label="$t('agriculturalMachinery.registrationNumber')"
                :readonly="true"
              >
              </v-text-field>
            </v-col>

            <v-col :md="4">
              <v-text-field
                v-model="machine.frameNumber"
                :label="$t('agriculturalMachinery.frameNumber')"
                :readonly="true"
              >
              </v-text-field>
            </v-col>

            <v-col :md="4">
              <v-text-field
                v-model="machine.type"
                :label="$t('agriculturalMachinery.type')"
                :readonly="true"
              >
              </v-text-field>
            </v-col>
          </v-row>

          <v-card-title class="justify-center">
            <span style="color: black" class="headline">Регистрирана на</span>
          </v-card-title>

          <div id="person" v-if="loadedData && ownedByPerson">
            <v-row>
              <v-col :md="4">
                <v-text-field
                  v-model="machine.owner.firstName"
                  :label="$t('person.firstName')"
                  :readonly="true"
                >
                </v-text-field>
              </v-col>

              <v-col :md="4">
                <v-text-field
                  v-model="machine.owner.middleName"
                  :label="$t('person.middleName')"
                  :readonly="true"
                >
                </v-text-field>
              </v-col>

              <v-col :md="4">
                <v-text-field
                  v-model="machine.owner.lastName"
                  :label="$t('person.lastName')"
                  :readonly="true"
                >
                </v-text-field>
              </v-col>
            </v-row>

            <v-row>
              <v-col :md="4">
                <v-text-field
                  v-model="machine.owner.identificationNumber"
                  :label="$t('person.egnln4')"
                  :readonly="true"
                >
                </v-text-field>
              </v-col>
              <v-col :md="4">
                <v-text-field
                  v-model="machine.owner.phone"
                  :label="$t('person.phone')"
                  :readonly="true"
                >
                </v-text-field>
              </v-col>
              <v-col :md="4">
                <v-text-field
                  v-model="machine.owner.email"
                  :label="$t('person.email')"
                  :readonly="true"
                >
                </v-text-field>
              </v-col>
            </v-row>
          </div>
          <div id="company" v-if="loadedData && !ownedByPerson">
            <v-row>
              <v-col :md="4">
                <v-text-field
                  v-model="machine.company.name"
                  :label="$t('company.name')"
                  :readonly="true"
                >
                </v-text-field>
              </v-col>

              <v-col :md="4">
                <v-text-field
                  v-model="machine.company.eik"
                  :label="$t('company.eik')"
                  :readonly="true"
                >
                </v-text-field>
              </v-col>

              <v-col :md="4">
                <v-text-field
                  v-model="machine.company.companyCaseNumber"
                  :label="$t('company.companyCaseNumber')"
                  :readonly="true"
                >
                </v-text-field>
              </v-col>
            </v-row>
          </div>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary" text @click="showDialog = false">
            {{ $t("buttons.close") }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts">
import httpService from "@/services/http.service";
import { Component, Prop, Vue } from "vue-property-decorator";
import AgriculturalMachineryCreationModel from "@/models/agriculturalMachinery";
import Person from "@/models/person";
import Company from "@/models/company";

@Component
export default class ViewAgriculturalMachine extends Vue {
  @Prop({}) itemId!: any;

  private machine: AgriculturalMachineryCreationModel;
  private showDialog: boolean;
  private loadedData: boolean;
  private ownedByPerson: boolean;

  constructor() {
    super();
    this.machine = new AgriculturalMachineryCreationModel({});
    this.machine.person = new Person({});
    this.machine.company = new Company({});
    this.showDialog = false;
    this.loadedData = false;
    this.ownedByPerson = false;
  }

  async viewItem(itemId: number) {
    try {
      const response = await httpService.get(
        `/api/AgriculturalMachinery/GetMachineById/${itemId}`
      );

      this.machine = response.data;
      this.loadedData = true;

      console.log(this.itemId);

      if (this.machine.company !== null) {
        this.ownedByPerson = false;
      } else if (this.machine.company === null) {
        this.ownedByPerson = true;
      }
    } catch (err) {
      this.showDialog = false;
      console.log(err);
    }
  }
}
</script>