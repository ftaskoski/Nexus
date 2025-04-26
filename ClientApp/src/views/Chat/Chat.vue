<template>
    <div>
      <Card :grow="true">
        <template #header>
          <div class="flex items-center justify-between gap-2 p-4 border-b">
            <div class="flex items-center gap-3">
              <div class="w-10 h-10 bg-blue-100 rounded-full flex items-center justify-center">
                <Icon icon="user" class="text-blue-600" />
              </div>
              <h1 class="text-xl font-semibold">{{ friend?.username }}</h1>
            </div>
          </div>
        </template>
  
        <template #content>
          <div class="flex flex-col h-96 overflow-y-auto p-4">
            <div v-for="msg in messages" :key="msg.id" class="mb-4">
              <div
                :class="[
                  msg.receiverId === friendId ? 'ml-auto bg-blue-500 text-white rounded-br-none' : 'mr-auto bg-gray-100 text-gray-800 rounded-bl-none',
                  'max-w-xs md:max-w-md p-3 rounded-lg'
                ]"
              >
                <p>{{ msg.content }}</p>
                <div :class="['text-xs mt-1', msg.receiverId === friendId ? 'text-blue-100' : 'text-gray-500']">
                  <span>{{ new Date(msg.sentAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) }}</span>
                </div>
              </div>
            </div>

            <!-- <ChatLoading /> -->
          </div>
        </template>

  
        <template #footer>
            <div class="border-t p-4 ">
                <div class="flex items-center gap-2">
                    <Input
                        id="chat-input"
                        v-model="message"
                        placeholder="Type a message..."
                        class="flex-1"
                        @keyup.enter="sendMsg"
                    />
                    <div class="flex justify-end">
                        <Button 
                        @click="sendMsg"
                        type="primary"
                        size="m"
                        >
                        <Icon icon="send" class="text-white" />
                        </Button>
                    </div>
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
  import Button from '@/components/Button.vue';
  import ChatLoading from '@/components/ChatLoading.vue';

  import { onMounted,ref } from 'vue';
  import { getFriend, sendMessage, getMessages } from './store';
  import { useRouter, useRoute } from 'vue-router';
  import type { Friend } from '@/components/Panels/Messages/types';
  import type { Message } from './types';

  const friendId = localStorage.getItem('friendId')
  const friend = ref<Friend | null>(null)
  const router = useRouter()
  const route = useRoute()
  const chatRoomId = route.params.id as string
  
  
  let message = ref<string>('')
  let messages = ref<Message[]>([])
  async function sendMsg() {
      const res = await sendMessage(friendId, message.value, chatRoomId)
      message.value = ''
  }

  async function getMsgs() {
    const res = await getMessages(chatRoomId)
    return res;
  }

  async function getData() {
  const [friendRes, messagesRes] = await Promise.all([
    getFriend(friendId),
    getMsgs()
  ]);
  friend.value = friendRes.payload;
  messages.value = messagesRes.payload;
}


  onMounted(async() => {
    await getData()
  });


  </script>