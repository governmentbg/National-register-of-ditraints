import { RouteConfig } from 'vue-router';

const routes: Array<RouteConfig> = [
    {
        path: '/announcements/',
        name: 'announcements',
        component: () => import('../views/auction/announcements/list.vue')
    },
    {
        path: '/announcements/my',
        name: 'myAnouncements',
        component: () => import('../views/auction/announcements/myList.vue')
    },
    {
        path: '/announcements/display/:id',
        name: 'announcementDisplay',
        component: () => import('../views/auction/announcements/display.vue')
    },
    {
        path: '/announcements/details/:id',
        name: 'announcementDisplayRegister',
        component: () => import('../views/auction/announcements/displayRegister.vue')
    },
    {
        path: '/announcements/create',
        name: 'announcementCreate',
        component: () => import('../views/auction/announcements/create.vue')
    },
    {
        path: '/announcements/register/:id',
        name: 'announcementRegister',
        component: () => import('../views/auction/registrations/register.vue')
    },
    {
        path: '/announcements/registrations/:id?',
        name: 'announcementRegistations',
        component: () => import('../views/auction/registrations/list.vue')
    },
    {
        path: '/announcements/registration/display/:id',
        name: 'announcementRegistationDisplay',
        component: () => import('../views/auction/registrations/display.vue')
    },
    {
        path: '/announcements/registration/approve/:id',
        name: 'announcementRegistationApprove',
        component: () => import('../views/auction/registrations/approve.vue')
    },
    {
        path: '/auctions',
        name: 'auctions',
        component: () => import('../views/auction/list.vue')
    },
    {
        path: '/auctions/my',
        name: 'myAuctions',
        component: () => import('../views/auction/myList.vue')
    },
    {
        path: '/auctions/:id',
        name: 'auction',
        component: () => import('../views/auction/auction.vue')
    },
    {
        path: '/auctions/result/:id',
        name: 'auctionResult',
        component: () => import('../views/auction/result.vue')
    }
]

export default routes;