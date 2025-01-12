<template>
  <div class="max-w-md mx-auto p-4">
    <div class="mb-4">
      <h2 class="text-xl font-semibold text-gray-800">Notifications</h2>
    </div>
    
    <div v-if="notifications.length > 0" class="space-y-3">
      <NotificationRow
        v-for="notification in notifications" 
        :key="notification.id"
      >
        <template #header>
          {{ notification.message }}
        </template>

        <template #content>
          <div class="flex items-center space-x-2">
            <div class="bg-blue-100 p-2 rounded-full">
              <Icon 
                icon="bell" 
                class="h-4 w-4 text-blue-600"
              />
            </div>
            <p class="text-sm text-gray-500">
              {{ notification.senderName }}
            </p>
          </div>

          <div class="flex items-center space-x-2">
            <Button 
              type="primary"
              size="s"
              class="w-auto"
            >
              <div class="flex items-center">
                <Icon 
                  icon="checkmark" 
                  class="h-4 w-4 mr-1"
                />
                Accept
              </div>
            </Button>
            <Button 
              @click="deleteNotification(notification.id)"
              type="danger"
              size="s"
              class="w-auto"
            >
              <div class="flex items-center">
                <Icon 
                  icon="cancel" 
                  class="h-4 w-4 mr-1"
                />
                Decline
              </div>
            </Button>
          </div>
        </template>
      </NotificationRow>
    </div>

    <div 
      v-else 
      class="text-center py-8 px-4 rounded-lg border-2 border-dashed border-gray-200"
    >
      <Icon 
        icon="bell" 
        class="mx-auto h-10 w-10 text-gray-400"
      />
      <p class="mt-2 text-sm text-gray-500">No new notifications</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { inject, onMounted, onUnmounted } from 'vue';
import { HubConnection, HubConnectionState } from '@microsoft/signalr';
import Icon from '@/components/Icon.vue';
import Button from '@/components/Button.vue';
import NotificationRow from '@/components/Rows/NotificationRow.vue';
import { notificationsCount, notifications, handleNotification } from './store';
import { getNotifications } from './store';
import { fetchy } from '@/plugins/axios';

const signalR = inject('signalR') as HubConnection;

async function deleteNotification(id: string) {
  await fetchy({
    url: `friendrequests/delete/${id}`,
    method: "DELETE",
  })

  const res = await getNotifications();
  notifications.value = res.notifications;
  notificationsCount.value = res.count;

}

onMounted(async () => {
  const res = await getNotifications();
  notifications.value = res.notifications;
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

onUnmounted(() => {
  if (signalR) {
    signalR.off('ReceiveNotification', handleNotification);
  }
});


</script>