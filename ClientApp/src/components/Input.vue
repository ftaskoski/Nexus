<script setup lang="ts">
import { ref, watch } from "vue";
import Icon from "./Icon.vue";

interface Props {
  id: string;
  type?: string;
  modelValue: string;
  placeholder?: string;
  label?: string;
  icon?: string; 
  iconColor?: string;
  iconFill?: string;
  required?: boolean;
  fillRule?:string;

}

const props = defineProps<Props>();

const emits = defineEmits(["update:modelValue"]);

const localValue = ref(props.modelValue);

watch(() => props.modelValue, (newVal) => {
  localValue.value = newVal;
});

const updateValue = (event: Event) => {
  const value = (event.target as HTMLInputElement).value;
  localValue.value = value;
  emits('update:modelValue', value);
};
</script>

<template>
  <div>
    <label :for="id" class="block text-sm font-medium text-gray-700">
      <slot name="label">{{ label }}</slot>
    </label>
    <div class="relative">
      <div v-if="icon" class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
        <Icon :icon="icon" :color="iconColor" :fill-rule="fillRule" :fill="iconFill"  class="mt-1" />
      </div>
      <input
        :id="id"
        :type="type"
        v-model="localValue"
        @input="updateValue"
        :placeholder="placeholder"
        :required="required"
        :class="[
          'block w-full py-3 border-0 ring-1 ring-inset ring-gray-200 rounded-lg focus:ring-2 focus:ring-blue-500 bg-gray-50/50 text-gray-900 placeholder:text-gray-400 sm:text-sm',
          icon ? 'pl-10 pr-3' : 'px-3' 
        ]"
      />
    </div>
  </div>
</template>