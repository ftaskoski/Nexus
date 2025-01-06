<template>
    <div
      v-if="isActive"
      class="fixed left-20 top-0 h-screen w-[400px] bg-white p-4 shadow-lg transition-all duration-300 ease-in-out"
    >
    <div v-if="type === 'search'">
      <SearchPanel  />
    </div>
  
      <div v-if="type === 'notifications'">
        <NotificationsPanel />
      </div>
  
      <div v-if="type === 'messages'">
        <MessagesPanel />
      </div>

    </div>
  </template>
  
<script setup lang="ts">

import NotificationsPanel from './Panels/Notifications/NotificationsPanel.vue';  
import MessagesPanel from './Panels/Messages/MessagesPanel.vue';
import SearchPanel from './Panels/Search/SearchPanel.vue';
import type { PanelType } from './types';

const props = defineProps<{
    type: PanelType
    isActive: boolean
  }>()
  
  
</script>