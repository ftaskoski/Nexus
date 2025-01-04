<template>
  <div class="min-h-screen bg-gray-50 flex items-center justify-center p-4 sm:p-6 lg:p-8">
    <div class="max-w-md w-full bg-white rounded-2xl shadow-lg p-8">
      <div v-if="$slots.header">
        <slot name="header"></slot>
      </div>
      <div>
        <slot name="content"></slot>
      </div>
      <div v-if="$slots.footer">
        <slot name="footer"></slot>
      </div>
    </div>
  </div>
</template>


