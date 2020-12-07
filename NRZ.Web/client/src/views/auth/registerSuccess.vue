<template>
  <div>
    <v-alert outlined type="success" text>
      <div class="title">Успешна регистрация</div>
      <div>
        На посочемия от Вас имейл ще получите връзка за потвърждение на Вашия
        профил.
      </div>
    </v-alert>
    <v-alert outlined type="warning" text v-if="showWarning">
      {{ warningMessage }}
    </v-alert>
  </div>
</template>

<script>
export default {
  data() {
    return {
      showWarning: false,
      warningMessage: "",
    };
  },
  mounted() {
    const {
      warning,
      warningMessage,
      number
    } = this.$route.query;

    if (warning) {
      this.showWarning = true;
      if (warningMessage) {
        this.warningMessage = this.$t("notifications." + warningMessage).format(number);
      }
    }
  },
};
</script>

<style>
</style>