<template>
  <Card>
    <template #header>
      <div class="text-center">
 
        <h2 class="mt-4 text-2xl font-semibold text-gray-900">
          Welcome to <span class="logo">Nexus</span>
        </h2>
        <p class="mt-2 text-sm text-gray-500">Sign in to continue messaging</p>
      </div>
    </template>

    <template #content>
      <form class="mt-8 space-y-5" @submit.prevent="handleSubmit">
        <div class="space-y-1">
          <Input
            id="email"
            type="email"
            placeholder="name@example.com"
            label="Email"
            icon="email"
            iconFill="#9ca3af"
            iconColor=""
            required
            v-model="email"
            fillRule="evenodd"
          />
        </div>

        <div class="space-y-1 relative">
          <Input
            id="password"
            :type="showPassword ? 'text' : 'password'"
            placeholder="Password"
            label="Password"
            icon="lock"
            iconFill="#9ca3af"
            iconColor=""
            required
            v-model="password"
            fillRule="evenodd"
          />
          <div
            @click="showPassword = !showPassword"
            class="absolute inset-y-0 right-0 top-3.5 pr-3 flex items-center cursor-pointer text-gray-400 hover:text-gray-500"
          >
            <Icon v-if="showPassword" icon="visible-password" strokeWidth="2"/>
            <Icon v-else icon="hidden-password"  strokeWidth="2" />
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
          >
          </Icon>

          {{ loading ? "Signing in..." : "Sign in" }}</Button
        >
      </form>
    </template>

    <template #footer>
      <p class="text-center text-sm text-gray-500 pt-2">
        New to Nexus?
        <router-link to="/register" href="#" class="font-medium text-blue-500 hover:text-blue-600">
          Create an account
        </router-link>
      </p>
      <div class="flex items-center justify-center pt-2">
        <a
          href="#"
          class="text-sm font-medium text-blue-500 hover:text-blue-600"
        >
          Forgot password?
        </a>
      </div>

      <Message class="mt-2 flex justify-center items-center" :show="error"  @close="error = false" type="error" >
        <p>Invalid email or password</p>
    </Message>

    </template>

  </Card>


</template>

<script setup lang="ts">
import { ref } from "vue";
import Card from "@/components/Card.vue";
import Icon from "@/components/Icon.vue";
import Button from "@/components/Button.vue";
import Input from "@/components/Input.vue";
import Message from "@/components/Message.vue";
import { fetchy } from "@/plugins/axios";
const email = ref<string>("");
const password = ref<string>("");
const loading = ref<boolean>(false);
const showPassword = ref<boolean>(false);
const error = ref<boolean>(false);

const handleSubmit = async () => {
  loading.value = true;
  const response = await fetchy({
    url: "user/login",
    method: "POST",
    data: { email: email.value, password: password.value },
  });

  if (response.payload) {
    loading.value = false;
  }

  if (response.errors) {
    loading.value = false;
    error.value = true;
  }
};

</script>
