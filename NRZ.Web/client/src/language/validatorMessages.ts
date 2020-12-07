import {localize} from "vee-validate";
import bg from 'vee-validate/dist/locale/bg.json';
import en from 'vee-validate/dist/locale/en.json';
import store from "@/store/store";

localize('bg',bg)
localize({
    bg: {
        messages: {
            passwordPattern: 'Паролата трябва да съдържа малка и голяма, латинска или кирилска, буква, число и специален символ',
            compareFields: 'Паролата не съвпада',
            isPropertyChecked: 'Не сте избрали вещ'
        }
    },
    en: {
        messages: {
            passwordPattern: 'The password must contains lower and upper latin or cyrillic letter, number and special symbol',
            compareFields: 'The password doesn\'t match',
            isPropertyChecked: 'You haven\'t picked a property'
        }
    }
});

