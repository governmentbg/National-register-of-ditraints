import Vue from 'vue'

import App from './App.vue'
import './registerServiceWorker'
import router from './router/router'
import store from './store/store'
import i18n  from './language/language'
import http from './services/http.service';
import moment from 'moment';

import '@mdi/font/css/materialdesignicons.css'
import 'material-design-icons-iconfont/dist/material-design-icons.css'

//Plugins
import vuetify from './plugins/vuetify';
import './plugins/veeValidate';
import './plugins/axios';
// import DatetimePicker from 'vuetify-datetime-picker';
// Vue.use(DatetimePicker)

//extensions
import './extensions/string';

//Localization
// Vue.use(VueI18n)

Vue.config.productionTip = false
Vue.prototype.$http = http;
Vue.prototype.$moment = moment;

new Vue({
  router,
  store,
  i18n,
  vuetify,
  render: h => h(App)
}).$mount('#app')
