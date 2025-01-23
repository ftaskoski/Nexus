<template>
  </template>
  
  <script setup lang="ts">
  import { inject, onMounted, onUnmounted } from "vue";
  import { HubConnection, HubConnectionState } from "@microsoft/signalr";
  import { notifications, notificationsCount, handleNotification } from "./store";
  import { getNotifications } from "./store";
  
  const signalR = inject("signalR2") as HubConnection;
  
  async function getData() {
    const res = await getNotifications();
    notifications.value = res.notifications;
    notificationsCount.value = res.count;
  }
  
  onMounted(async () => {
    await getData();
  
    if (!signalR) return;
  
    signalR.on("ReceiveNotification", handleNotification);
  
    if (signalR.state === HubConnectionState.Disconnected) {
      await signalR.start();
    }
  });
  
  onUnmounted(() => {
    if (signalR) {
      signalR.off("ReceiveNotification", handleNotification);
    }
  });
  </script>