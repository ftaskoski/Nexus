import { createRouter, createWebHistory } from 'vue-router'
import { loginRoutes } from '@/views/Login/routes'
import { registerRoutes } from '@/views/Register/routes'
import { homeRoutes } from '@/views/Home/routes'
import { isAuthenticated, Status } from '../../authStore/store'
import { searchRoutes } from '@/components/Panels/Search/routes'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    loginRoutes,
    registerRoutes,
    homeRoutes,
    // searchRoutes
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
