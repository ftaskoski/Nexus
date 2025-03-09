<template>
  <div>
    <Card :grow="true">
      <template #header>
        <div class="flex items-center justify-between gap-2 p-4 border-b">
          <div class="flex items-center gap-3">
            <div class="w-10 h-10 bg-blue-100 rounded-full flex items-center justify-center">
              <Icon icon="user" class="text-blue-600" />
            </div>
            <h1 class="text-xl font-semibold">AI Assistant</h1>
          </div>
        </div>
      </template>

      <template #content>
        <div class="flex flex-col h-96 overflow-y-auto p-4">
          <div v-if="sentMessage" class="mb-4">
            <div class="ml-auto max-w-xs md:max-w-md p-3 bg-blue-500 text-white rounded-lg rounded-br-none">
              <p>{{ sentMessage }}</p>
              <div class="text-xs mt-1 text-blue-100">{{ new Date().toLocaleTimeString() }}</div>
            </div>
          </div>

          <div v-if="chatResponse && !isLoading" class="mb-4">
            <div class="mr-auto max-w-xs md:max-w-md p-3 bg-gray-100 text-gray-800 rounded-lg rounded-bl-none">
              <div v-html="formatResponse(chatResponse)"></div>
              <div class="text-xs mt-1 text-gray-500">{{ new Date().toLocaleTimeString() }}</div>
            </div>
          </div>

          <ChatLoading v-if="isLoading" />
        </div>
      </template>

      <template #footer>
        <div class="border-t p-4">
          <div class="flex items-center gap-2">
            <Input
              id="chat-input"
              v-model="userInput"
              placeholder="Type a message..."
              class="flex-1"
              @keyup.enter="sendMessage"
            />
            <div class="flex justify-end">
              <Button
                type="primary"
                size="m"
                @click="sendMessage"
                :disabled="isLoading || !userInput.trim()"
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
import { ref, nextTick } from "vue";
import Card from '@/components/Card.vue';
import Icon from '@/components/Icon.vue';
import Input from '@/components/Input.vue';
import Button from '@/components/Button.vue';
import { fetchy } from "@/plugins/axios";
import ChatLoading from "@/components/ChatLoading.vue";
import hljs from "highlight.js"; 
import "highlight.js/styles/github.css"; 

const userInput = ref<string>("");
const chatResponse = ref<string>("");
const sentMessage = ref<string>("");
const isLoading = ref<boolean>(false);

async function sendMessage() {
  if (!userInput.value.trim() || isLoading.value) return;

  isLoading.value = true;
  sentMessage.value = userInput.value;

  const response = await fetchy({
    url: "chat",
    method: "POST",
    data: { 
      message: userInput.value
    },
  });

  if (response.payload) {
    chatResponse.value = response.payload.reply;
    sentMessage.value = userInput.value;
  } else {
    chatResponse.value = "Sorry, something went wrong. Please try again.";
  }

  isLoading.value = false;
  userInput.value = "";

  await nextTick();
  highlightCode();
}
function formatResponse(response: string) {
  return response.replace(/```([\s\S]*?)```/g, (_, code) => {
    const escapedCode = code
      .replace(/&/g, "&amp;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;")
      .replace(/"/g, "&quot;")
      .replace(/'/g, "&#039;");
    return `<pre><code>${escapedCode}</code></pre>`;
  });
}
function highlightCode() {
  document.querySelectorAll("pre code").forEach((block) => {
    hljs.highlightBlock(block as HTMLElement);
  });
}


</script>
<style scoped>
pre {
  background-color: #f6f8fa;
  padding: 12px;
  border-radius: 4px;
  overflow-x: auto;
}

code {
  font-family: "Courier New", Courier, monospace;
  font-size: 14px;
}
</style>