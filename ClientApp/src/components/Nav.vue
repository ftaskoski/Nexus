<template>
  <div
    class="fixed left-0 top-0 h-screen border-r border-gray-200 bg-white transition-all duration-300 ease-in-out"
    :class="[isSearchActive ? 'w-20' : 'w-64']"
  >
    <div class="mb-6 px-3 py-4 hover:cursor-pointer" @click="toHome">
      <span class="logo" v-if="!isSearchActive">Nexus</span>
      <span class="slim-logo" v-else>N</span>
    </div>

    <div
      v-if="isSearchActive"
      class="fixed left-20 top-0 h-screen w-[400px] bg-white p-4 shadow-lg transition-all duration-300 ease-in-out"
    >
      <div class="mb-4">
        <div class="space-y-1">
          <Input
            v-model="searchQuery"
            placeholder="Search for friends..."
            class="w-full "
            icon="search"
            icon-color="gray"
            icon-fill="none"
            id="search"
          />
        </div>
        
      </div>
      <div class="space-y-2">
        <div v-if="searchQuery" class="text-sm text-gray-500">
          Searching for: {{ searchQuery }}
        </div>
      </div>
    </div>

    <nav class="space-y-1">
      <router-link
        v-for="item in menuItems"
        :key="item.id"
        :to="item.to || '/'"
        class="group flex items-center rounded-lg px-3 py-3 text-sm font-medium transition-all duration-200 ease-in-out"
        :class="[
          activeItem === item.id
            ? 'bg-gray-100 text-black'
            : 'text-gray-600 hover:bg-gray-50',
          isSearchActive ? 'justify-center' : 'w-full'
        ]"
        @click="handleItemClick(item)"
      >
        <svg
          class="h-6 w-6 transition-transform duration-200 ease-in-out"
          :class="[
            hoveredItem === item.id || activeItem === item.id ? 'scale-110' : 'scale-100',
            !isSearchActive ? 'mr-4' : ''
          ]"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          stroke-linecap="round"
          stroke-linejoin="round"
          v-html="item.icon"
        ></svg>
        <span
          v-if="!isSearchActive"
          class="transform transition-all duration-200 ease-in-out"
          :class="[
            hoveredItem === item.id || activeItem === item.id
              ? 'translate-x-1 font-semibold'
              : 'translate-x-0'
          ]"
        >
          {{ item.label }}
        </span>
      </router-link>
    </nav>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import Input from './Input.vue'
import type { NavItem } from './types'
import { useRouter } from 'vue-router'

const router = useRouter()
const activeItem = ref<string>('home')
const hoveredItem = ref<string | null>(null)
const isSearchActive = ref<boolean>(false)
const searchQuery = ref<string>('')


const menuItems : NavItem[] = [
    { 
      id: 'home',
      icon: '<path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path><polyline points="9 22 9 12 15 12 15 22"></polyline>',
      label: 'Home',
      to: '/'
    },
    {
      id: 'search',
      icon: '<circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line>',
      label: 'Search',
      to: '/search'
    },
    {
      id: 'explore',
      icon: '<circle cx="12" cy="12" r="10"></circle><polygon points="16.24 7.76 14.12 14.12 7.76 16.24 9.88 9.88 16.24 7.76"></polygon>',
      label: 'Explore'
    },
    {
      id: 'messages',
      icon: '<path d="M21 11.5a8.38 8.38 0 0 1-.9 3.8 8.5 8.5 0 0 1-7.6 4.7 8.38 8.38 0 0 1-3.8-.9L3 21l1.9-5.7a8.38 8.38 0 0 1-.9-3.8 8.5 8.5 0 0 1 4.7-7.6 8.38 8.38 0 0 1 3.8-.9h.5a8.48 8.48 0 0 1 8 8v.5z"></path>',
      label: 'Messages',
      to: '/messages'
    },
    {
      id: 'notifications',
      icon: '<path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"></path>',
      label: 'Notifications'
    },
    {
      id: 'create',
      icon: '<rect x="3" y="3" width="18" height="18" rx="2" ry="2"></rect><line x1="12" y1="8" x2="12" y2="16"></line><line x1="8" y1="12" x2="16" y2="12"></line>',
      label: 'Create'
    },
    {
      id: 'profile',
      icon: '<path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path><circle cx="12" cy="7" r="4"></circle>',
      label: 'Profile'
    },
    {
      id: 'more',
      icon: '<line x1="3" y1="12" x2="21" y2="12"></line><line x1="3" y1="6" x2="21" y2="6"></line><line x1="3" y1="18" x2="21" y2="18"></line>',
      label: 'More'
    }
  ]

function toHome() {
    activeItem.value = 'home'
    isSearchActive.value = false
    router.push('/')
  }

function handleItemClick(item: NavItem | { id: string }) {
  if (item.id === 'search') {
    isSearchActive.value = true;
    if (isSearchActive.value) {
      activeItem.value = 'search';
    }
  } else {
    isSearchActive.value = false;
    activeItem.value = item.id;
  }
}



</script>

<style>
.logo {
  color: #333;
  text-transform: uppercase;
  letter-spacing: 2px;
  background: linear-gradient(45deg, #866bff, #77dcf5);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  font-family: 'Montserrat', sans-serif;
  font-weight: 700;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.1);
  font-size: 1.5rem;
  line-height: 2rem;
}

.slim-logo {
  color: #333;
  text-transform: uppercase;
  background: linear-gradient(45deg, #866bff, #77dcf5);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  font-family: 'Montserrat', sans-serif;
  font-weight: 800;
  font-size: 1.8rem;
  display: flex;
  justify-content: center;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.1);
}
</style>