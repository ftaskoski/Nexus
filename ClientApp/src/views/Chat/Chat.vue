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
        <div ref="scrollContainer" class="flex flex-col h-96 overflow-y-auto p-4" @scroll="handleScroll">
          <div v-if="loading" class="flex justify-center items-center h-10">
            <Icon v-if="loading" icon="loading" size="20px" class="animate-spin text-black mb-4" fill="currentColor"
              color="none">
            </Icon>
          </div>
          <div v-for="msg in messages" :key="msg.id" class="mb-4">
            <div :class="[
              msg.receiverId === friendId ? 'ml-auto bg-blue-500 text-white rounded-br-none' : 'mr-auto bg-gray-100 text-gray-800 rounded-bl-none',
              'max-w-xs md:max-w-md p-3 rounded-lg'
            ]">
              <p>{{ msg.content }}</p>
              <div :class="['text-xs mt-1', msg.receiverId === friendId ? 'text-blue-100' : 'text-gray-500']">
                <span>{{ formatChatTime(msg.sentAt) }}</span>
              </div>
            </div>
          </div>

          <IsTyping v-if="isTyping" />
        </div>
      </template>


      <template #footer>
        <div class="border-t p-4 ">
          <div class="flex items-center gap-2">
            <Input id="chat-input" v-model="message" placeholder="Type a message..." class="flex-1"
              @keyup.enter="sendMsg" @input="handleTyping" />
            <div class="flex justify-end">
              <Button @click="sendMsg" type="primary" size="m">
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
import IsTyping from '@/components/IsTyping.vue';
import Button from '@/components/Button.vue';
import Input from '@/components/Input.vue';
import Card from '@/components/Card.vue';
import Icon from '@/components/Icon.vue';

import { HubConnection, HubConnectionState } from "@microsoft/signalr";

import type { Friend } from '@/components/Panels/Messages/types';
import { getFriend, sendMessage, getMessages } from './store';
import { useRouter, useRoute } from 'vue-router';
import { onMounted, ref, nextTick, onUnmounted, inject } from 'vue';
import type { Message } from './types';
import { formatChatTime } from '@/utils/dateUtils';

const friendId = localStorage.getItem('friendId')
const friend = ref<Friend | null>(null)
const router = useRouter()
const route = useRoute()
const chatRoomId = route.params.id as string
const signalRConnection = inject('messageSignalR') as HubConnection;

let message = ref<string>('')
let messages = ref<Message[]>([])
let skip = ref<number>(0)
let loading = ref<boolean>(false)
let isConnected = ref<boolean>(false)
const take = 5
let typingTimeout: number | undefined;
const isTyping = ref<boolean>(false);


async function sendMsg() {
  if (!message.value.trim()) return;

  await ensureConnection();

  const res = await sendMessage(friendId, message.value, chatRoomId);
  message.value = '';
}

function handleReceiveMessage(newMessage: Message) {

  if (!messages.value.some(m => m.id === newMessage.id)) {
    messages.value.push(newMessage);
    nextTick(() => {
      scrollToBottom();
    });
  }
}

async function getMsgs() {
  const res = await getMessages(chatRoomId, skip.value, take)
  return res;
}

async function getData() {
  const [friendRes, messagesRes] = await Promise.all([
    getFriend(friendId),
    getMsgs()
  ]);
  friend.value = friendRes.payload;
  messages.value = messagesRes.payload.reverse();
}

const scrollContainer = ref<HTMLElement | null>(null)

function scrollToBottom() {
  if (scrollContainer.value) {
    scrollContainer.value.scrollTo(0, scrollContainer.value.scrollHeight);
  }
}

async function handleScroll() {
  if (!scrollContainer.value) return;

  if (scrollContainer.value.scrollTop === 0 && !loading.value) {
    loading.value = true;
    skip.value += take;
    const res = await getMsgs();
    if (res && res.payload.length > 0) {
      messages.value = [...res.payload.reverse(), ...messages.value];
    }
    loading.value = false;
    await nextTick();
    scrollContainer.value.scrollTop = 10;
  }
}

async function ensureConnection() {
  if (!signalRConnection) {
    console.error('SignalR connection not available');
    return false;
  }

  if (signalRConnection.state === HubConnectionState.Disconnected) {
    await signalRConnection.start();
  }

  if (signalRConnection.state === HubConnectionState.Connected) {
    await signalRConnection.invoke("JoinChatRoom", chatRoomId);
    isConnected.value = true;
    return true;
  }


  return false;
}

function setupSignalRHandlers() {
  if (!signalRConnection) return;

  signalRConnection.off("ReceiveMessage");
  signalRConnection.off("UserTyping");
  signalRConnection.off("UserStoppedTyping");

  signalRConnection.on("ReceiveMessage", handleReceiveMessage);
  signalRConnection.on("UserTyping", (username: string) => {
    isTyping.value = true;
  });

  signalRConnection.on("UserStoppedTyping", (username: string) => {
    isTyping.value = false;
  });

  signalRConnection.onclose((error) => {
    console.error('SignalR connection closed:', error);
    isConnected.value = false;

    setTimeout(async () => {
      await ensureConnection();
    }, 3000);
  });

  signalRConnection.onreconnected((connectionId) => {
    isConnected.value = true;
    signalRConnection.invoke("JoinChatRoom", chatRoomId)
      .catch(err => console.error("Error rejoining chat room after reconnect:", err));
  });
}

function handleTyping() {
  if (!signalRConnection || signalRConnection.state !== HubConnectionState.Connected) return;

  signalRConnection.invoke("StartTyping", chatRoomId, "You").catch(console.error);

  if (typingTimeout) clearTimeout(typingTimeout);

  typingTimeout = window.setTimeout(() => {
    signalRConnection.invoke("StopTyping", chatRoomId, "You").catch(console.error);
  }, 2000);
}

onMounted(async () => {
  await getData();
  scrollToBottom();

  setupSignalRHandlers();

  await ensureConnection();
});

onUnmounted(() => {
  if (signalRConnection) {
    signalRConnection.off("ReceiveMessage");

    if (signalRConnection.state === HubConnectionState.Connected) {
      signalRConnection.invoke("LeaveChatRoom", chatRoomId)
        .catch(err => console.error("Error leaving chat room:", err));
    }
  }
});

</script>