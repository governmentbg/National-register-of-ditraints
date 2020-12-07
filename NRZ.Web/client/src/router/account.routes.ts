import { RouteConfig } from 'vue-router';
import helper from "@/helpers/helper";

const routes: Array<RouteConfig> = [
    {
        path: '/account/emailconfirm',
        name: 'emailConfirm',
        component: () => import('../views/account/emailConfirm.vue')
    },
    {
        path: '/account/profile',
        name: 'userProfile',
        component: () => import('../views/account/userProfile.vue')
    },
    {
        path: '/account/profile/:id',
        name: 'userProfile',
        component: () => import('../views/account/userProfile.vue'),
        props: helper.castIdRouteParams
    }
]

export default routes;