<script setup lang="ts">
import Icon from '@/components/Icon.vue'

interface Props {
  grow?:    boolean
  loading?: boolean
}

const props = withDefaults( defineProps<Props>(), {
  grow:    false,
  loading: false
})
</script>

<template>
  <div class="min-h-screen bg-gray-50 flex items-center justify-center p-4 sm:p-6 lg:p-8">
    <div
      class="w-full bg-white rounded-2xl shadow-lg p-8 transition-all duration-300"
      :class="{
        'max-w-7xl': props.grow,
        'max-w-md': !props.grow,
        'opacity-75': loading,
      }"
    >
      <div v-if="$slots.header">
        <slot name="header" />
      </div>

      <div class="relative min-h-[200px]">
        <transition name="fade">
          <div v-if="loading" class="absolute inset-0 flex flex-col justify-center items-center space-y-3">
            <div class="flex items-center space-x-2">
              <Icon
                icon="loading"
                size="24px"
                class="animate-spin text-blue-500"
                fill="currentColor"
                color="none"
              />
              <span class="font-medium text-gray-700 text-lg animate-pulse">Loading content...</span>
            </div>
          </div>
          <slot v-else name="content" />
        </transition>
      </div>

      <div v-if="$slots.footer">
        <slot name="footer" />
      </div>
    </div>
  </div>
</template>
