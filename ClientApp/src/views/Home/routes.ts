import Home from './Home.vue'

export const homeRoutes = {
  path:      '/',
  name:      'Home',
  component: Home,
  meta:      {
    requiresAuth: true
  }
}
