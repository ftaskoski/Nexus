import { createRouter, createWebHistory } from 'vue-router'
import { panelRoutes } from '@/components/Panels/routes'
import { chatRoutes } from '@/views/Chat/routes'
import { homeRoutes } from '@/views/Home/routes'
import { aiRoutes } from '@/views/HuggingFace/routes'
import { loginRoutes } from '@/views/Login/routes'
import { registerRoutes } from '@/views/Register/routes'
import { isAuthenticated, Status } from '../../authStore/store'

const router = createRouter({
  history: createWebHistory( import.meta.env.BASE_URL ),
  routes:  [
    loginRoutes,
    registerRoutes,
    homeRoutes,
    panelRoutes,
    chatRoutes,
    aiRoutes
  ],
})

router.beforeEach( async ( to, from, next ) => {
  await Status()
  if ( to.meta.requiresAuth && !isAuthenticated.value ) {
    next( '/login' )
  }
  else if (( to.path === '/login' || to.path === '/register' ) && isAuthenticated.value ) {
    next( '/' )
  }
  else {
    next()
  }
})

export default router
