<template>
    <button
      type="submit"
      :disabled="loading || disabled"
      :class="[
      buttonTypeClasses,
      buttonSizeClasses,
      'w-full flex items-center justify-center border border-transparent text-sm font-medium rounded-lg focus:outline-none focus:ring-2 focus:ring-offset-2 transition-colors duration-200',
      { 'cursor-not-allowed': loading || disabled }  
    ]"
  >
      <slot></slot>
    </button>
  </template>
  

<script setup lang="ts">
import { computed } from 'vue'
import type { UtilsSize } from './types'
interface Props {
  size?: UtilsSize
  type?: 'primary' | 'secondary' | 'tertiary' | 'skinny' | 'naked' | 'danger' | 'warning' | 'success' | 'info'
  loading?: boolean
  disabled?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  size: 'l',
  type: 'primary',
  loading: false,
  disabled: false
})

const buttonSizeClasses = computed(() => {
  switch (props.size) {
    case 'xxl':
      return 'px-8 py-4 text-xl'; 
    case 'xl':
      return 'px-6 py-3 text-lg';
    case 'l':
      return 'px-5 py-2 text-base';
    case 'm':
      return 'px-4 py-2 text-sm'; 
    case 's':
      return 'px-3 py-1 text-xs';
    case 'xs':
      return 'px-2 py-1 text-xs';
    default:
      return 'px-4 py-2 text-sm'; 
  }
})

const buttonTypeClasses = computed(() => {
  switch (props.type) {
    case 'primary':
      return 'bg-blue-500 hover:bg-blue-600 focus:ring-blue-500 text-white';
    case 'secondary':
      return 'bg-gray-500 hover:bg-gray-600 focus:ring-gray-500 text-white';
    case 'tertiary':
      return 'bg-transparent hover:bg-gray-100 focus:ring-gray-500 text-gray-800';
    case 'skinny':
      return 'bg-transparent border border-gray-500 hover:bg-gray-100 focus:ring-gray-500 text-gray-500';
    case 'naked':
      return 'bg-transparent hover:bg-transparent focus:ring-transparent text-gray-500';
    case 'danger':
      return 'bg-red-500 hover:bg-red-600 focus:ring-red-500 text-white';
    case 'warning':
      return 'bg-yellow-500 hover:bg-yellow-600 focus:ring-yellow-500 text-white';
    case 'success':
      return 'bg-green-500 hover:bg-green-600 focus:ring-green-500 text-white';
    case 'info':
      return 'bg-teal-500 hover:bg-teal-600 focus:ring-teal-500 text-white';
    default:
      return 'bg-blue-500 hover:bg-blue-600 focus:ring-blue-500 text-white'; 
  }
});
</script>
