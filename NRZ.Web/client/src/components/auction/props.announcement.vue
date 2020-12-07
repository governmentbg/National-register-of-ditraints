<template>
  <v-card>
    <ValidationObserver ref="validator">
      <v-row>
        <v-col md="4">
          <ValidationProvider
            v-slot="{ errors }"
            :name="$t('auctions.announcements.list.headers.code')"
            rules="required"
          >
            <v-text-field
              class="required"
              :label="$t('auctions.announcements.list.headers.code')"
              :error-messages="errors"
              v-model="value.code"
            ></v-text-field>
          </ValidationProvider>
        </v-col>
        <v-col md="4">
          <ValidationProvider
            v-slot="{ errors }"
            :name="$t('auctions.announcements.list.headers.title')"
            rules="required"
          >
            <v-text-field
              class="required"
              :label="$t('auctions.announcements.list.headers.title')"
              :error-messages="errors"
              v-model="value.title"
            ></v-text-field>
          </ValidationProvider>
        </v-col>
        <v-col md="4">
          <ValidationProvider
            v-slot="{ errors }"
            :name="$t('auctions.announcements.list.headers.actionOrder')"
            rules="required"
          >
            <v-select
              class="required"
              :label="$t('auctions.announcements.list.headers.actionOrder')"
              :error-messages="errors"
              v-model="value.order"
              :items="orderItems"
              item-value="code"
              item-text="name"
              @change="onOrderChange"
            >
            </v-select>
          </ValidationProvider>
        </v-col>
      </v-row>
      <v-row>
        <v-col md="4">
          <ValidationProvider
            v-slot="{ errors }"
            :name="$t('auctions.announcements.list.headers.propertyType')"
            rules="required"
          >
            <v-select
              class="required"
              :label="$t('auctions.announcements.list.headers.propertyType')"
              :error-messages="errors"
              v-model="value.propertyType"
              :items="propertyTypes"
              item-value="code"
              item-text="name"
              @change="onPropertyTypeChange"
            >
            </v-select>
          </ValidationProvider>
        </v-col>
        <v-col md="4">
          <ValidationProvider
            v-slot="{ errors }"
            :name="$t('auctions.announcements.list.headers.startPrice')"
            rules="required"
          >
            <v-text-field
              class="required"
              :label="$t('auctions.announcements.list.headers.startPrice')"
              :error-messages="errors"
              v-model="value.startPrice"
              type="number"
            ></v-text-field>
          </ValidationProvider>
        </v-col>
        <v-col md="4">
          <ValidationProvider
            v-slot="{ errors }"
            :name="$t('auctions.announcements.list.headers.step')"
            rules="required"
          >
            <v-text-field
              class="required"
              :label="$t('auctions.announcements.list.headers.code')"
              :error-messages="errors"
              v-model="value.bidStep"
              type="number"
            ></v-text-field>
          </ValidationProvider>
        </v-col>
      </v-row>
      <v-row>
        <v-col md="6">
          <div class="v-label required">{{$t('auctions.announcements.list.headers.registerPeriod')}}</div>
          <v-row>
            <v-col lg="6" md="6">
              <ValidationProvider
                v-slot="{ errors }"
                :name="$t('auctions.announcements.list.headers.registerStartDate')"
                rules="required"
              >
                <DatetimePicker
                  v-model="value.registerStartDate"
                  :label="$t('auctions.announcements.list.headers.from')"
                  :dateFormat="'dd.MM.yyyy'"
                  :timeFormat="'HH:mm'"
                  :timePickerProps="{ format: '24hr' }"
                ></DatetimePicker>
                <div class="text-danger">{{ errors.join(",") }}</div>
              </ValidationProvider>
            </v-col>
            <v-col lg="6" md="6">
              <ValidationProvider
                v-slot="{ errors }"
                :name="$t('auctions.announcements.list.headers.registerEndDate')"
                rules="required"
              >
                <DatetimePicker
                  v-model="value.registerEndDate"
                  :label="$t('auctions.announcements.list.headers.to')"
                  :dateFormat="'dd.MM.yyyy'"
                  :timeFormat="'HH:mm'"
                  :timePickerProps="{ format: '24hr' }"
                  @input="onRegisterStartChange"
                ></DatetimePicker>
                <div class="text-danger">{{ errors.join(",") }}</div>
              </ValidationProvider>
            </v-col>
          </v-row>
        </v-col>
        <v-col md="6">
          <div class="v-label required">{{ $t('auctions.announcements.list.headers.auctionPeriod') }}</div>
          <v-row>
            <v-col lg="6" md="6">
              <ValidationProvider
                v-slot="{ errors }"
                :name="$t('auctions.announcements.list.headers.auctionExecutionStartDate')"
                rules="required"
              >
                <DatetimePicker
                  v-model="value.auctionStartDate"
                  :label="$t('auctions.announcements.list.headers.from')"
                  :dateFormat="'dd.MM.yyyy'"
                  :timeFormat="'HH:mm'"
                  :timePickerProps="{ format: '24hr' }"
                  @input="onAuctionStartChange"
                  
                ></DatetimePicker>
                <div class="text-danger">{{ errors.join(",") }}</div>
              </ValidationProvider>
            </v-col>
            <v-col lg="6" md="6">
              <ValidationProvider
                v-slot="{ errors }"
                :name="$t('auctions.announcements.list.headers.auctionExecutionEndDate')"
                rules="required"
              >
                <DatetimePicker
                  v-model="value.auctionEndDate"
                  :label="$t('auctions.announcements.list.headers.to')"
                  :dateFormat="'dd.MM.yyyy'"
                  :timeFormat="'HH:mm'"
                  :timePickerProps="{ format: '24hr' }"
                ></DatetimePicker>
                <div class="text-danger">{{ errors.join(",") }}</div>
              </ValidationProvider>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
      <v-row>
        <v-col md="12">
          <div class="v-label pb-3">{{ $t('auctions.announcements.list.headers.description') }}</div>
          <editor
            api-key="2ap5dccp95hpdv0izfqz5gniixbw769t58f1dldq3o3asabl"
            v-model="value.description"
            :init="{
              height: 300,
              menubar: false,
              plugins: [
                'advlist autolink lists link image charmap print preview anchor',
                'searchreplace visualblocks code fullscreen',
                'insertdatetime media table paste code help wordcount',
              ],
              toolbar:
                'undo redo | formatselect | bold italic backcolor | \
              alignleft aligncenter alignright alignjustify | \
              bullist numlist outdent indent | removeformat | help',
            }"
          />
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <div class="v-label pb-3">{{ $t('auctions.announcements.list.headers.attachedFiles') }}</div>
          <v-file-input
            multiple
            show-size
            counter
            small-chips
            @change="onFileInput"
          ></v-file-input>
        </v-col>
      </v-row>
    </ValidationObserver>
  </v-card>
