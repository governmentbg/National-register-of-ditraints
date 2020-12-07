<template>
  <div class="RegixPerson">
    <div>
      <ValidationObserver ref="validator">
        <v-row>
          <v-col cols="6" sm="3">
            <ValidationProvider
              v-slot="{ errors }"
              :name="$t('regixPerson.firstName')"
              rules="required"
            >
              <v-text-field
                v-model="localItem.firstName"
                :label="$t('regixPerson.firstName')"
                :error-messages="errors"
                class="required"
                :readonly="isReadonly"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
          <v-col cols="6" sm="3">
            <v-text-field
              v-model="localItem.middleName"
              :label="$t('regixPerson.middleName')"
              :readonly="isReadonly"
            ></v-text-field>
          </v-col>
          <v-col cols="6" sm="3">
            <ValidationProvider
              v-slot="{ errors }"
              :name="$t('regixPerson.lastName')"
              rules="required"
            >
              <v-text-field
                v-model="localItem.lastName"
                :label="$t('regixPerson.lastName')"
                :error-messages="errors"
                class="required"
                :readonly="isReadonly"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="6" sm="3">
            <ValidationProvider
              v-slot="{ errors }"
              :name="$t('regixPerson.identifier')"
              rules="required"
            >
              <v-text-field
                v-model="localItem.identifier"
                :label="$t('regixPerson.identifier')"
                :error-messages="errors"
                class="required"
                :readonly="isReadonly"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
          <v-col cols="6" sm="3">
            <v-menu
              v-model="dateMenuBirth"
              :close-on-content-click="false"
              :nudge-right="40"
              transition="scale-transition"
              offset-y
              min-width="290px"
            >
              <template v-slot:activator="{ on, attrs }">
                  <v-text-field
                    :label="$t('regixPerson.dateOfBirth')"
                    prepend-icon="mdi-calendar"
                    readonly
                    v-bind="attrs"
                    v-on="on"
                    v-model="formattedDateOfBirth"
                  ></v-text-field>
              </template>
              <v-date-picker
                v-model="localItem.dateOfBirth"
                @input="dateMenuBirth = false"
                :first-day-of-week="1"
                :locale="language"
              ></v-date-picker>
            </v-menu>
          </v-col>
          <v-col cols="6" sm="3">
            <v-menu
              v-model="dateMenuDeath"
              :close-on-content-click="false"
              :nudge-right="40"
              transition="scale-transition"
              offset-y
              min-width="290px"
            >
              <template v-slot:activator="{ on, attrs }">
                  <v-text-field
                    :label="$t('regixPerson.dateOfDeath')"
                    prepend-icon="mdi-calendar"
                    readonly
                    v-bind="attrs"
                    v-on="on"
                    v-model="formattedDateOfDeath"
                  ></v-text-field>
              </template>
              <v-date-picker
                v-model="localItem.dateOfDeath"
                @input="dateMenuDeath = false"
                :first-day-of-week="1"
                :locale="language"
              ></v-date-picker>
            </v-menu>
          </v-col>
        </v-row>
      </ValidationObserver>
    </div>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { Component, Vue, Prop, Ref } from "vue-property-decorator";
import { RegixPersonModel } from "@/models/person";
import { mapGetters } from "vuex";
import moment from "moment";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
  },
  computed: {
    ...mapGetters({
      dateFormat: "dateFormat",
      dateTimeFormat: "dateTimeFormat",
      language: "language",
    }),
  },
})
export default class RegixPersonCard extends Vue {
  @Prop({ required: true }) item: any;
  @Prop({ required: true }) isReadonlyCard: any;
  private dateMenuBirth = false;
  private dateMenuDeath = false;
  dateFormat: any;
  dateTimeFormat: any;
  language: any;
  @Ref("validator")
  private validator: any;

  constructor() {
    super();
  }

  get localItem() {
    return this.item;
  }

    get isReadonly() {
    return this.isReadonlyCard;
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

  public async validateInput() {
    const formValid = await this.validator.validate();
    return formValid;
  }
}
</script>