<template>
  <div class="errorMessage">
    <v-alert dense outlined type="error" v-if="doShow" class="multi-line">
      {{ localMessages }}
    </v-alert>
  </div>
</template>


 <script lang="ts">
// @ is an alias to /src
import { Component, Vue, Prop } from "vue-property-decorator";

@Component({})
export default class ErrorMessage extends Vue {
  @Prop({ default: false }) show: any;
  @Prop({ required: true }) messages: any;

  constructor() {
    super();
  }

  get doShow() {
    return this.show;
  }

  get localMessages() {
    const cleanedMessages = this.messages.filter((v: string) => v!="");
    return cleanedMessages.join("\r\n");
  }
}
</script>

<style>
.multi-line {
  white-space: pre-line;
}
</style>