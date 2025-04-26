<template>
  <div
    class="fixed left-0 top-0 h-screen border-r border-gray-200 bg-white transition-all duration-300 ease-in-out"
    :class="[activePanelType ? 'w-20' : 'w-64']"
  >
    <div class="mb-6 px-3 py-4 hover:cursor-pointer" @click="toHome">
      <span class="logo" v-if="!activePanelType">Nexus</span>
      <span class="slim-logo" v-else>N</span>
    </div>

    <nav class="flex h-[calc(100%-90px)] flex-col space-y-1">
      <router-link
  v-for="item in menuItems.filter((i) => i.id !== 'logout')"
  :key="item.id"
  :to="item.to || '/'"
  class="group relative flex items-center rounded-lg px-3 py-3 text-sm font-medium transition-all duration-200 ease-in-out"
  :class="[isLinkActive(item) ? 'bg-gray-100 text-black' : 'text-gray-600 hover:bg-gray-50', activePanelType ? 'justify-center' : 'w-full']"
  @click="handleItemClick(item)"
>
  <svg
    class="h-6 w-6 transition-transform duration-200 ease-in-out group-hover:scale-110"
    :class="!activePanelType ? 'mr-4' : ''"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    stroke-width="2"
    stroke-linecap="round"
    stroke-linejoin="round"
    v-html="item.icon"
  ></svg>
  
    <span v-if="item.id === 'notifications' && notificationsCount > 0" class="absolute top-0 right-0 ml-4 mr-2 h-4 w-4 rounded-full bg-red-500 text-center text-xs font-bold text-white" > {{ notificationsCount }} </span>

    <span
      v-if="!activePanelType"
      class="transform transition-all duration-200 ease-in-out group-hover:translate-x-1 group-hover:font-semibold"
    >
      {{ item.label }}
    </span>

</router-link>


      <div class="flex-grow"></div>

      <router-link
        v-for="item in menuItems.filter((i) => i.id === 'logout')"
        :key="item.id"
        :to="item.to || '/'"
        class="group flex items-center rounded-lg px-3 py-3 text-sm font-medium transition-all duration-200 ease-in-out"
        :class="[
          isLinkActive(item)
            ? 'bg-gray-100 text-black'
            : 'text-gray-600 hover:bg-gray-50',
          activePanelType ? 'justify-center' : 'w-full',
        ]"
        @click="handleLogout"
      >
        <svg
          class="h-6 w-6 transition-transform duration-200 ease-in-out group-hover:scale-110"
          :class="!activePanelType ? 'mr-4' : ''"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          stroke-linecap="round"
          stroke-linejoin="round"
          v-html="item.icon"
        ></svg>
        <span
          v-if="!activePanelType"
          class="transform transition-all duration-200 ease-in-out group-hover:translate-x-1 group-hover:font-semibold"
        >
          {{ item.label }}
        </span>
      </router-link>
    </nav>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import { useRouter, useRoute } from "vue-router";
import type { NavItem, PanelType } from "./types";
import { fetchy } from "@/plugins/axios";
import { isAuthenticated } from "../../authStore/store";
import { notificationsCount } from "./Panels/Notifications/store";

const router = useRouter();
const route = useRoute();
const activePanelType = ref<PanelType | null>(null);

const menuItems: NavItem[] = [
  {
    id: "home",
    icon: '<path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path><polyline points="9 22 9 12 15 12 15 22"></polyline>',
    label: "Home",
    to: "/",
  },
  {
    id: "search",
    icon: '<circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line>',
    label: "Search",
    to: "/search",
    hasPanel: true,
  },
  {
    id: "messages",
    icon: '<path d="M21 11.5a8.38 8.38 0 0 1-.9 3.8 8.5 8.5 0 0 1-7.6 4.7 8.38 8.38 0 0 1-3.8-.9L3 21l1.9-5.7a8.38 8.38 0 0 1-.9-3.8 8.5 8.5 0 0 1 4.7-7.6 8.38 8.38 0 0 1 3.8-.9h.5a8.48 8.48 0 0 1 8 8v.5z"></path>',
    label: "Messages",
    to: "/messages",
    hasPanel: true,
  },
  {
    id: "notifications",
    icon: '<path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"></path>',
    label: "Notifications",
    to: "/notifications",
    hasPanel: true,
  },
  {
  id: "huggingface",
  icon: '<path d="M13 2L3 14h9l-1 8 10-12h-9l1-8z"></path>',
  label: "Hugging Face AI",
  to: "/huggingface",
},
  {
    id: "logout",
    icon: '<path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4" /><polyline points="16 17 21 12 16 7" /><line x1="21" y1="12" x2="9" y2="12" />',
    label: "Logout",
    to: "/login",
  },
];

async function handleLogout() {
  await fetchy({
    url: "user/logout",
    method: "POST",
  });
  isAuthenticated.value = false;
  router.push("/login");
}

watch(
  () => route.path,
  (newPath) => {
    const panelItems = menuItems.filter((item) => item.hasPanel);
    const activeItem = panelItems.find(
      (item) => item.to && newPath.startsWith(item.to)
    );

    if (activeItem) {
      activePanelType.value = activeItem.id as PanelType;
    } else {
      activePanelType.value = null;
    }
  },
  { immediate: true }
);

function toHome() {
  activePanelType.value = null;
  router.push("/");
}

function handleItemClick(item: NavItem) {
  if (isLinkActive(item)) return;

  if (item.hasPanel) {
    activePanelType.value =
      activePanelType.value === (item.id as PanelType)
        ? null
        : (item.id as PanelType);
  } else {
    activePanelType.value = null;
  }
}

function isLinkActive(item: NavItem): boolean {
  if (!item.to) return false;
  return item.to === "/" ? route.path === "/" : route.path.startsWith(item.to);
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
  font-family: "Montserrat", sans-serif;
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
  font-family: "Montserrat", sans-serif;
  font-weight: 800;
  font-size: 1.8rem;
  display: flex;
  justify-content: center;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.1);
}


</style>
