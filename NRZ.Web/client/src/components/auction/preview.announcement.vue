<template>
  <v-row>
    <v-col md="12"
      ><div class="text-h5">{{ $t('auctions.announcements.list.headers.auctionAssets') }}</div>
    </v-col>
    <v-col md="12">
      <properties-list
        :propertyType="propertyType.Other"
        :items="value.items"
        :selectable="false"
      ></properties-list>
    </v-col>
    <v-col md="12"
      ><div class="text-h5">{{ $t('auctions.announcements.list.headers.auctionInfo') }}</div>
    </v-col>
    <v-col md="12">
      <v-row>
        <v-col md="4">
          <v-text-field
            class="required"
            :disabled="true"
            :label="$t('auctions.announcements.list.headers.code')"
            v-model="value.code"
          ></v-text-field>
        </v-col>
        <v-col md="4">
          <v-text-field
            class="required"
            :disabled="true"
            :label="$t('auctions.announcements.list.headers.title')"
            v-model="value.title"
          ></v-text-field>
        </v-col>
        <v-col md="4">
            <v-text-field
            class="required"
            :disabled="true"
            :label="$t('auctions.announcements.list.headers.actionOrder')"
            v-model="value.orderName"
          ></v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col md="4">
            <v-text-field
            class="required"
            :disabled="true"
            :label="$t('auctions.announcements.list.headers.propertyType')"
            v-model="value.propertyTypeName"
          ></v-text-field>
        </v-col>
        <v-col md="4">
          <v-text-field
            class="required"
            :disabled="true"
            :label="$t('auctions.announcements.list.headers.startPrice')"
            v-model="value.startPrice"
            type="number"
          ></v-text-field>
        </v-col>
        <v-col md="4">
          <v-text-field
            class="required"
            :disabled="true"
            :label="$t('auctions.announcements.list.headers.step')"
            v-model="value.bidStep"
            type="number"
          ></v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col md="6">
          <div class="v-label required">{{$t('auctions.announcements.list.headers.registerPeriod')}}</div>
          <v-row>
            <v-col lg="6" md="6">
              <DatetimePicker
                v-model="value.registerStartDate"
                :disabled="true"
                :label="$t('auctions.announcements.list.headers.from')"
                :dateFormat="'dd.MM.yyyy'"
                :timeFormat="'HH:mm'"
                :timePickerProps="{ format: '24hr' }"
              ></DatetimePicker>
            </v-col>
            <v-col lg="6" md="6">
              <DatetimePicker
                v-model="value.registerEndDate"
                :disabled="true"
                :label="$t('auctions.announcements.list.headers.to')"
                :dateFormat="'dd.MM.yyyy'"
                :timeFormat="'HH:mm'"
                :timePickerProps="{ format: '24hr' }"
              ></DatetimePicker>
            </v-col>
          </v-row>
        </v-col>
        <v-col md="6">
          <div class="v-label required">{{ $t('auctions.announcements.list.headers.auctionPeriod') }}</div>
          <v-row>
            <v-col lg="6" md="6">
              <DatetimePicker
                v-model="value.auctionStartDate"
                :disabled="true"
                :label="$t('auctions.announcements.list.headers.from')"
                :dateFormat="'dd.MM.yyyy'"
                :timeFormat="'HH:mm'"
                :timePickerProps="{ format: '24hr' }"
              ></DatetimePicker>
            </v-col>
            <v-col lg="6" md="6">
              <DatetimePicker
                v-model="value.auctionEndDate"
                :disabled="true"
                :label="$t('auctions.announcements.list.headers.to')"
                :dateFormat="'dd.MM.yyyy'"
                :timeFormat="'HH:mm'"
                :timePickerProps="{ format: '24hr' }"
              ></DatetimePicker>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
      <v-row>
        <v-col md="12">
          <div class="v-label pb-3">{{ $t('auctions.announcements.list.headers.description') }}</div>
          <editor
            :disabled="true"
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
        <v-col md="12">
          <div class="v-label pb-3">{{  $t('auctions.announcements.list.headers.attachedFiles') }}</div>
          <div v-for="(file, index) in value.attachments" :key="'attachment' + index">
            <a :href="fileUrl(file.id)">{{file.file.name}}</a>
          </div>
        </v-col>
      </v-row>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { AuctionAnnouncement } from "@/models/autction.models";
import { Component, Vue, Prop } from "vue-property-decorator";
import { PropertyType } from "@/models/enums";
import PropertiesList from "@/components/properties/propertiesShortDisplayList.vue";
import DatetimePicker from "vuetify-datetime-picker/src/components/DatetimePicker.vue";
import Editor from "@tinymce/tinymce-vue";

@Component({
  components: {
    PropertiesList,
    DatetimePicker,
    Editor
  },
  computed: {
    propertyType: () => PropertyType,
  },
})
export default class PreviewAnnouncement extends Vue {
  @Prop({ required: true })
  value!: AuctionAnnouncement;

  fileUrl(id: number|null) {
    if (!id) {
      return '#';
    }

    return this.$store.getters.baseUrl + "/download/" + id;
  }

  mounted() {
    console.log(this.value.attachments[0].file);
    
  }
}
</script>

<style>
</style>