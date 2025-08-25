<script setup lang="ts">
import { onMounted, ref } from 'vue'

const isMounted = ref<boolean>( false )

onMounted(() => isMounted.value = true )
</script>

<template>
  <Teleport v-if="isMounted" to="#panel-target">
    <div class="h-full w-full bg-white p-4 shadow-lg">
      <router-view v-slot="{ Component }">
        <transition name="panel" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
    </div>
  </Teleport>
</template>