</template>

<script lang="ts">
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { Component, Vue, Watch, Ref, Prop } from "vue-property-decorator";
import DatetimePicker from "vuetify-datetime-picker/src/components/DatetimePicker.vue";
import Editor from "@tinymce/tinymce-vue";
import { AuctionAnnouncement } from "@/models/autction.models";
import nomenclatureService from "@/services/nomenclature.service";
import FileModel from "@/models/file";
import { Nomenclature } from "@/models/nomenclature";

@Component({
  components: {
    DatetimePicker,
    Editor,
    ValidationProvider,
    ValidationObserver,
  },
})
export default class AnnouncementProps extends Vue {
  @Prop({ required: true })
  private value!: AuctionAnnouncement;
  @Ref("validator")
  private validator!: InstanceType<typeof ValidationObserver>;
  propertyTypes: Nomenclature[] = [];
  orderItems: Nomenclature[] = [];
  minAucStartDate = new Date();
  minRegisterDate = new Date()

  async fetchOrderTypeItems() {
    try {
      const result = await nomenclatureService.getAuctionOrderTypes();
      this.orderItems = result;
    } catch (error) {
      console.log("Error on getting order type items: " + error);
    }
  }

  async fetchPropertyTypes() {
    try {
      const result = await nomenclatureService.getPropertyTypes();
      this.propertyTypes = result as any;
    } catch (error) {
      console.log("Error on getting property types: " + error);
    }
  }

  public async validate() {
    const valid = await this.validator.validate();
    return valid;
  }

  onFileInput(files: any[]) {
    this.value.attachments.splice(0, this.value.attachments.length);
    files.forEach((f) => {
      const item = new FileModel({ file: f });
      this.value.attachments.push(item);
    });
  }

  @Watch("value.registerStartDate")
  private onRegisterStartChange(newVal: Date) {
    // seconds * minutes * hours * milliseconds = 1 day
    const day = 60 * 60 * 24 * 1000;
    const month = day * 30;
    const tmpDate = new Date(newVal);
    const endDate = new Date(tmpDate.setMonth(tmpDate.getMonth() + 1));
    this.value.registerEndDate = endDate;
  }

  @Watch("value.auctionStartDate")
  private onAuctionStartChange() {
    // seconds * minutes * hours * milliseconds = 1 day
    const day = 60 * 60 * 24 * 1000;

    const endDate = new Date(this.value.auctionStartDate!.getTime() + day * 7);
    this.value.auctionEndDate = endDate;
  }

  onOrderChange() {
    const item = this.orderItems.find((x) => x.code == this.value.order);

    if (item) {
      this.value.orderName = item.name;
    }
  }

  onPropertyTypeChange() {
    const item = this.propertyTypes.find(
      (x) => x.code == this.value.propertyType
    );

    if (item) {
      this.value.propertyTypeName = item.name;
    }
  }

  mounted() {
    this.fetchPropertyTypes();
    this.fetchOrderTypeItems();
  }
}
</script>

<style>
</style>