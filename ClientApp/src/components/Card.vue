<template>
  <div class="min-h-screen bg-gray-50 flex items-center justify-center p-4 sm:p-6 lg:p-8">
    <div class="w-full bg-white rounded-2xl shadow-lg p-8"
    :class="{ 'max-w-7xl': props.grow, 'max-w-md': !props.grow }">
      <div v-if="$slots.header">
        <slot name="header"></slot>
      </div>
      <div>
        <slot name="content"></slot>
      </div>
      <div v-if="$slots.footer">
        <slot name="footer"></slot>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Props {
  grow?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  grow: false
})

</script>