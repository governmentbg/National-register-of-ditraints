<template>
  <v-row>
    <v-col md="12">
      <v-card>
        <v-card-title>
          <h3 class="w-100 text-center">
            {{ $t("auctions.registrations.register.title") }}
          </h3>
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="onSubmit">
            <ValidationObserver ref="validator">
              <v-row>
                <v-col md="12">
                  <h4>{{ $t("auctions.registrations.register.participant") }}</h4>
                </v-col>
                <v-col md="4">
                  <v-checkbox
                      v-model="model.appliedByCourtEnforcer"
                      :label="
                      $t(
                        'auctions.registrations.register.applicationSubmission'
                      )
                    "
                  ></v-checkbox>
                </v-col>
                <v-col md="12">
                  <person
                      :showAddress="true"
                      :person="model.participant"
                  ></person>
                </v-col>
              </v-row>
              <v-row>
                <v-col md="12">
                  <h4 class="w-100">{{ $t("auctions.registrations.register.participantType") }}</h4>
                </v-col>
                <v-col md="4">
                  <ValidationProvider
                      v-slot="{ errors }"
                      :name="$t('auctions.registrations.register.participantType')"
                      rules="required"
                  >
                    <v-select
                        class="required"
                        v-model="model.representationType"
                        :items="roles"
                        item-text="name"
                        item-value="code"
                        :label="$t('auctions.registrations.register.participantType')"
                        :error-messages="errors"
                    ></v-select>
                  </ValidationProvider>
                </v-col>
                <v-col
                    md="12"
                    v-if="
                    model.representationType &&
                    model.representationType !== 'PERSONAL'
                  "
                >
                  <h5 class="w-100">{{ $t("auctions.registrations.register.representedPerson") }}</h5>
                  <v-col md="12">
                    <v-radio-group v-model="representedUserType">
                      <v-radio
                          :label="$t('personType.person')"
                          :value="'person'"
                      ></v-radio>
                      <v-radio
                          :label="$t('personType.company')"
                          :value="'company'"
                      ></v-radio>
                    </v-radio-group>
                  </v-col>
                  <company
                      :showAddress="false"
                      v-if="representedUserType == 'company'"
                      :company="model.representedCompany"
                  ></company>
                  <person
                      :person="model.representedPerson"
                      :showAddress="false"
                      v-if="representedUserType == 'person'"
                  ></person>
                </v-col>
              </v-row>
              <v-row>
                <v-col md="12">
                  <h4>{{ $t("auctions.registrations.register.additionalInfo") }}</h4>
                </v-col>
                <v-col md="4">
                  <ValidationProvider
                      v-slot="{ errors }"
                      :name="$t('auctions.registrations.register.receiveResultWay')"
                      rules="required"
                  >
                    <v-select
                        class="required"
                        v-model="model.resultDeliveryType"
                        :items="resultOptions"
                        item-text="name"
                        item-value="code"
                        :label="
                        $t('auctions.registrations.register.receiveResultWay')
                      "
                        :error-messages="errors"
                    ></v-select>
                    <v-alert dense outlined type="info">
                      Декларирам, че приемам всички уведомления във връзка с продажбата да ми бъдат изпращани по посочения по-горе начин.
                    </v-alert>
                  </ValidationProvider>
                </v-col>
                <v-col md="4">
                  <v-checkbox
                      v-model="model.isOwner"
                      :label="$t('auctions.registrations.register.propertyOwner')"
                  ></v-checkbox>
                </v-col>
                <v-col md="4">
                  <v-checkbox
                      v-model="model.isOwnerSpouse"
                      :label="
                      $t(
                        'auctions.registrations.register.isOwnerSpouse'
                      )
                    "
                  ></v-checkbox>
                </v-col>
                <v-col>
                  <h4>{{ $t("auctions.registrations.register.attachedFiles") }}</h4>
                  <v-file-input
                      multiple
                      show-size
                      counter
                      small-chips
                      @change="onFileInput"
                  ></v-file-input>
                </v-col>
                <v-col md="12" class="text-center">
                  <v-btn type="submit" color="primary">{{
                      $t("buttons.submit.text")
                    }}
                  </v-btn>
                </v-col>
              </v-row>
            </ValidationObserver>
          </v-form>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import {Vue, Component, Watch, Ref} from "vue-property-decorator";
