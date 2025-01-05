import { createRouter, createWebHistory } from 'vue-router'
import { loginRoutes } from '@/views/Login/routes'
import { registerRoutes } from '@/views/Register/routes'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    loginRoutes,
    registerRoutes
  ],
})

export default router
