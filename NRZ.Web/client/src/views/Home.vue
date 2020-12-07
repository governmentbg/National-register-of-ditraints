<template>
    <v-row>
        <v-col md="4">
            <v-card height="150px">
                <v-card-title class="text-center">
                    {{ $t("RequestForCertificateOfDistraintOfProperty.requestTitle") }}
                </v-card-title>
                <v-card-actions class="card-actions-bottom">
                    <div class="w-100 text-center">
                        <v-btn
                            text
                            color="primary"
                            :to="{ path: `/certificateOfDistraintOfProperty/request` }"
                        >
                            {{ $t('home.newAnnouncement') }}
                        </v-btn>
                    </div>
                </v-card-actions>
            </v-card>
        </v-col>
        <v-col md="4">
            <v-card height="150px">
                <v-card-title class="text-center">
                    <div class="w-100 text-center">{{ $t('home.publicSaleAnnouncements') }}</div>
                </v-card-title>

                <v-card-actions class="card-actions-bottom">
                    <div class="w-100 text-center">
                        <v-btn text color="primary" to="/announcements">
                            {{ $t('home.publicSaleAnnouncements') }}
                        </v-btn>
                    </div>
                </v-card-actions>
            </v-card>
        </v-col>
        <v-col md="4">
            <v-card height="150px">
                <v-card-title class="text-center">
                    <div class="w-100 text-center">{{ $t('home.electronicPublicSale') }}</div>
                </v-card-title>

                <v-card-actions class="card-actions-bottom">
                    <div class="w-100 text-center">
                        <v-btn text color="primary" to="/auctions">
                            {{ $t('home.auctions') }}
                        </v-btn>
                    </div>
                </v-card-actions>
            </v-card>
        </v-col>
        <v-col md="4">
            <v-card height="150px">
                <v-card-title class="text-center">
                    <div class="w-100 text-center">{{ $t("seizedPropertyAvailabilityRequest.title") }}</div>
                </v-card-title>

                <v-card-actions class="card-actions-bottom">
                    <div class="w-100 text-center">
                        <v-btn text color="primary" to="/seizedPropertyAvailability/request">
                            {{ $t("seizedPropertyAvailabilityRequest.newRequest") }}
                        </v-btn>
                    </div>
                </v-card-actions>
            </v-card>
        </v-col>
    </v-row>
</template>

<script>
// @ is an alias to /src
export default {
    name: "Home",
    components: {
    },
    data() {
        return {
            searchText: "",
            loading: false,
            totalItems: 0,
            items: [],
            options: {},
            headers: [
                {
                    text: "Name",
                    align: "start",
                    sortable: false,
                    value: "name",
                },
                // { text: "Calories", value: "calories" },
                // { text: "Fat (g)", value: "fat" },
                // { text: "Carbs (g)", value: "carbs" },
                // { text: "Protein (g)", value: "protein" },
                // { text: "Iron (%)", value: "iron" },
            ],
        };
    },
    methods: {},
    watch: {},
};
</script>

<style scoped lang="scss">
.card-actions-bottom {
    position: absolute;
    left: 0;
    right: 0;
    bottom: 0;
}
</style>
