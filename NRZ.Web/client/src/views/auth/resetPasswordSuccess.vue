<template>
  <div>
    <v-alert outlined type="success" text>
      <div class="title">{{ $t('resetPassword.resetPasswordSuccessTitle') }}</div>
      <div>
        {{ $t('resetPassword.resetPasswordSuccessBody') }}
      </div>
      <div>За да се върнете на "Начален екран" натиснете <router-link to="/">тук</router-link> или изчакайте</div>
    </v-alert>
  </div>
</template>

<script>
export default {
  mounted(){
    setTimeout(() => {
      this.$router.push('/')
    }, 10000);
  }
}
</script>


<style>
</style>