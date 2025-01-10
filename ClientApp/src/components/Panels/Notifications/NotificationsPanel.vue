<template>
  <div>
    <div class="mb-4">
      <h2 class="text-lg font-semibold">Notifications</h2>
    </div>
    <div v-if="notifications.length > 0">
      <NavigationRow v-for="notification in notifications" :key="notification.id">
        <p>{{ notification.message }}</p>
        <p class="text-sm text-gray-500">{{ notification.senderName }}</p>
      </NavigationRow>
    </div>
    <div v-else>
      <p>No new notifications.</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { inject, ref, onMounted, onUnmounted} from 'vue';
import { HubConnection, HubConnectionState } from '@microsoft/signalr';
import NavigationRow from '@/components/NavigationRow.vue';
import type { Notification } from './types';
import { fetchy } from '@/plugins/axios';

const signalR = inject('signalR') as HubConnection;
const notifications = ref<Notification[]>([]);


const handleNotification = (notification: Notification) => {
  notifications.value.push({
    id: notification.id,    
    message: notification.message,
    type: notification.type,
    senderId: notification.senderId,
    senderName: notification.senderName,
  });
}; 

async function getNotifications() { 
   const res = await fetchy({
    url: 'friendrequests/notifications',
    method: 'GET',
  })
  return res.payload
}

onMounted(async () => {

  if (!signalR) {
    return;
  }
  try {
    signalR.on('ReceiveNotification', handleNotification);

    if (signalR.state === HubConnectionState.Disconnected) {
      await signalR.start();
    }
  } catch (err) {
    console.error('SignalR connection error:', err);
  }
});

onUnmounted(() => {
  if (signalR) {
    signalR.off('ReceiveNotification', handleNotification);
  }
});
</script>