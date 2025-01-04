<script setup lang="ts">
import { computed, defineProps } from 'vue'
import { type IconName, icons } from '@/components/icon'

interface Props {
  icon?: IconName
  size?: string
  color?: string
  strokeWidth?: string
}

const props = withDefaults( defineProps<Props>(), {
  icon  : '',
  size  : '24px',
  color : 'currentColor',
  strokeWidth: '1.5'
})

const icon = computed(() => {
  return icons.find(i => i.name === props.icon) ?? icons[0];
});
</script>

<template>
  <svg
    :color="color"
    :width="size"
    :height="size"
    fill="none"
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 24 24"
    :stroke-width="strokeWidth"
    >
    <path
      v-for="path in icon.path"
      :key="path"
      :d="path"
      :stroke="color"
      :stroke-width="strokeWidth"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
  </svg>
</template>
