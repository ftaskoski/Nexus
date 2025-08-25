import Chat from './Chat.vue'

export const chatRoutes = {
  path:      '/chat/:id',
  name:      'Chat',
  component: Chat,
  meta:      {
    requiresAuth: true
  },
}
