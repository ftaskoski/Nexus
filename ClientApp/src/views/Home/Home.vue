<template>
  <div>
    <Card :grow="true">
      <template #header>
        <div class="flex items-center justify-between gap-2 p-4 border-b">
          <h1 class="text-xl font-semibold">Messages</h1>
          <div class="flex items-center gap-4">
            <Input
              placeholder="Search messages..."
              icon="search"
              icon-fill="none"
              v-model="searchQuery"
              class="w-64"
              id="search"          
            />
      
          </div>
        </div>
      </template>

      <template #content>
        <div class="p-4 space-y-4">
          <div class="space-y-2">
            <h2 class="text-sm font-medium text-gray-500">Recent Chats</h2>
            <div class="space-y-2">
              <div v-for="i in 5" :key="i" 
                class="flex items-center gap-3 p-3 rounded-lg hover:bg-gray-50 cursor-pointer transition-colors"
              >
                <div class="w-10 h-10 bg-blue-100 rounded-full flex items-center justify-center">
                  <Icon icon="user" class="w-6 h-6 text-blue-600" />
                </div>
                <div class="flex-1">
                  <div class="flex items-center justify-between">
                    <span class="font-medium">User Name</span>
                    <span class="text-sm text-gray-500">2m ago</span>
                  </div>
                  <p class="text-sm text-gray-600 truncate">
                    Latest message preview goes here...
                  </p>
                </div>
              </div>
            </div>
          </div>

          <div class="mt-6">
            <h2 class="text-sm font-medium text-gray-500 mb-3">Online Friends</h2>
            <div class="flex gap-4 overflow-x-auto pb-2">
              <div v-for="friend in onlineFriends" :key="friend.id" 
                class="flex flex-col items-center space-y-1 min-w-[60px]"
              >
                <div class="relative">
                  <div class="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center">
                    <Icon icon="user" class="w-7 h-7 text-blue-600" />
                  </div>
                  <div class="absolute bottom-0 right-0 w-3 h-3 bg-green-500 rounded-full border-2 border-white"></div>
                </div>
                <span class="text-xs font-medium">{{ friend.username }}</span>
              </div>
            </div>
          </div>
        </div>
      </template>

      <template #footer>
        <div class="border-t p-4">
          <div class="flex items-center justify-end text-sm text-gray-500">
            <span>© 2025 Nexus</span>
          </div>
        </div>
      </template>
    </Card>
  </div>
</template>

<script setup lang="ts">
import Card from '@/components/Card.vue';
import Icon from '@/components/Icon.vue';
import Input from '@/components/Input.vue';
import { ref, onMounted, onUnmounted } from 'vue';
import { HubConnection, HubConnectionState } from "@microsoft/signalr";
import { inject } from 'vue';
import { fetchy } from '@/plugins/axios';
import type { OnlineFriend } from './types';
import { notificationsCount, notifications } from '@/components/Panels/Notifications/store';
import { handleNotification } from '@/components/Panels/Notifications/store';
import { getNotifications } from '@/components/Panels/Notifications/store';



const signalRConnection = inject('signalR1') as signalR.HubConnection;
const signalR = inject("signalR1") as HubConnection;
const signalR2 = inject("signalR2") as HubConnection;

const searchQuery = ref<string>('');
const onlineFriends = ref<OnlineFriend[]>([]);

const fetchOnlineFriends = async () => {
 
    const response = await fetchy({
      url: 'user/friends/online',
      method: 'GET',
    })

    const data = response.payload || [];
    onlineFriends.value = data;
 
};


const handleUserStatusChanged = (userId: string, username: string, isOnline: boolean) => {
  if (isOnline) {
    const exists = onlineFriends.value.some(friend => friend.id === userId);
    if (!exists) {
      onlineFriends.value.push({
        id: userId,
        username: username,
        isOnline: true
      });
    }
  } else {
    onlineFriends.value = onlineFriends.value.filter(friend => friend.id !== userId);
  }
};



onMounted(async () => {

  const res = await getNotifications();
  notifications.value = res.notifications
  notificationsCount.value = res.count;

  signalR2.on('ReceiveNotification', handleNotification);


    await fetchOnlineFriends();
    
    if (signalR.state === HubConnectionState.Disconnected) {
      await signalRConnection.start();
    }
    
    signalRConnection.on("UserStatusChanged", handleUserStatusChanged);
    
    signalRConnection.onclose((error) => {
      console.error('SignalR connection closed:', error);
    });

});

onUnmounted(() => {
  signalRConnection.off("UserStatusChanged", handleUserStatusChanged);
});
</script>
