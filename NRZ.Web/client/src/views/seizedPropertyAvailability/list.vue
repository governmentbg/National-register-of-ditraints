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
import { Component, Vue } from "vue-property-decorator";
import moment from 'moment';

@Component
export default class List extends Vue {
    isLoading = false;
    items = [];
    headers = [];

    constructor() {
        super();
        moment.locale('bg');
    }

    loadData() {
        this.isLoading = true;
        this.headers = [
            { text: this.$t('seizedPropertyAvailabilityRequest.id'), align: 'start', sortable: false, value: "id" },
            { text: this.$t('seizedPropertyAvailabilityRequest.date'), value: 'date', sortable: true },
            { text: this.$t('seizedPropertyAvailabilityRequest.requesterPerson'), value: 'requestorPersonFullName', sortable: true },
            { text: this.$t('seizedPropertyAvailabilityRequest.inTheQualityOf'), value: 'inTheQualityOf', sortable: true },
            { text: this.$t('seizedPropertyAvailabilityRequest.bulstat'), value: 'requestorCompanyEik', sortable: true },
            { text: this.$t('seizedPropertyAvailabilityRequest.checkedPerson'), value: 'checkedPersonFullName' }
        ];

        this.$http.get('/api/SeizedPropertyAvailabilityRequest').then(result => {
            result.data.forEach(item => {
                item.date = moment(item.date).format('L');
            });
            this.items = result.data;
            this.isLoading = false;
        }).catch(error => {
            console.error(error);
        });
    }

    mounted() {
        this.loadData();
    }
}
</script>
