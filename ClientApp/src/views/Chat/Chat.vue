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
            <div class="mb-4">
              <div class="mr-auto max-w-xs md:max-w-md p-3 bg-gray-100 text-gray-800 rounded-lg rounded-bl-none">
                <p>Lorem ipsum dolor sit amet consectetur, adipisicing elit. Excepturi quod repellat est, animi impedit officiis natus molestias iste placeat reiciendis quam, maiores ipsam provident itaque alias, perferendis enim labore inventore.</p>
                <div class="text-xs mt-1 text-gray-500">10:25 AM</div>
              </div>
            </div>
            
            <div class="mb-4">
              <div class="ml-auto max-w-xs md:max-w-md p-3 bg-blue-500 text-white rounded-lg rounded-br-none">
                <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Rem earum cumque odit reprehenderit omnis? Voluptatum similique sequi ipsa asperiores, itaque nisi sit rem doloremque quia qui, eius harum, distinctio corporis!</p>
                <div class="text-xs mt-1 text-blue-100">10:26 AM</div>
              </div>
            </div>
            
            <div class="mb-4">
              <div class="mr-auto max-w-xs md:max-w-md p-3 bg-gray-100 text-gray-800 rounded-lg rounded-bl-none">
                <p>Lorem ipsum dolor sit, amet consectetur adipisicing elit. Mollitia fugit dolores soluta id alias aspernatur dolorem, consequatur minus earum consectetur suscipit sed sit ipsa laudantium? Tenetur saepe consequuntur repudiandae natus.</p>
                <div class="text-xs mt-1 text-gray-500">10:28 AM</div>
              </div>
            </div>
            
            <div class="mb-4">
              <div class="ml-auto max-w-xs md:max-w-md p-3 bg-blue-500 text-white rounded-lg rounded-br-none">
                <p>Lorem ipsum dolor sit amet consectetur, adipisicing elit. Nisi reiciendis ratione ut sint. Voluptate minus iusto quibusdam. Quisquam, asperiores harum magni ducimus laborum molestias dolore, possimus incidunt nihil itaque fugit.</p>
                <div class="text-xs mt-1 text-blue-100">10:30 AM</div>
              </div>
            </div>
            
            <ChatLoading />
          </div>
        </template>
  
        <template #footer>
            <div class="border-t p-4 ">
                <div class="flex items-center gap-2">
                    <Input
                        id="chat-input"
                        modelValue=""
                        placeholder="Type a message..."
                        class="flex-1"
                        @update:modelValue="(value) => {}"
                    />
                    <div class="flex justify-end">
                        <Button 
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
  import { getFriend } from './store';
  import type { Friend } from '@/components/Panels/Messages/types';

  const friendId = localStorage.getItem('friendId')
  const friend = ref<Friend | null>(null)

  onMounted(async() => {
    const res = await getFriend(friendId)
    friend.value = res.payload
  });


  </script>