import { createRouter, createWebHistory } from 'vue-router'
import { loginRoutes } from '@/views/Login/routes'
import { registerRoutes } from '@/views/Register/routes'
import { homeRoutes } from '@/views/Home/routes'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    loginRoutes,
    registerRoutes,
    homeRoutes,
  ],
})

export default router
