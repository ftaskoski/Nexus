<template>
  <div>
    <h2 class="text-lg font-semibold mb-4">Search</h2>
    <div class="space-y-4">
      <Input
        v-model="searchQuery"
        placeholder="Search for friends..."
        class="w-full"
        icon="search"
        icon-color="gray"
        icon-fill="none"
        id="search"
        @input="searchForFriends"
      />
    </div>
 
    <div class="space-y-2 mt-4">
      <div 
        v-for="result in searchResults" 
        :key="result.id"
        class="flex items-center justify-between p-3 rounded-lg hover:bg-gray-50 border border-gray-200 transition-colors"
      >
        <div class="flex items-center gap-3">
          <div class="w-8 h-8 bg-blue-100 rounded-full flex items-center justify-center">
            <Icon icon="user" size="20" class=" text-blue-600" />
          </div>
          <span class="font-medium">{{ result.username }}</span>
        </div>
        <button 
         class="p-2 rounded-full hover:bg-gray-100 transition-colors group"
         title="Add friend"
       >
         <Icon icon="add-user" class="w-5 h-5 text-gray-500 group-hover:text-blue-600" />
       </button>
      </div>
    </div>
 
  </div>
 </template>

<script setup lang="ts">
import { ref } from "vue";
import Input from "@/components/Input.vue";
import { fetchy } from "@/plugins/axios";
import type { UserSearchResult } from "./types";
import Icon from "@/components/Icon.vue";

const searchQuery = ref<string>("");
let debounce: number | undefined = undefined;
const searchResults = ref<UserSearchResult[]>([]);

async function searchForFriends()  {
  searchResults.value = [];

  clearTimeout(debounce);
  debounce = setTimeout(async () => {
   const res = await fetchy<UserSearchResult[]>({
      url: `user/${searchQuery.value}`,
      method: "GET",
    });

    searchResults.value = res.payload || [];

  }, 500);


}


</script>
