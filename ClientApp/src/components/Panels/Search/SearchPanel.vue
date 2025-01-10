<template>
  <div>
    <h2 class="text-lg font-semibold mb-4">Search</h2>

    <div class="space-y-1 relative">
      <Input
        v-model="searchQuery"
        placeholder="Search for friends..."
        class="w-full pr-10"
        icon="search"
        icon-color="gray"
        icon-fill="none"
        id="search"
        @input="searchForFriends"
      />
      <div
        @click="clearSearch"
        class="absolute inset-y-0 right-9 -top-1 pr-3 flex items-center cursor-pointer text-gray-400 hover:text-gray-500"
      >
        <Icon icon="cancel" strokeWidth="2" />
      </div>
    </div>

    <div class="space-y-2 mt-4">
      <NavigationRow v-for="result in searchResults" :key="result.id">
        <div class="flex items-center gap-3">
          <div
            class="w-8 h-8 bg-blue-100 rounded-full flex items-center justify-center"
          >
            <Icon icon="user" size="20" class="text-blue-600" />
          </div>
          <span class="font-medium">{{ result.username }}</span>
        </div>
        <div
          class="p-2 rounded-full hover:bg-gray-100 transition-colors group cursor-pointer"
          :title="getIconTitle(result)"
          @click="handleFriendAction(result)"
        >
          <Icon
            :icon="getIconType(result)"
            class="text-gray-500 group-hover:text-blue-600"
          />
        </div>
      </NavigationRow>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import Input from "@/components/Input.vue";
import { fetchy } from "@/plugins/axios";
import type { UserSearchResult } from "./types";
import Icon from "@/components/Icon.vue";
import NavigationRow from "@/components/NavigationRow.vue";

const searchQuery = ref<string>("");
let debounce: number | undefined = undefined;
const searchResults = ref<UserSearchResult[]>([]);

function getIconTitle(result: UserSearchResult): string {
  if (result.status === null) return "Add friend";
  if (result.status === 0) {
    return result.isIncoming ? "User sent you a friend request" : "Pending friend request";
  }
  return "";
}

function getIconType(result: UserSearchResult): string {
  if (result.status === null) return "add-user";
  if (result.status === 0) {
    return result.isIncoming ? "incoming-friend-request" : "pending";
  }
  return "add-user"; 
}

function handleFriendAction(result: UserSearchResult) {
  if (result.status === null) {
    sendFriendRequest(result.id);
  }
 
}

async function searchForFriends() {
  searchResults.value = [];

  clearTimeout(debounce);
  debounce = setTimeout(async () => {
    const res = await fetchy<UserSearchResult[]>({
      url: `friendrequests/${searchQuery.value}`,
      method: "GET",
    });

    searchResults.value = res.payload || [];
  }, 500);
}

async function sendFriendRequest(receiverId: string) {
  const res = await fetchy({
    url: "friendrequests/add-friend",
    method: "POST",
    data: { receiverId },
  });
  const user = searchResults.value.find((u) => u.id === receiverId);
  if (user) {
    user.status = 0;
  }
}

function clearSearch() {
  searchQuery.value = "";
  searchResults.value = [];
}
</script>