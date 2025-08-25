<script setup lang="ts">
import { RouterView } from 'vue-router'
import { isAuthenticated } from '../authStore/store'
import Nav from './components/Nav.vue'
import NotificationsSocket from './components/Panels/Notifications/NotificationsSocket.vue'

</script>

<template>
  <div class="relative">
    <Nav v-if="isAuthenticated" />
    <NotificationsSocket v-if="isAuthenticated" />
    <main>
      <RouterView />
    </main>
    <div
      id="panel-target"
      class="fixed left-20 top-0 h-screen transition-all duration-300"
      :class="{
        'w-[400px] opacity-100': $route.meta?.panel,
        'w-0 opacity-0': !$route.meta?.panel,
      }"
    />
  </div>
</template>

<style>
::-webkit-scrollbar {
  width: 6px;
}

::-webkit-scrollbar-thumb {
  background: #aaa;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: #888;
}

::-webkit-scrollbar-track {
  background: #f0f0f0;
  border-radius: 4px;
}
</style>
