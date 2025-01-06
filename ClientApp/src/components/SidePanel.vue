<template>
    <div
      v-if="isActive"
      class="fixed left-20 top-0 h-screen w-[400px] bg-white p-4 shadow-lg transition-all duration-300 ease-in-out"
    >
    <div v-if="type === 'search'">
      <SearchPanel  />
    </div>
  
      <div v-if="type === 'notifications'">
        <div class="mb-4">
          <h2 class="text-lg font-semibold">Notifications</h2>
        </div>
        <div class="space-y-4">
          <div class="rounded-lg border p-4">
            <p class="text-sm text-gray-600">No new notifications</p>
          </div>
        </div>
      </div>
  
      <div v-if="type === 'messages'">
        <div class="mb-4">
          <h2 class="text-lg font-semibold">Messages</h2>
        </div>
        <div class="space-y-4">
          <div class="rounded-lg border p-4">
            <p class="text-sm text-gray-600">No new messages</p>
          </div>
        </div>
      </div>
    </div>
  </template>
  
<script setup lang="ts">
import { ref } from 'vue'
import SearchPanel from './Panels/Search/SearchPanel.vue';
import type { PanelType } from './types';
  
const props = defineProps<{
    type: PanelType
    isActive: boolean
  }>()
  
  const searchQuery = ref('')
  
</script>