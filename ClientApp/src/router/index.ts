import { createRouter, createWebHistory } from 'vue-router'
import { loginRoutes } from '@/views/Login/routes'
import { registerRoutes } from '@/views/Register/routes'
import { homeRoutes } from '@/views/Home/routes'
import { isAuthenticated, Status } from '../../authStore/store'
import { panelRoutes } from '@/components/Panels/routes'
import { exploreRoutes } from '@/views/Explore/routes'
import { chatRoutes } from '@/views/Chat/routes'
import { aiRoutes } from '@/views/HuggingFace/routes'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    loginRoutes,
    registerRoutes,
    homeRoutes,
    exploreRoutes,
    panelRoutes,
    chatRoutes,
    aiRoutes
  ],
})

await Status();
router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth && !isAuthenticated.value) {
    next("/login");
  } else if ((to.path === '/login' || to.path === '/register') && isAuthenticated.value) {
    next("/");
  } else {
    next();
  }
});

export default router
