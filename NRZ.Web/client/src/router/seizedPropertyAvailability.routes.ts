import { RouteConfig } from 'vue-router';

const routes: Array<RouteConfig> = [
    {
        path: '/seizedPropertyAvailability/request',
        name: 'seizedPropertyAvailabilityRequest',
        component: () => import('../views/seizedPropertyAvailability/seizedPropertyAvailabilityRequest.vue')
    },
    {
        path: '/seizedPropertyAvailability/resultList',
        name: 'seizedPropertyAvailabilityResultList',
        component: () => import('../views/seizedPropertyAvailability/resultList.vue')
    },
    {
        path: '/seizedPropertyAvailability/list',
        name: 'seizedPropertyAvailabilityList',
        component: () => import('../views/seizedPropertyAvailability/list.vue'),
        meta: { auth: true, roles: ["SysAdmin", "MJAdmin"]}
    },
    {
        path: '/seizedPropertyAvailability/paidReport',
        name: 'seizedPropertyAvailabilityPaidReport',
        component: () => import('../views/seizedPropertyAvailability/paidReport.vue'),
        props: route => ({ id: route.query.id })
    }
]

export default routes;
