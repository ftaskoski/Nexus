import MessagesPanel from './MessagesPanel.vue'
export const messagesRoutes = {
      path: '/messages',
      name: 'MessagesPanel',
      component: MessagesPanel,
      meta: {
        requiresAuth: true
      }
    }