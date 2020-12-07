<template>
  <div class="distraintEnforce">
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
                      :name="$t('distraints.list.enforcementDate')"
                      rules="required"
                    >
                      <v-text-field
                        :label="$t('distraints.list.enforcementDate')"
                        prepend-icon="mdi-calendar"
                        readonly
                        v-bind="attrs"
                        v-on="on"
                        v-model="formattedEnforcementDate"
                        :error-messages="errors"
                      ></v-text-field>
                    </ValidationProvider>
                  </template>
                  <v-date-picker
                    v-model="localItem.enforcementDate"
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
        <v-card-actions>
          <v-btn color="primary" @click.stop="enforce()">
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
import { DistraintEnforceModel } from "@/models/distraint";
import { mapGetters } from "vuex";
import moment from "moment";
import distraintService from "@/services/distraint.service";
import { namespace } from "vuex-class";
const userNS = namespace("user");

@Component({
  components: {
    ValidationProvider,
    ValidationObserver,
  },
  computed: {
    ...mapGetters({
      dateFormat: "dateFormat",
      language: "language",
    }),
  },
})
export default class DistraintEnforce extends Vue {
  @userNS.Getter("getUserId") userId: any;
  @Prop({ default: false }) show: any;
  @Prop({ required: true }) item: any;
  @Prop({ required: true }) today!: Date;
  private dateMenu = false;
  dateFormat: any;
  language: any;
  @Ref("validator")
  private validator: any;

  constructor() {
    super();
  }

  get doShow() {
    return this.show;
  }

  get localItem() {
    return this.item;
  }

  get formattedEnforcementDate() {
    return this.formatDate(this.localItem.enforcementDate);
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

  close() {
    this.$emit("close-enforce");
  }

  async validateInput() {
    const formValid = await this.validator.validate();
    return formValid;
  }

  async enforce() {
    const isValid = await this.validateInput();
    if (!isValid) return;
    try {
      const model = new DistraintEnforceModel({
        id: this.localItem.id,
        enforcementDate: this.localItem.enforcementDate,
        enforcedBy: this.userId,
      });
      const result = await distraintService.enforce(model);
      console.log("enforce result: " + result);
      this.$emit("saved-enforce");
    } catch (error) {
      console.log("Enforce distraint error ..." + error);
    }
  }
}
</script>