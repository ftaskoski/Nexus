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
        <div ref="chatContainer" class="flex flex-col h-96 overflow-y-auto p-4">
          <div v-if="sentMessage" class="mb-4">
            <div class="ml-auto max-w-xs md:max-w-md p-3 bg-blue-500 text-white rounded-lg rounded-br-none">
              <p>{{ sentMessage }}</p>
              <div class="text-xs mt-1 text-blue-100">{{ new Date().toLocaleTimeString() }}</div>
            </div>
          </div>

          <div v-if="chatResponse && !isLoading" class="mb-4">
            <div class="mr-auto max-w-xs md:max-w-md p-3 bg-gray-100 text-gray-800 rounded-lg rounded-bl-none">
              <div ref="responseContainer" v-html="formattedResponse"></div>
              <div class="text-xs mt-1 text-gray-500">{{ new Date().toLocaleTimeString() }}</div>
            </div>
          </div>

          <IsTyping v-if="isLoading" />
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
import { ref, computed, nextTick, onMounted, onBeforeUnmount, watch } from "vue";
import Card from '@/components/Card.vue';
import Icon from '@/components/Icon.vue';
import Input from '@/components/Input.vue';
import Button from '@/components/Button.vue';
import { fetchy } from "@/plugins/axios";
import IsTyping from "@/components/IsTyping.vue";
import hljs from "highlight.js"; 
import "highlight.js/styles/github.css"; 

const userInput = ref<string>("");
const chatResponse = ref<string>("");
const sentMessage = ref<string>("");
const isLoading = ref<boolean>(false);
const chatContainer = ref<HTMLElement | null>(null);
const responseContainer = ref<HTMLElement | null>(null);
const copyButtonListeners = ref<{ element: HTMLElement, listener: EventListener }[]>([]);

const formattedResponse = computed(() => {
  return formatResponse(chatResponse.value);
});

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
  } else {
    chatResponse.value = "Sorry, something went wrong. Please try again.";
  }

  isLoading.value = false;
  userInput.value = "";
  
  await nextTick();
  setupCodeBlocks();
}

function formatResponse(response: string) {
  if (!response) return "";
  
  let codeBlockCount = 0;
  return response.replace(/```([\s\S]*?)```/g, (_, code) => {
    const codeBlockId = `code-block-${Date.now()}-${codeBlockCount++}`;
    const escapedCode = code
      .replace(/&/g, "&amp;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;")
      .replace(/"/g, "&quot;")
      .replace(/'/g, "&#039;");
    
    return `
      <div class="code-container relative">
        <div class="copy-button-container absolute top-2 right-2">
          <button class="copy-button bg-gray-200 hover:bg-gray-300 text-gray-700 px-2 py-1 rounded text-xs" data-code-id="${codeBlockId}">
            Copy
          </button>
        </div>
        <pre><code id="${codeBlockId}">${escapedCode}</code></pre>
      </div>
    `;
  });
}

function setupCodeBlocks() {
  removeAllCopyButtonListeners();
  
  highlightCode();
  
  setupCopyButtons();
}

function highlightCode() {
  if (!responseContainer.value) return;
  
  const codeBlocks = responseContainer.value.querySelectorAll("pre code");
  codeBlocks.forEach((block) => {
    hljs.highlightBlock(block as HTMLElement);
  });
}

function setupCopyButtons() {
  if (!responseContainer.value) return;
  
  const buttons = responseContainer.value.querySelectorAll(".copy-button");
  buttons.forEach((button) => {
    const listener = handleCopyClick.bind(null);
    button.addEventListener("click", listener);
    
    copyButtonListeners.value.push({
      element: button as HTMLElement,
      listener: listener
    });
  });
}

function removeAllCopyButtonListeners() {
  copyButtonListeners.value.forEach(({ element, listener }) => {
    element.removeEventListener("click", listener);
  });
  copyButtonListeners.value = [];
}

function handleCopyClick(event: Event) {
  const button = event.currentTarget as HTMLButtonElement;
  const codeId = button.getAttribute("data-code-id");
  const codeElement = document.getElementById(codeId as string);
  
  if (codeElement) {
    const codeText = codeElement.textContent || "";
    
    navigator.clipboard.writeText(codeText).then(() => {
      const originalText = button.textContent;
      button.textContent = "Copied";
      button.classList.add("bg-green-200", "text-green-700");
      button.classList.remove("bg-gray-200", "text-gray-700");
      
      setTimeout(() => {
        button.textContent = originalText;
        button.classList.remove("bg-green-200", "text-green-700");
        button.classList.add("bg-gray-200", "text-gray-700");
      }, 2000);
    }).catch((err) => {
      console.error("Failed to copy text: ", err);
    });
  }
}


watch(formattedResponse, () => {
  nextTick(() => {
    setupCodeBlocks();
  });
});

onMounted(() => {
  setupCodeBlocks();
});

onBeforeUnmount(() => {
  removeAllCopyButtonListeners();
});
</script>

<style scoped>
pre {
  background-color: #f6f8fa;
  padding: 12px;
  border-radius: 4px;
  overflow-x: auto;
  margin-top: 8px;
}

code {
  font-family: "Courier New", Courier, monospace;
  font-size: 14px;
  white-space: pre;
  word-wrap: normal;
  overflow-x: auto;
}

.code-container {
  position: relative;
  margin: 1rem 0;
}

.copy-button {
  transition: all 0.2s ease;
}

:deep(pre) {
  display: block;
  overflow-x: auto;
  padding: 1em;
  background: #f6f8fa;
  border-radius: 4px;
}

:deep(code) {
  display: block;
  font-family: "Courier New", Courier, monospace;
  white-space: pre;
  word-spacing: normal;
  word-break: normal;
  word-wrap: normal;
  line-height: 1.5;
}
</style>