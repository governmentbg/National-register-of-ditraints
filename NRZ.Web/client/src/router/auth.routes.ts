import { RouteConfig } from 'vue-router';

const routes: Array<RouteConfig> = [
    {
        path: '/login',
        name: 'Login',
        component: () => import('../views/auth/login.vue')
    },
    {
        path: '/login/result',
        name: 'ESignLoginResult',
        component: () => import('../views/auth/eSignLoginResult.vue')
    },
    {
        path: '/register',
        name: 'Register',
        component: () => import('../views/auth/register.vue')
    },
    {
        path: '/register/result',
        name: 'ESignLoginResult',
        component: () => import('../views/auth/eSignRegisterResult.vue')
    },
    {
        path: '/register/success',
        name: 'RegisterSuccess',
        component: () => import('../views/auth/registerSuccess.vue')
    },
    {
        path: '/forgottenPassword',
        name: 'ForgottenPassword',
        component: () => import('../views/auth/forgottenPassword.vue')
    },
    {
        path: '/resetPassword',
        name: 'ResetPassword',
        component: () => import('../views/auth/resetPassword.vue')
    },
    {
        path: '/forgottenPassword/success',
        name: 'ForgottenPasswordSuccess',
        component: () => import('../views/auth/forgottenPasswordSuccess.vue')
    },
    {
        path: '/resetPassword/success',
        name: 'ResetPasswordSuccess',
        component: () => import('../views/auth/resetPasswordSuccess.vue')
    },
]

export default routes;