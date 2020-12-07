<template>
  <div>
    <v-row :align="'baseline'" :justify="'center'">
      <v-col>
        <v-card>
          <v-card-title>{{ $t("userProfile.personalData") }}</v-card-title>
          <v-card-text>
            <v-row :align="'baseline'" :justify="'center'">
              <message :type="alertType"
                       :messages="alertMessage"
                       :show="alertShow"
                       :duration="7000"
                       @change="alertShow = !alertShow"></message>
            </v-row>
            <div v-if="loading">
              <v-row :align="'center'" :justify="'center'">
                <v-progress-circular :indeterminate="true" :color="'blue-grey'" :size="100"></v-progress-circular>
              </v-row>
            </div>
            <div v-if="!loading">

              <ValidationObserver ref="validator">
                <v-row>
                  <div class="col-md-4 col-6" v-if="isAdmin && users">
                    <autocomplete
                        :items="users"
                        v-model="userId"
                        :required="true"
                        :label="$t('user.user')">
                    </autocomplete>
                  </div>
                  <div class="col-md-4 col-6">
                    <v-text-field :disabled="true" v-model="userName" :label="$t('user.user')"></v-text-field>
                  </div>
                </v-row>
                <person :show-address="true" :person="person" v-if="person"></person>
                  <v-card-actions>
                    <v-btn color="primary" @click.stop="onSubmit" :disabled="alertShow">
                      {{ $t("buttons.save") }}
                    </v-btn>
                  </v-card-actions>
              </ValidationObserver>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import {Component, Vue, Ref, Prop, Watch} from "vue-property-decorator";
import httpService from '../../services/http.service';
import Person from '../person/person.vue'
import PersonModel from '../../models/person'
import AddressModel from '../../models/address'
import {ValidationProvider, ValidationObserver} from "vee-validate";
import UserModel from "@/models/user";
import Message from "@/components/common/message.vue";
import Autocomplete from "@/components/dropdowns/autocomplete.vue";
import {TranslateResult} from 'vue-i18n';


@Component({
  components: {
    Autocomplete,
    Message,
    Person,
    ValidationProvider,
    ValidationObserver
  }
})
export default class PersonalData extends Vue {
  @Prop({default: null}) person!: PersonModel | null;
  @Prop({default: null}) users!: Array<any>;
  @Prop() userId!: string;
  @Prop() isAdmin!: boolean;
  @Ref() private validator: any;

  user!: UserModel;
  loading!: boolean;
  isValid!: boolean;
  alertShow!: boolean;
  alertMessage!: string | TranslateResult;
  alertType!: string;
  userName!: string;

  constructor() {
    super();
    this.user = new UserModel();
    this.loading = false;
    this.isValid = false;
    this.alertShow = false;
    this.alertType = '';
    this.alertMessage = '';
    this.userName = '';
  }

  @Watch("userId", {immediate:true})
  onUserIdChange(newVal: any) {
    if (newVal) {
      //this.userId = newVal;
      this.getUserData(this.userId);
      this.getCurrentUser(this.userId);
    }
  }

  private getUserData(id: string): PersonModel | null {
    this.loading = true;
    this.person = null;
    httpService.get(`/api/person/getPersonByUserId/${id}`).then(response => {
      if (response.data) {
        this.person = new PersonModel(response.data)
        this.loading = false;
      }
    }).then(() => {
      if (this.person == null) {
        this.person = new PersonModel();
        this.person.address = new AddressModel();
        this.person.userId = this.userId;
      } else if (this.person.address == null) {
        this.person.address = new AddressModel();
      }
      this.loading = false;
    }).catch(error => {
      this.loading = false;
      if (error?.response?.data){
        this.showError(error.response.data)
      }
      else{
        this.showError(error)
      }
    })

    return this.person;
  }

  private getCurrentUser(currentUserId: string) {
    this.loading = true;
    httpService.get(`api/user/getUserRolesById/${currentUserId}`).then(response => {
      if (response && response.data) {
        this.user = new UserModel(response.data);
        this.userName = this.user.userName + ' - ' + this.user.email;
        this.loading = false;
      }
    }).catch(error => {
      this.loading = false;
      if (error?.response?.data){
        this.showError(error.response.data)
      }
      else{
        this.showError(error)
      }
    })
  }

  async onSubmit() {
    this.isValid = await this.validator.validate();
    if (this.isValid) {
      if (this.person && this.person.id) {
        this.edit();
      } else {
        this.create();
      }
    }
  }

  create() {
    httpService.post(`/api/person/create`, this.person).then(response => {
      if (response) {
        this.showSuccess(this.$t('success.successfullyCreatedEntry'))
      }
    }).catch(error => {
      if (error?.response?.data){
        this.showError(error.response.data)
      }
      else{
        this.showError(error)
      }
    })
  }

  edit() {
    httpService.put(`/api/person/`, this.person).then(response => {
      if (response) {
        this.showSuccess(this.$t('success.successfullyEditedEntry'))
      }
    }).catch(error => {
      if (error?.response?.data){
        this.showError(error.response.data)
      }
      else{
        this.showError(error)
      }
    })
  }

  showError(error: string) {
    this.alertMessage = this.$t('errors.getDataError') + error;
    this.alertType = 'error'
    this.alertShow = true;
  }

  showSuccess(message: TranslateResult) {
    this.alertMessage = message;
    this.alertType = 'success'
    this.alertShow = true;
  }
}
</script>

<style>

</style>