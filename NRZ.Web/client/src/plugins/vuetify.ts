import Vue from 'vue';
import Vuetify from 'vuetify/lib';
import colors from 'vuetify/lib/util/colors'
import bg from '@/language/vuetify.bg';

Vue.use(Vuetify);

export default new Vuetify({
    lang: {
        locales: { bg },
        current: 'bg',
    },
    icons: {
        iconfont: 'md', // 'mdiSvg' || 'mdiSvg' || 'md' || 'fa' || 'fa4' || 'faSvg'
    },
    theme: {
        themes: {
            light: {
                primary: colors.blueGrey.base,
                secondary: colors.deepOrange.base,
                accent: colors.lightGreen.base,
                error: colors.red.base,
                warning: colors.orange.base,
                info: colors.lightBlue.base,
                success: colors.green.base
            }
        },
    }
});
