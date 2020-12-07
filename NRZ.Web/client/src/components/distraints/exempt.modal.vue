<template>
  <div class="distraintExempt">
    <v-dialog v-model="doShow" persistent max-width="500">
      <v-card>
        <div>
          <ValidationObserver ref="validator">
            <v-row>
              <v-col cols="12" sm="6">
                <v-menu
                  v-model="dateMenu"
                  :close-on-content-click="false"
                  :nudge-right="40"
                  transition="scale-transition"
                  offset-y
                  min-width="290px"
                >
                  <template v-slot:activator="{ on, attrs }">
                    <ValidationProvider
                      v-slot="{ errors }"
                      :name="$t('distraints.list.exemptionDate')"
                      rules="required"
                    >
                      <v-text-field
                        :label="$t('distraints.list.exemptionDate')"
                        prepend-icon="mdi-calendar"
                        readonly
                        v-bind="attrs"
                        v-on="on"
                        v-model="formattedExemptionDate"
                        :error-messages="errors"
                      ></v-text-field>
                    </ValidationProvider>
                  </template>
                  <v-date-picker
                    v-model="localItem.exemptionDate"
                    @input="dateMenu = false"
                    :first-day-of-week="1"
                    :locale="language"
                    :max="maxDate"
                  ></v-date-picker>
                </v-menu>
              </v-col>
            </v-row>
          </ValidationObserver>
        </div>
        <div>
          <error-message
            :show="showError"
            :messages="errorMessages"
          ></error-message>
        </div>
        <v-card-actions>
          <v-btn color="primary" @click.stop="exempt()">
            {{ $t("buttons.save") }}</v-btn
          >
          <v-btn color="primary" @click.stop="close()">{{
            $t("buttons.cancel")
          }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { Component, Vue, Prop, Ref } from "vue-property-decorator";
import { DistraintExemptModel } from "@/models/distraint";
import { mapGetters } from "vuex";
import moment from "moment";
import distraintService from "@/services/distraint.service";
import { namespace } from "vuex-class";
const userNS = namespace("user");
import ErrorMessage from "@/components/common/errorMessage.vue";
import VueI18n, { TranslateResult } from "vue-i18n";

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
    ErrorMessage,
  },
  computed: {
    ...mapGetters({
      dateFormat: "dateFormat",
      language: "language",
    }),
  },
})
export default class DistraintExempt extends Vue {
  @userNS.Getter("getUserId") userId: any;
  @Prop({ default: false }) show: any;
  @Prop({ required: true }) item: any;
  @Prop({ required: true }) today!: Date;
  private dateMenu = false;
  dateFormat: any;
  language: any;
  @Ref("validator")
  private validator: any;
  private showError = false;
  private errorMessages: [VueI18n.TranslateResult] = [""];

  constructor() {
    super();
  }

  get doShow() {
    return this.show;
  }

  get localItem() {
    return this.item;
  }

  get formattedExemptionDate() {
    return this.formatDate(this.localItem.exemptionDate);
  }

  get maxDate() {
    if (this.today)
      return new Date(
        this.today.getFullYear(),
        this.today.getMonth(),
        this.today.getDate() + 1
      )
        .toISOString()
        .slice(0, 10);

    const now = new Date();
    const t = new Date(now.getFullYear(), now.getMonth(), now.getDate() + 1);
    return t.toISOString().slice(0, 10);
  }

  formatDate(value: Date) {
    if (value) return moment(value.toLocaleString()).format(this.dateFormat);
    else return "";
  }

  resetErrorMessages() {
    this.showError = false;
    this.errorMessages = [""];
  }

  setErrorMessage(generalError: TranslateResult, error: any) {
    console.log(error);
    this.showError = true;
    if (generalError) {
      this.errorMessages.push(generalError);
    }
    if (error) {
      this.errorMessages.push(`${error.response.data}`);
    }
  }

  close() {
    this.$emit("close-exempt");
  }

  async validateInput() {
    const formValid = await this.validator.validate();
    return formValid;
  }

  async exempt() {
    const isValid = await this.validateInput();
    if (!isValid) return;
    try {
      this.resetErrorMessages();
      const model = new DistraintExemptModel({
        id: this.localItem.id,
        exemptionDate: this.localItem.exemptionDate,
        exemptedBy: this.userId,
      });
      const result = await distraintService.exempt(model);
      console.log("exempt result: " + result);
      this.$emit("saved-exempt");
    } catch (error) {
      console.log("Exempt distraint error ..." + error);
      this.setErrorMessage(this.$t("errors.errorExemptingDistraint"), null);
    }
  }
}
</script>