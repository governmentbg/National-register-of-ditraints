import { MainState } from '@/interfaces/states';
import Vue from 'vue'
import Vuex from 'vuex'
import userModule from './modules/user.module';

Vue.use(Vuex)

const isDevelopment = process.env.NODE_ENV == "development";
console.log("environment: ", process.env.NODE_ENV);

let baseUrl = '';
if (!isDevelopment) {
    let href = location.href.split('#')[0];
    if (href[href.length - 1] === '/') {
        href = href.slice(0, href.length - 1);
    }
    baseUrl = href;
} else {
  baseUrl = 'https://localhost:44361';
}

const state: MainState= {
  baseUrl,
  language: localStorage.getItem('culture') || 'bg',
  sideMenu: false,
  dateTimeFormat: 'DD.MM.YYYY HH:mm',
  dateTimeLongFormat: 'DD.MM.YYYY HH:mm:ss',
  dateFormat: 'DD.MM.YYYY',
  timeFormat: 'HH:mm',
  user: null,
  eAuthUrl: 'https://eauthn.egov.bg:9445/eAuthenticator/eAuthenticator.seam'
}

export default new Vuex.Store({
  state,
  getters: {
    language: (state) => state.language,
    baseUrl: (state) => state.baseUrl,
    sideMenu: (state) => state.sideMenu,
    dateTimeFormat: (state) => state.dateTimeFormat,
    dateTimeLongFormat: (state) => state.dateTimeLongFormat,
    dateFormat: (state) => state.dateFormat,
    timeFormat: (state) => state.timeFormat,
    userId: (state) => state.user ? state.user.userId : '',
    eAuthUrl: (state) => state.eAuthUrl
  },
  mutations: {
    toggleSideMenu: (state: MainState) => {
      state.sideMenu = !state.sideMenu;
    },
    setSideMenu: (state: MainState, payload: boolean) => {
      state.sideMenu = payload;
    },
    setLanguage: (state: MainState, payload: string) => {
      state.language = payload;
    }
  },
  actions: {
    toggleSideMenu: ({ commit }) => {
      commit('toggleSideMenu');
    },
    setSideMenu: ({ commit }, payload: boolean) => {
      commit('setSideMenu', payload);
    },
    setLanguage({commit}, payload: string) {
      localStorage.setItem('culture', payload);
      commit('setLanguage', payload);
    }
  },
  modules: {
    user: userModule
  }
})