import person from "@/components/person/person.vue";
import company from "@/components/company/company.vue";
import Person from "@/models/person";
import Company from "@/models/company";
import {Registration} from "@/models/autction.models";
import FileModel from "@/models/file";
import http from "@/services/http.service";
import {ValidationProvider, ValidationObserver} from "vee-validate";
import personService from '@/services/person.service';
import Address from "@/models/address";

@Component({
  components: {
    person,
    company,
    ValidationProvider,
    ValidationObserver,
  },
})
export default class AuctionRegister extends Vue {
  @Ref() private validator!: InstanceType<typeof ValidationObserver>;
  private isSaving = false;
  private role = "";
  private creator: Person = new Person();
  private representedUser!: Person;
  private representedCompany!: Company;
  private representedUserType = "person";
  private model: Registration;
  private isParticipantDisabled = true;

  constructor() {
    super();
    this.model = new Registration();
    this.model.participant = new Person();
    this.model.representedPerson = new Person();
  }

  get roles() {
    return [
      {code: "PERSONAL", name: "Лично"},
      {code: "REPRESENTATIVE", name: "Законен представител"},
      {code: "ASSIGNEE", name: "Пълномощник"},
    ];
  }

  get resultOptions() {
    return [
      {code: "EDELIVERY", name: "е-Връчване"},
      {code: "EMAIL", name: "Ел. поща"},
    ];
  }

  @Watch("representedUserType")
  onRepresentedUserTypeChange(newVal: string) {
    console.log("representation type change: ", newVal);

    if (newVal === "person") {
      this.model.representedCompany = null;
      const p = new Person();
      p.address = new Address();
      this.model.representedPerson = p;
    } else if (this.representedUserType == "company") {
      this.model.representedPerson = null;
      this.model.representedCompany = new Company();
    }
  }

  @Watch("model.appliedByCourtEnforcer")
  onAppliedByCourtEnforcerChange(newVal: string) {
    if (newVal) {
      this.isParticipantDisabled = false;
      this.model.participant = new Person();
      this.model.participant.address = new Address();
    } else {
      this.isParticipantDisabled = true;
      this.getCurrentUserInfo();
    }
  }

  onFileInput(files: File[]) {
    this.model.attachments.splice(0, this.model.attachments.length);
    files.forEach((f: File) => {
      const item = new FileModel({file: f});
      this.model.attachments.push(item);
    });
  }

  async onSubmit() {
    const valid = await this.validator.validate();

    if (valid) {
      this.isSaving = true;
      if (this.model.representationType === "PERSONAL") {
        this.model.representedCompany = null;
        this.model.representedPerson = null;
      } else {
        if (this.representedUserType == "person") {
          this.model.representedCompany = null;
          delete this.model.representedPerson?.address;
        } else {
          this.model.representedPerson = null;
          delete this.model.representedCompany?.address;
        }
      }

      const jsonData = JSON.stringify(this.model);
      console.log(jsonData);

      const formData = new FormData();
      formData.append("data", jsonData);

      //attachments
      if (this.model.attachments.length) {
        for (let index = 0; index < this.model.attachments.length; index++) {
          const attachment = this.model.attachments[index];
          formData.append(`files`, attachment.file);
        }
      }

      http
          .post("/api/AuctionRegister/create", formData)
          .then(() => this.$router.push("/announcements"))
          .catch((err) => console.log(err));
    }

    console.log("Inavalid model");

  }

  getCurrentUserInfo() {
    const userId = this.$store.getters["user/getUserId"];
    personService.getByUserId(userId).then((data: Person) => {
      console.log('Current user\'s person', data);

      this.model.participant = data;
    })
        .catch(error => console.log(error));
  }

  mounted() {
    const {id} = this.$route.params;
    this.model.announcementId = parseInt(id);
    this.getCurrentUserInfo();
  }
}
</script>

<style lang="scss">
.v-label {
  margin-bottom: 0px !important;
}
</style>