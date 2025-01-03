import { createRouter, createWebHistory } from 'vue-router'
import { homeRoutes } from '@/views/Home/routes'
import { aboutRoutes } from '@/views/About/routes'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    homeRoutes,
    aboutRoutes
  ],
})

export default router
