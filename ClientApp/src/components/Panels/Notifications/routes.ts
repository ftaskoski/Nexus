import NotificationsPanel from './NotificationsPanel.vue'
export const notificationsRoutes = {
      path: '/notifications',
      name: 'NotificationsPanel',
      component: NotificationsPanel,
      meta: {
        requiresAuth: true
      }
    }