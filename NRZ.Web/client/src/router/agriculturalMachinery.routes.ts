import { RouteConfig } from 'vue-router';


const routes: Array<RouteConfig> = [
    {
       path: '/agriculturalMachinery',
       name: 'agriculturalMachinery',
       component: () => import('../views/agriculturalMachinery/agriculturalMachinery.vue')
    },
]
export default routes;