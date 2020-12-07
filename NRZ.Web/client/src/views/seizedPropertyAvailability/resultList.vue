<template>
    <div>
        <v-data-table :loading="isLoading"
                      :headers="headers"
                      :items="items"
                      :items-per-page="10">
        </v-data-table>
    </div>
</template>

<script>
import { Component, Vue, Prop } from "vue-property-decorator";

@Component()
export default class ResultList extends Vue {
    //@Prop({ required: true }) items;

    isLoading = false;
    items = [];
    headers = [];

    constructor() {
        super();

        this.headers = [
/*            { text: this.$t('seizedPropertyAvailabilityRequest.id'), align: 'start', sortable: false, value: "id" },
            { text: this.$t('seizedPropertyAvailabilityRequest.name'), align: 'start', sortable: false, value: "name" },*/
        ];

        this.items = [{

        }];
    }

    mounted() {
        console.log();
    }
}
</script>
