<script setup lang="ts">
import type { HubConnection } from '@microsoft/signalr'
import { HubConnectionState } from '@microsoft/signalr'
import { inject, onMounted, onUnmounted } from 'vue'
import { getNotifications, handleNotification, notifications, notificationsCount } from './store'

const signalR = inject( 'signalR2' ) as HubConnection

async function getData() {
  const res = await getNotifications()
  notifications.value = res.notifications
  notificationsCount.value = res.count
}

onMounted( async () => {
  await getData()

  if ( !signalR )
    return

  signalR.on( 'ReceiveNotification', handleNotification )

  if ( signalR.state === HubConnectionState.Disconnected ) {
    await signalR.start()
  }
})

onUnmounted(() => {
  if ( signalR ) {
    signalR.off( 'ReceiveNotification', handleNotification )
  }
})
</script>
