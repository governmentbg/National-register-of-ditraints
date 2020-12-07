<template>
  <v-row>
    <v-col md="12">
      <v-card>
        <v-card-title>
          <h3 class="w-100 text-center">
            {{ $t('auctions.registrations.register.title') }}
          </h3>
        </v-card-title>
        <v-card-text>
          <v-form>
            <v-row>
              <v-col md="12" v-if="value.uniqueNumber">
                <h4>{{ $t('auctions.registrations.register.uniqueNumber') }}</h4>
                <div class="w-100">{{ value.uniqueNumber }}</div>
              </v-col>
              <v-col md="12">
                <h4>{{ $t('auctions.registrations.register.participant') }}</h4>
              </v-col>
              <v-col md="4">
                <v-checkbox
                  :disabled="true"
                  v-model="value.appliedByCourtEnforcer"
                  :label="
                    $t('auctions.registrations.register.applicationSubmission')
                  "
                ></v-checkbox>
              </v-col>
              <v-col md="12">
                <person
                  :disabled="true"
                  :showAddress="true"
                  :person="value.participant"
                ></person>
              </v-col>
            </v-row>
            <v-row>
              <v-col md="12">
                <h4 class="w-100">{{ $t('auctions.registrations.register.participantType') }}</h4>
              </v-col>
              <v-col md="4">
                <v-text-field
                  :disabled="true"
                  class="required"
                  v-model="value.representationTypeName"
                  :label="$t('auctions.registrations.register.as')"
                ></v-text-field>
              </v-col>
              <v-col
                md="12"
                v-if="
                  value.representationType &&
                  value.representationType !== 'PERSONAL'
                "
              >
                <h5 class="w-100">{{ $t('auctions.registrations.register.representedPerson') }}</h5>
                <v-col md="12">
                  <v-radio-group :disabled="true" v-model="representedUserType">
                    <v-radio
                      :disabled="true"
                      :label="$t('personType.person')"
                      :value="'person'"
                    ></v-radio>
                    <v-radio
                      :disabled="true"
                      :label="$t('personType.company')"
                      :value="'company'"
                    ></v-radio>
                  </v-radio-group>
                </v-col>
                <company
                  :disabled="true"
                  :showAddress="false"
                  v-if="representedUserType == 'company'"
                  :company="value.representedCompany"
                ></company>
                <person
                  :disabled="true"
                  :person="value.representedPerson"
                  :showAddress="false"
                  v-if="representedUserType == 'person'"
                ></person>
              </v-col>
            </v-row>
            <v-row>
              <v-col md="12">
                <h4>{{ $t('auctions.registrations.register.additionalInfo') }}</h4>
              </v-col>
              <v-col md="4">
                <v-text-field
                  class="required"
                  v-model="value.resultDeliveryTypeName"
                  :label="$t('auctions.registrations.register.receiveResultWay')"
                  :disabled="true"
                ></v-text-field>
              </v-col>
              <v-col md="4">
                <v-checkbox
                  :disabled="true"
                  v-model="value.isOwner"
                  :label="$t('auctions.registrations.register.propertyOwner')"
                ></v-checkbox>
              </v-col>
              <v-col md="4">
                <v-checkbox
                  :disabled="true"
                  v-model="value.isOwnerSpouse"
                  :label="
                    $t('auctions.registrations.register.isOwnerSpouse')
                  "
                ></v-checkbox>
              </v-col>
              <v-col>
                <h4>{{ $t('auctions.registrations.register.attachedFiles') }}</h4>
                <div
                  v-for="(file, index) in value.attachments"
                  :key="'attachment' + index"
                >
                  <a :href="fileUrl(file.id)">{{ file.file.name }}</a>
                </div>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Vue, Component, Prop } from "vue-property-decorator";
import person from "@/components/person/person.vue";
import company from "@/components/company/company.vue";
import Person from "@/models/person";
import Company from "@/models/company";
import { Registration } from "@/models/autction.models";
import FileModel from "@/models/file";
import http from "@/services/http.service";
import personService from "@/services/person.service";

@Component({
  components: {
    person,
    company,
  },
})
export default class AuctionRegister extends Vue {
  @Prop({ required: true })
  private value!: Registration;
  get representedUserType() {
    return this.value.representedPerson ? "person" : "company";
  }

  fileUrl(id: number | null) {
    if (!id) {
      return "#";
    }

    return this.$store.getters.baseUrl + "/download/" + id;
  }
}
</script>

<style lang="scss">
.v-label {
  margin-bottom: 0px !important;
}
</style>