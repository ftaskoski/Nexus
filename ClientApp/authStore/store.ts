import { ref } from 'vue'
import { fetchy } from '../src/plugins/axios'

export const isAuthenticated = ref<boolean>( false )

export async function Status() {
  const res = await fetchy({
    url:    '/user/lookup',
    method: 'GET',
  })
  isAuthenticated.value = res.payload.isAuthenticated
}

export function successfulAuth( router: any ) {
  isAuthenticated.value = true
  router.push( '/' )
}
