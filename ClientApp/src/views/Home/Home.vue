<script setup lang="ts">
import type { HubConnection } from '@microsoft/signalr'
import type { OnlineFriend, RecentChat } from './types'
import { HubConnectionState } from '@microsoft/signalr'
import { inject, onMounted, onUnmounted, ref } from 'vue'

import { useRouter } from 'vue-router'
import Card from '@/components/Card.vue'
import Icon from '@/components/Icon.vue'
import Input from '@/components/Input.vue'
import { getChatId } from '@/components/Panels/Messages/store'
import { formatChatTime } from '@/utils/dateUtils'
import { getFriendsData, getRecentChats } from './store'

const signalRConnection = inject( 'signalR1' ) as signalR.HubConnection
const signalRConnection2 = inject( 'signalR2' ) as signalR.HubConnection
const router = useRouter()
const signalR = inject( 'signalR1' ) as HubConnection

const searchQuery = ref<string>( '' )
const onlineFriends = ref<OnlineFriend[]>( [] )

const recentChats = ref<RecentChat[]>( [] )
const loading = ref<boolean>( false )

async function fetchOnlineFriends() {

  const response = await getFriendsData()
  const data = response.payload || []
  onlineFriends.value = data

}

async function getRecentChatsData() {
  const response = await getRecentChats()
  const data = response.payload || []
  recentChats.value = data
}

async function toChat( id: string ) {
  const res = await getChatId( id )
  router.push( `/chat/${res.payload.chatRoomId}` )
  localStorage.setItem( 'friendId', id )
}

async function handleUserStatusChanged() {
  await fetchOnlineFriends()
}

async function getData() {
  loading.value = true
  await getRecentChatsData()
  await fetchOnlineFriends()
  loading.value = false
}

signalRConnection2.on( 'FriendRequestAccepted', handleUserStatusChanged )

onMounted( async () => {

  await getData()

  if ( signalR.state === HubConnectionState.Disconnected ) {
    await signalRConnection.start()
  }

  signalRConnection.on( 'UserStatusChanged', handleUserStatusChanged )

  signalRConnection.onclose(( error ) => {
    console.error( 'SignalR connection closed:', error )
  })

})

onUnmounted(() => {
  signalRConnection.off( 'UserStatusChanged', handleUserStatusChanged )
})
</script>

<template>
  <div>
    <Card :grow="true" :loading="loading">
      <template #header>
        <div class="flex items-center justify-between gap-2 p-4 border-b">
          <h1 class="text-xl font-semibold">
            Messages
          </h1>
          <div class="flex items-center gap-4">
            <Input
              id="search"
              v-model="searchQuery"
              placeholder="Search messages..."
              icon="search"
              icon-fill="none"
              class="w-64"
            />

          </div>
        </div>
      </template>

      <template #content>
        <div class="p-4 space-y-4">
          <div class="space-y-2">
            <h2 class="text-sm font-medium text-gray-500">
              Recent Chats
            </h2>
            <div class="space-y-2 overflow-y-auto h-96">
              <div
                v-for="recentChat in recentChats" :key="recentChat.id"
                class="flex items-center gap-3 p-3 rounded-lg hover:bg-gray-50 cursor-pointer transition-colors"
                @click="toChat(recentChat.id)"
              >
                <div class="relative">
                  <div class="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center">
                    <Icon icon="user" size="28" class="text-blue-600" />
                  </div>

                  <div v-if="recentChat.isOnline" class="absolute bottom-0 right-0 w-3 h-3 bg-green-500 rounded-full border-2 border-white" />
                </div>

                <div class="flex-1">

                  <div class="flex items-center justify-between">
                    <span class="font-medium">{{ recentChat.username }}</span>
                    <span class="text-sm text-gray-500">{{ formatChatTime(recentChat.sentAt) }} </span>
                  </div>

                  <p class="text-sm text-gray-600 truncate">
                    {{ recentChat.lastMessage || 'No messages yet' }}
                  </p>
                </div>
              </div>
            </div>
          </div>

          <div class="mt-6">
            <h2 class="text-sm font-medium text-gray-500 mb-3">
              Online Friends
            </h2>
            <div class="flex gap-4 overflow-x-auto pb-2">
              <div
                v-for="friend in onlineFriends" :key="friend.id" class="flex flex-col items-center space-y-1 min-w-[60px] cursor-pointer"
                @click="toChat(friend.id)"
              >
                <div class="relative">
                  <div class="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center">
                    <Icon icon="user" size="28" class="text-blue-600" />
                  </div>
                  <div class="absolute bottom-0 right-0 w-3 h-3 bg-green-500 rounded-full border-2 border-white" />
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
