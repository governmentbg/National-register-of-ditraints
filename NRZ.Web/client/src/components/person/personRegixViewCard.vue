<template>
  <div class="RegixPersonView">
    <div>
      <v-row>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.firstName"
            :label="$t('regixPerson.firstName')"
            readonly
          ></v-text-field>
        </v-col>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.middleName"
            :label="$t('regixPerson.middleName')"
            readonly
          ></v-text-field>
        </v-col>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.lastName"
            :label="$t('regixPerson.lastName')"
            readonly
          ></v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="localItem.identifier"
            :label="$t('regixPerson.identifier')"
            readonly
          ></v-text-field>
        </v-col>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="formattedDateOfBirth"
            :label="$t('regixPerson.dateOfBirth')"
            readonly
          ></v-text-field>
        </v-col>
        <v-col cols="6" sm="3">
          <v-text-field
            v-model="formattedDateOfDeath"
            :label="$t('regixPerson.dateOfDeath')"
            readonly
          ></v-text-field>
        </v-col>
      </v-row>
    </div>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop } from "vue-property-decorator";
import { RegixPersonModel } from "@/models/person";
import { mapGetters } from "vuex";
import moment from "moment";

@Component({
  components: {
  },
  computed: {
    ...mapGetters({
      dateFormat: "dateFormat",
      dateTimeFormat: "dateTimeFormat",
      language: "language",
    }),
  },
})
export default class RegixPersonViewCard extends Vue {
  @Prop({ required: true }) item: any;
  dateFormat: any;
  dateTimeFormat: any;
  language: any;

  constructor() {
    super();
  }

  get localItem() {
    return this.item;
  }

  get formattedDateOfBirth() {
    if (this.item.dateOfBirth) return this.formatDate(this.item.dateOfBirth);
    return "";
  }

  get formattedDateOfDeath() {
    if (this.item.dateOfDeath) return this.formatDate(this.item.dateOfDeath);
    return "";
  }

  formatDate(value: Date) {
    if (value) return moment(value.toLocaleString()).format(this.dateFormat);
    else return "";
  }

}
</script>