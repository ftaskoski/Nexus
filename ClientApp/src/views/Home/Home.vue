<template>
    <div>
        <h1 class="flex justify-center items-center h-screen">Home</h1>
    </div>
</template>

<script setup lang="ts">
import { onMounted,inject } from 'vue';
import { HubConnectionState, HubConnection } from '@microsoft/signalr';
import { notificationsCount, notifications } from '@/components/Panels/Notifications/store';
import { handleNotification } from '@/components/Panels/Notifications/store';
import { getNotifications } from '@/components/Panels/Notifications/store';

const signalR = inject('signalR') as HubConnection;


onMounted(async () => {
  const res = await getNotifications();
  notifications.value = res.notifications
  notificationsCount.value = res.count;

  if (!signalR) return;
  
  try {
    signalR.on('ReceiveNotification', handleNotification);

    if (signalR.state === HubConnectionState.Disconnected) {
      await signalR.start();
    }
  } catch (err) {
    console.error('SignalR connection error:', err);
  }
});



</script>

<style scoped>

</style>