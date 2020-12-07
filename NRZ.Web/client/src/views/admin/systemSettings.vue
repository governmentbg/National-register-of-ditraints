<template>
  <v-row justify="center" align="baseline">
    <v-col :md="12">
      <h2 class="text-center">{{ $t("systemSettings.title") }}</h2>
    </v-col>
    <v-col :md="6">
      <v-card>
        <v-card-title class="justify-center">
          {{ $t("systemSettings.connectionWithRegix") }}
        </v-card-title>
        <div class="p-3">
          <div class="from-group">
            <div class="text-center">
              <v-row>
                <v-col>
                  <v-btn @click="testConnectionToRegixVehicle()">
                    RegiX - Vehicles</v-btn
                  >
                </v-col>
                <v-col>
                  <v-btn @click="testConnectionToRegixVessels()">
                    RegiX - Vessels</v-btn
                  ></v-col
                >
                <v-col>
                  <v-btn @click="testConnectionToRegixAircrafts()">
                    RegiX - Aircrafts</v-btn
                  ></v-col
                >
              </v-row>
              <v-card-text v-if="showResult">
                <v-alert :type="success ? 'success' : 'error'" outlined text>
                  {{ msg }} - Request id: {{ requestId }} - Property Identifier:
                  {{ propertyIdentifier }}</v-alert
                >
              </v-card-text>
            </div>
          </div>
        </div>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import httpService from "@/services/http.service";

@Component
export default class SystemSettings extends Vue {
  private msg: string;
  private success = false;
  private showResult = false;
  private propertyIdentifier!: string;
  private requestId!: number;
  constructor() {
    super();
    this.msg = "";
    this.propertyIdentifier = "";
    this.requestId = 0;
  }

  async testConnectionToRegixVehicle() {
    this.showResult = false;
    try {
      const response = await httpService.post(
        `/api/integration/TestConnectionToRegiXVehicle`,
        null
      );
      this.success = true;
      this.msg = "Connected";
      this.propertyIdentifier = response.data.propertyIdentifier;
      this.requestId = response.data.requestId;
    } catch (error) {
      this.success = false;
      this.msg = JSON.stringify(error.response.data);
    }

    this.showResult = true;
  }

  async testConnectionToRegixVessels() {
    this.showResult = false;
    try {
      const response = await httpService.post(
        `/api/integration/TestConnectionToRegiXVessels`,
        null
      );
      this.success = true;
      this.msg = "Connected";
      this.propertyIdentifier = response.data.propertyIdentifier;
      this.requestId = response.data.requestId;
    } catch (error) {
      this.success = false;
      this.msg = JSON.stringify(error.response.data);
    }

    this.showResult = true;
  }

  
  async testConnectionToRegixAircrafts() {
    this.showResult = false;
    try {
      const response = await httpService.post(
        `/api/integration/TestConnectionToRegiXAircrafts`,
        null
      );
      this.success = true;
      this.msg = "Connected";
      this.propertyIdentifier = response.data.propertyIdentifier;
      this.requestId = response.data.requestId;
    } catch (error) {
      this.success = false;
      this.msg = JSON.stringify(error.response.data);
    }

    this.showResult = true;
  }

}
</script>

<style>
h4 {
  text-align: center;
}
.multi-line {
  white-space: pre-line;
}
</style>