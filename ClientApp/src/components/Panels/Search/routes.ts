import SearchPanel from './SearchPanel.vue'

export const searchRoutes = {
  path:      '/search',
  name:      'SearchPanel',
  component: SearchPanel,
  meta:      {
    requiresAuth: true
  }
}
