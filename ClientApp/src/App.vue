<script setup lang="ts">
import { RouterView } from 'vue-router'
import {isAuthenticated} from '../authStore/store'
//@ts-ignore
import Nav from './components/Nav.vue';

</script>

<template>
  
 <Nav v-if="isAuthenticated"/>
  <RouterView />
</template>

