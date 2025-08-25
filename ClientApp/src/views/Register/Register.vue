<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import Button from '@/components/Button.vue'
import Card from '@/components/Card.vue'
import Icon from '@/components/Icon.vue'
import Input from '@/components/Input.vue'
import Message from '@/components/Message.vue'
import { fetchy } from '@/plugins/axios'
import { successfulAuth } from '../../../authStore/store'

const username = ref<string>( '' )
const email = ref<string>( '' )
const password = ref<string>( '' )
const loading = ref<boolean>( false )
const showPassword = ref<boolean>( false )
const error = ref<boolean>( false )
const router = useRouter()

async function handleSubmit() {
  loading.value = true
  const response = await fetchy({
    url:    'user',
    method: 'POST',
    data:   {
      username: username.value,
      email:    email.value,
      password: password.value,
    },
  })

  if ( response.payload ) {
    loading.value = false
    successfulAuth( router )
  }

  if ( response.errors ) {
    loading.value = false
    error.value = true
  }
};
</script>

<template>
  <Card>
    <template #header>
      <div class="text-center">

        <h2 class="mt-4 text-2xl font-semibold text-gray-900">
          Welcome to  <span class="logo">Nexus</span>
        </h2>

        <p class="mt-2 text-sm text-gray-500">
          Register to continue messaging
        </p>
      </div>
    </template>

    <template #content>
      <form class="mt-8 space-y-5" @submit.prevent="handleSubmit">
        <div class="space-y-1">
          <Input
            id="username"
            v-model="username"
            type="text"
            placeholder="Username"
            label="Username"
            icon="user"
            icon-fill="#9ca3af"
            icon-color=""
            required
            fill-rule="evenodd"
          />
        </div>

        <div class="space-y-1">
          <Input
            id="email"
            v-model="email"
            type="email"
            placeholder="name@example.com"
            label="Email"
            icon="email"
            icon-fill="#9ca3af"
            icon-color=""
            required
            fill-rule="evenodd"
            :icon-align="true"
          />
        </div>

        <div class="space-y-1 relative">
          <Input
            id="password"
            v-model="password"
            :type="showPassword ? 'text' : 'password'"
            placeholder="Password"
            label="Password"
            icon="lock"
            icon-fill="#9ca3af"
            icon-color=""
            required
            fill-rule="evenodd"
            :icon-align="true"
          />
          <div
            class="absolute inset-y-0 right-0 top-3.5 pr-3 flex items-center cursor-pointer text-gray-400 hover:text-gray-500"
            @click="showPassword = !showPassword"
          >
            <Icon v-if="showPassword" icon="visible-password" stroke-width="2" />
            <Icon v-else icon="hidden-password" stroke-width="2" />
          </div>
        </div>

        <Button size="l" :disabled="loading">
          <Icon
            v-if="loading"
            icon="loading"
            size="20px"
            class="animate-spin -ml-1 mr-3 h-5 w-5 text-white"
            fill="currentColor"
            color="none"
          />

          {{ loading ? "Signing up..." : "Sign up" }}
        </Button>
      </form>
    </template>

    <template #footer>
      <p class="text-center text-sm text-gray-500 pt-2">
        Already have an account?
        <router-link to="/login" class="font-medium text-blue-500 hover:text-blue-600">
          Sign in
        </router-link>
      </p>

      <Message class="mt-2 flex justify-center items-center" :show="error" type="error" @close="error = false">
        <p>Email or username is already in use</p>
      </Message>
    </template>
  </Card>
</template>
