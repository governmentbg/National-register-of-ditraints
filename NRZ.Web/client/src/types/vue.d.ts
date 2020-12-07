import Vue from 'vue'
import moment from 'moment';
import Http from 'axios-typescript-response';


declare module 'vue/types/vue' {
  interface Vue {
    $moment: typeof moment;
    $http: typeof Http;
    _uid: number;
  }
}