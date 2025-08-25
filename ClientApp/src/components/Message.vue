<script setup lang="ts">
import { watch } from 'vue'
import Icon from './Icon.vue'

interface Props {
  type?:      'error' | 'warning' | 'success' | 'info'
  show?:      boolean
  timeout?:   number
  autoLeave?: boolean
}

const props = withDefaults( defineProps<Props>(), {
  type:      'error',
  show:      false,
  timeout:   1500,
  autoLeave: true,
})

const emit = defineEmits<{
  ( eventName: 'close', value: void ): void
}>()

const closeMessage = () => emit( 'close' )

watch(
  () => props.show,
  ( newVal ) => {
    if ( newVal && props.autoLeave ) {
      setTimeout(() => {
        closeMessage()
      }, props.timeout )
    }
  }
)
</script>

<template>

  <transition name="message">

    <div
      v-if="show"
      class="w-wull max-w-full h-max px-4 py-2.5 flex items-start space-x-4 rounded"
      :class="{
        'bg-red-500': type === 'error',
        'bg-teal-500': type === 'info',
        'bg-green-500': type === 'success',
        'bg-yellow-500': type === 'warning',

        'text-black': type === 'warning',
        'text-white': type === 'error' || type === 'success' || type === 'info',
      }"
    >

      <Icon :icon="type" stroke-width="2" />

      <div class="flex items-start justify-start w-full h-full">
        <slot>Message</slot>
      </div>

      <Icon icon="cancel" class="cursor-pointer" @click="closeMessage" />

    </div>

  </transition>

</template>

<style scoped>

.message-enter-active,
.message-leave-active {
  transition: opacity 0.5s;
}

.message-enter-from,
.message-leave-to {
  opacity: 0;
}

</style>
