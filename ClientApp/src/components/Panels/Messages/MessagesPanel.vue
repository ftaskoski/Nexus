<template>
  <div>
    <div class="mb-4">
      <h2 class="text-lg font-semibold">Messages</h2>
    </div>
    <div class="space-y-4">
      <div
        v-for="friend in friends"
        :key="friend.id"
        class="flex items-center gap-3 p-3 rounded-lg hover:bg-gray-50 cursor-pointer transition-colors"
        @click="toChat(friend.id)"
      >
        <div class="relative">
          <div
            class="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center"
          >
            <Icon icon="user" size="28" class="text-blue-600" />
          </div>
          <div
            v-if="friend.isOnline"
            class="absolute bottom-0 right-0 w-3 h-3 bg-green-500 rounded-full border-2 border-white"
          ></div>
        </div>
        <div class="flex-1 truncate">
          <div class="flex items-center justify-between">
            <span class="font-medium">{{ friend.username }}</span>
          </div>
          <p class="text-sm text-gray-600 truncate">
            {{ friend.lastMessage }}
          </p>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import Icon from "@/components/Icon.vue";
import {  HubConnectionState } from "@microsoft/signalr";
import { getChatId } from "./store";
import { onMounted, ref, inject } from "vue";
import { useRouter } from 'vue-router'

import type { Friend } from "./types";
import { getFriendsData } from "./store";

let friends = ref<Friend[]>([]);
const router  = useRouter()

const signalR= inject('signalR1') as signalR.HubConnection;

async function getFriends(){
  const res = await getFriendsData();
  friends.value = res.payload.friends;
}

signalR.on("UserStatusChanged", getFriends);


onMounted(async () => {

  await getFriends();

  if (signalR.state === HubConnectionState.Disconnected) {
      await signalR.start();
    }
});

async function toChat(id: string) {
  const res = await getChatId(id);
  router.push(`/chat/${res.payload.chatRoomId}`)
  localStorage.setItem('friendId',id)
}


</script>
