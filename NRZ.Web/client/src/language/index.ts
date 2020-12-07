import Vue from 'vue';
import VueI18n from 'vue-i18n';
import bg from './bg';
import en from './en';

Vue.use(VueI18n);

const messages = {
	bg,
	en
}

export default new VueI18n({
	locale: localStorage.getItem('culture') || 'bg',
	fallbackLocale: 'bg',
	messages
})