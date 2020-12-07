import { RouteConfig } from 'vue-router';
import helper from '../helpers/helper';

const routes: Array<RouteConfig> = [
    {
        path: '/ePayments/list',
        name: 'EPaymentsList',
        component: () => import('../views/ePayments/list.vue')
    },
    {
        path: '/ePayments/settings',
        name: 'EPaymentsSettings',
        component: () => import('../views/ePayments/editEServicesSettings.vue')
    },
    
]

export default routes;