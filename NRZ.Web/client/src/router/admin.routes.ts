import { RouteConfig } from 'vue-router';

const routes: Array<RouteConfig> = [
    {
        path: '/admin/users',
        name: 'users',
        component: () => import('../views/admin/users.vue')
    },
    {
        path: '/admin/usersRoles',
        name: 'usersRoles',
        component: () => import('../views/admin/usersRoles.vue')
    },
    {
        path: '/admin/logs',
        name: 'systemLogs',
        component: () => import('../views/admin/systemLogs.vue')
    },
    {
        path: '/admin/systemSettings',
        name: 'systemSettings',
        component: () => import('../views/admin/systemSettings.vue')
    },
    {
        path: '/admin/timestamp',
        name: 'timestamp',
        component: () => import('../views/admin/timestamp.vue')
    }
]

export default routes;