import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import Home from '../views/Home.vue'
import RequestForCertificateOfDistraintOfProperty from '../views/certificateOfDistraintOfProperty/request.vue';
import RequestsForCertificateOfDistraintOfPropertyList from '../views/certificateOfDistraintOfProperty/requestsList.vue';
import store from '../store/store';

import authRoutes from './auth.routes';
import adminRoutes from './admin.routes';
import distraintRoutes from './distraints.routes';
import accountRoutes from './account.routes';
import agriculturalRoutes from './agriculturalMachinery.routes';
import seizedPropertyAvailability from './seizedPropertyAvailability.routes';
import auctionRoutes from './auction.routes';
import ePaymentRoutes from './ePayments.routes';

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    meta: { auth: false }
  },
  {
    path: '/certificateOfDistraintOfProperty/request',
    name: 'RequestForCertificateOfDistraintOfProperty',
    component: RequestForCertificateOfDistraintOfProperty 
  },
  {
    path: '/certificateOfDistraintOfProperty/list',
    name: 'RequestsForCertificateOfDistraintOfPropertyList',
    component: RequestsForCertificateOfDistraintOfPropertyList,
    meta: { auth: true, roles: ["SysAdmin", "MJAdmin"]}
  },
  ...accountRoutes,
  ...adminRoutes,
  ...authRoutes,
  ...distraintRoutes,
  ...agriculturalRoutes,
  ...seizedPropertyAvailability,
  ...auctionRoutes,
  ...ePaymentRoutes
]

const router = new VueRouter({
  //mode: 'history',
  base: process.env.BASE_URL,
  routes
})


//Check for authentication
router.beforeEach((to, from, next) => {
  const { auth, roles } = to.meta;

  if (auth) {
    //Route requires authentication
    if (!store.getters['user/isAuthenticated']) {
      //User not authenticated - redirect to login
      next(`/login?returnUrl=${to.fullPath}`);
    } else {
      if (roles) {
        //User authenticated - check for permissions
        const hasRole: boolean = store.getters['user/hasRole'](roles);
        if (!hasRole) {
          next('/unauthorized');
        }
      }
      next();
    }
  } else {
    //Route does not require authentication
    next();
  }

  return;
})

export default router
