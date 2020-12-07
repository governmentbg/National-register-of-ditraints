<template>
    <div>
        <div class="row">
            <div class="col-md-4 col-12">
                <v-select
                    v-model="authority"
                    :items="autorities"
                    item-text="name"
                    item-value="code"
                    label="Доставчик на услугата"
                ></v-select>
            </div>
            <div class="col-md-4 col-12">
                <v-btn outlined color="success" @click="getTimeStamp"
                    >Get timestamp</v-btn
                >
            </div>
        </div>
        <div v-if="showResult">
            <v-card-text>
                <v-alert
                    :type="success ? 'success' : 'error'"
                    outlined
                    text
                    class="overflow-auto"
                >
                    <pre>{{ result }}</pre>
                </v-alert>
            </v-card-text>

            <v-btn outlined color="success" @click="getTimeStamp"
                >Get timestamp</v-btn
            >
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import autocomplete from "@/components/dropdowns/autocomplete.vue";
import httpService from "@/services/http.service";
import nomenclatureService from "@/services/nomenclature.service";
import { Nomenclature } from "@/models/nomenclature";

@Component({
    components: { autocomplete },
})
export default class TimestampTest extends Vue {
    private result = "";
    private success = false;
    private showResult = false;
    private autorities: Nomenclature[] = [];
    private authority = '';

     async loadAuthorityItems() {
        try {
            const result = await nomenclatureService.getTimeStampAuthorities();
            this.autorities = result
        } catch (error) {
            // Igrore
        }
    }

    async getTimeStamp() {
        this.showResult = false;

        try {
            const response = await httpService.get(
                `/api/integration/TestTimestamp`, undefined, undefined, { params: { authority: this.authority }}
            );
            this.result = response.data;
            this.success = true;
        } catch (error) {
            this.result = JSON.stringify(error);
            this.success = false;
        }

        this.showResult = true;
    }

    mounted() {
        this.loadAuthorityItems();
    }
}
</script>

