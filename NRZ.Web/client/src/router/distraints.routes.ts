import { RouteConfig } from 'vue-router';
import helper from '../helpers/helper';

const routes: Array<RouteConfig> = [
    {
        path: '/distraints/list',
        name: 'DistraintsList',
        component: () => import('../views/distraints/list.vue')
    },
    {
        path: '/distraints/create',
        name: 'CreateDistraint',
        component: () => import('../views/distraints/createDistraint.vue')
    },
    {
        path: '/distraints/view/:id',
        name: 'ViewDistraint',
        component: () => import('../views/distraints/viewDistraint.vue'),
        props: helper.castIdRouteParams
    },
    
]

export default routes;