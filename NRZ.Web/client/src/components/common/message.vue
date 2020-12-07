<template>
  <div class="message">
    <v-col class="d-flex" cols="12">
      <v-alert dense :type="type" v-if="doShow" class="multi-line">
        {{ localMessages }}
      </v-alert>
    </v-col>
  </div>
</template>


<script lang="ts">
// @ is an alias to /src
import {Component, Vue, Prop, Watch} from "vue-property-decorator";

@Component({})
export default class ErrorMessage extends Vue {
  @Prop({default: false}) show: any;
  @Prop({required: true}) messages: any;
  @Prop() type: string;
  @Prop() duration: number;

  constructor() {
    super();
    this.type = 'error';
    this.duration = 5000;
  }

  get doShow() {
    return this.show;
  }

  get localMessages() {
    // const cleanedMessages = this.messages.filter((v: string) => v!="");
    // return cleanedMessages.join("\r\n");
    return this.messages;
  }

  @Watch('show')
  onShowChange(newVal: boolean) {
    if (newVal) {
      setTimeout(() => {
        this.show = false;
        this.$emit('change',this.show);
      }, this.duration);
    }
  }
}
</script>

<style>
.multi-line {
  white-space: pre-line;
}
</style>