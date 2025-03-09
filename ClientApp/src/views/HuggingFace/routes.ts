import HuggingFace from "./HuggingFace.vue"
export const aiRoutes = {
      path: '/huggingface',
      name: 'HuggingFace',
      component: HuggingFace,
      meta: {
        requiresAuth: true
      }
    }