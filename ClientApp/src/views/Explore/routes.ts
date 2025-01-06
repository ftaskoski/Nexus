import Explore from './Explore.vue'
export const exploreRoutes = {
      path: '/explore',
      name: 'Explore',
      component: Explore,
      meta: {
        requiresAuth: true
      }
    }