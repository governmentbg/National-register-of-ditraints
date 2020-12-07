/*
* Configure application multilanguage support
* including validation messages for vee-validate
*/

import Vue from 'vue';
import VueI18n from 'vue-i18n';
import bg from './bg';
import en from './en';
import enValidationMessages from 'vee-validate/dist/locale/en.json';
import bgValidationMessages from 'vee-validate/dist/locale/bg.json';
import { configure } from 'vee-validate';

Vue.use(VueI18n);

//add validation messages to cultures object
(en as any).validations = enValidationMessages;
(bg as any).validations = bgValidationMessages;

const messages = {
	bg,
	en
}

const i18n = new VueI18n({
	locale: localStorage.getItem('culture') || 'bg',
	fallbackLocale: 'bg',
	messages
})

//Configure validation i18n
configure({
	// this will be used to generate messages.
	defaultMessage: (field: string, values: Record<string, any>) => {
		// values._field_ = i18n.t(`fields.${field}`);
		return i18n.t(`validations.messages.${values._rule_}`, values).toString();
	}
});

export default i18n;