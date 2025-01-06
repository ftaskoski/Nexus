<template>
  <div>
    <h2 class="text-lg font-semibold">Search</h2>
    <div class="space-y-1">
      <Input
        v-model="searchQuery"
        placeholder="Search for friends..."
        class="w-full"
        icon="search"
        icon-color="gray"
        icon-fill="none"
        id="search"
      />
    </div>
    <div v-if="searchQuery" class="mt-4 text-sm text-gray-500">
      Searching for: {{ searchQuery }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import Input from "@/components/Input.vue";

const searchQuery = ref("");
</script>
