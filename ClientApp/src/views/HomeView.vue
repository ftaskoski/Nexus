<script setup lang="ts">
import { fetchy } from "@/plugins/axios"
import { onMounted } from 'vue';

onMounted(async() => {

const res = await fetchy({
  url: "user/testuser",
  method: "GET"
 })
 console.log(res.payload);

});
</script>

<template>

</template>
