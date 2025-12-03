<template>
  <div class="bg-white rounded-lg shadow-md overflow-hidden flex flex-col h-96">
    <div
      class="bg-blue-500 text-white px-4 py-3 flex-shrink-0 flex items-center justify-between"
    >
      <h3 class="text-lg font-semibold">{{ stopName }}</h3>
      <button
        @click="unsubscripbe()"
        class="bg-red-500 px-2 py-1 rounded text-sm hover:bg-red-600 transition-colors"
        aria-label="Unsubscribe"
      >
        Usuń
      </button>
    </div>
    <div class="p-2 overflow-y-auto flex-1">
      <slot :departures="departures"></slot>
    </div>
    <div class="px-4 py-3 bg-gray-50 border-t border-gray-200 flex-shrink-0">
      <p class="text-sm text-gray-500">
        Aktualizacja: {{ new Date(lastUpdate).toLocaleTimeString() }}
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Departure } from "@/types";
import { signalRService } from "@/services/signalRService";

interface Props {
  stopId: number;
  stopName: string;
  departures: Departure[];
  lastUpdate: Date;
}

const props = defineProps<Props>();

const emit = defineEmits<{
  untrack: [stopId: number]
}>();

const unsubscripbe = async () => {
  await signalRService.untrackStop(props.stopId);
  emit('untrack', props.stopId);
};
</script>
