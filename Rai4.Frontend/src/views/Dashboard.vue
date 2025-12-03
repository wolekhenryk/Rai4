<template>
  <div class="min-h-screen bg-gray-100">
    <nav class="bg-white shadow-md">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between h-16">
          <div class="flex items-center">
            <h1 class="text-xl font-bold">Rai4.Frontend</h1>
          </div>
          <div class="flex items-center">
            <button
              @click="handleLogout"
              class="bg-red-500 text-white px-4 py-2 rounded-md hover:bg-red-600 transition-colors"
            >
              Wyloguj się
            </button>
          </div>
        </div>
      </div>
    </nav>

    <main class="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
      <div class="px-4 py-6 sm:px-0">
        <div class="bg-white rounded-lg shadow p-6">
          <h2 class="text-2xl font-bold mb-4">Śledzenie przystanków</h2>

          <div class="mb-6">
            <label
              for="busStopSelect"
              class="block text-sm font-medium text-gray-700 mb-2"
            >
              Wybierz przystanek do śledzenia:
            </label>
            <div class="flex gap-2">
              <select
                id="busStopSelect"
                v-model="selectedStopId"
                class="flex-1 px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option :value="null">-- Wybierz przystanek --</option>
                <option
                  v-for="stop in busStops"
                  :key="stop.stopId"
                  :value="stop.stopId"
                >
                  {{ stop.stopName }}
                </option>
              </select>
              <button
                @click="handleStartTracking"
                :disabled="selectedStopId === null"
                class="px-6 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors disabled:bg-gray-300 disabled:cursor-not-allowed"
              >
                Subskrybuj
              </button>
            </div>
          </div>
        </div>

        <div v-if="stopDeparturesMap.size > 0" class="mt-6">
          <h3 class="text-xl font-semibold mb-4">Twoje przystanki</h3>
          <DepartureGrid>
            <DepartureCard
              v-for="[stopId, stopData] in stopDeparturesMap"
              :key="stopId"
              :stopId="stopId"
              :stop-name="stopData.stopName"
              :departures="stopData.departures.departures"
              :last-update="stopData.departures.lastUpdate"
              :class="{ 'card-flash': animatingCards.has(stopId) }"
              @untrack="handleUntrack"
            >
              <template #default="{ departures }">
                <div
                  v-for="dep in departures"
                  :key="dep.id"
                  class="mb-2 p-2 last:border-0"
                >
                  <div class="flex justify-between items-center">
                    <div>
                      <p class="font-semibold text-lg">
                        Linia {{ dep.routeShortName }}
                      </p>
                      <p class="text-gray-600">{{ dep.headsign }}</p>
                    </div>
                    <div class="text-right">
                      <p
                        class="font-medium text-lg"
                        v-delay-color="dep.delayInSeconds"
                      >
                        {{ new Date(dep.estimatedTime).toLocaleTimeString() }}
                      </p>
                      <p
                        v-if="dep.delayInSeconds > 60"
                        class="text-xs"
                        v-delay-color="dep.delayInSeconds"
                      >
                        +{{ Math.floor(dep.delayInSeconds / 60) }} min.
                      </p>
                      <p
                        v-else-if="dep.delayInSeconds < -60"
                        class="text-xs"
                        v-delay-color="dep.delayInSeconds"
                      >
                        -{{ Math.floor(Math.abs(dep.delayInSeconds) / 60) }}
                        min.
                      </p>
                      <p
                        v-else
                        class="text-xs"
                        v-delay-color="dep.delayInSeconds"
                      >
                        Punktualnie
                      </p>
                    </div>
                  </div>
                </div>
              </template>
            </DepartureCard>
          </DepartureGrid>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, ref } from "vue";
import { apiService } from "@/services/apiService";
import { signalRService } from "@/services/signalRService";
import type { StopDepartures } from "@/types";
import DepartureCard from "@/components/DepartureCard.vue";
import DepartureGrid from "@/components/DepartureGrid.vue";
import { useBusStops } from "@/composables/useBusStops";
import { vDelayColor } from "@/directives/delayColor";

interface StopData {
  stopName: string;
  departures: StopDepartures;
}

const { busStops, fetchBusStops } = useBusStops();
const stopDepartures = ref<StopDepartures | null>(null);
const selectedStopId = ref<number | null>(null);
const stopDeparturesMap = ref<Map<number, StopData>>(new Map());
const trackedStops = ref<Set<number>>(new Set());
const animatingCards = ref<Set<number>>(new Set());

onMounted(async () => {
  await fetchBusStops();

  await signalRService.start();

  signalRService.onReceiveStopUpdates(
    (stopId: number, stopName: string, data: StopDepartures) => {
      const existingData = stopDeparturesMap.value.get(stopId);
      const isNewUpdate =
        !existingData ||
        new Date(existingData.departures.lastUpdate).getTime() !==
          new Date(data.lastUpdate).getTime();

      stopDeparturesMap.value.set(stopId, {
        stopName,
        departures: data,
      });

      if (isNewUpdate && existingData) {
        animatingCards.value.add(stopId);

        setTimeout(() => {
          animatingCards.value.delete(stopId);
        }, 1000);
      }

      stopDepartures.value = data;
    }
  );
});

onUnmounted(async () => {
  signalRService.offReceiveStopUpdates();
  await signalRService.stop();
});

const handleStartTracking = async () => {
  if (selectedStopId.value !== null) {
    const selectedStop = busStops.value.find(
      (stop) => stop.stopId === selectedStopId.value
    );

    if (selectedStop) {
      try {
        await signalRService.trackStop(
          selectedStop.stopId,
          selectedStop.stopName
        );
        trackedStops.value.add(selectedStop.stopId);
      } catch (error) {
        console.error("Error tracking stop:", error);
      }
    }
  }
};

const handleUntrack = (stopId: number) => {
  stopDeparturesMap.value.delete(stopId);
  trackedStops.value.delete(stopId);
  animatingCards.value.delete(stopId);
};

const handleLogout = () => {
  apiService.logout();
};
</script>

<style scoped>
@keyframes flash {
  0% {
    transform: scale(1);
    box-shadow: 0 0 0 0 rgba(59, 130, 246, 0.7);
  }
  50% {
    transform: scale(1.02);
    box-shadow: 0 0 20px 10px rgba(59, 130, 246, 0.4);
  }
  100% {
    transform: scale(1);
    box-shadow: 0 0 0 0 rgba(59, 130, 246, 0);
  }
}

.card-flash {
  animation: flash 1s ease-in-out;
}

/* Delay color classes */
:deep(.delay-very-late) {
  color: #dc2626 !important; /* red-600 - very late (>5 min) */
  font-weight: 700;
}

:deep(.delay-late) {
  color: #f97316 !important; /* orange-500 - late (>1 min) */
  font-weight: 600;
}

:deep(.delay-early) {
  color: #3b82f6 !important; /* blue-500 - early */
}

:deep(.delay-on-time) {
  color: #22c55e !important; /* green-500 - on time */
}
</style>
