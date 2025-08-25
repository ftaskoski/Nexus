<template>
  <div class="relative overflow-hidden bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-100 p-4 shadow-sm transition-all hover:shadow-md">
    <div class="mb-3">
      <p class="text-sm font-medium text-gray-900 truncate">
        <slot name="header" />
      </p>
    </div>

    <div class="flex items-center justify-between">
      <slot name="content" />
    </div>
  </div>
</template>
