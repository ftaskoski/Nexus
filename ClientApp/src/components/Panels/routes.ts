import { messagesRoutes } from './Messages/routes'
import { notificationsRoutes } from './Notifications/routes'
import PanelLayout from './PanelLayout.vue'
import { searchRoutes } from './Search/routes'

export const panelRoutes = {
  path:      '/panel',
  component: PanelLayout,
  meta:      {
    requiresAuth: true,
    panel:        true,
  },
  children: [
    searchRoutes,
    messagesRoutes,
    notificationsRoutes,
  ]
}
