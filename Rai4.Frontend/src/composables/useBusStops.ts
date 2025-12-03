import { ref } from "vue";
import { apiService } from "@/services/apiService";
import type { FriendlyBusStop } from "@/types";

export function useBusStops() {
  const busStops = ref<FriendlyBusStop[]>([]);
  const loading = ref(false);
  const error = ref<Error | null>(null);

  const fetchBusStops = async () => {
    loading.value = true;
    error.value = null;

    try {
      busStops.value = await apiService.get<FriendlyBusStop[]>("/BusStops/all");
    } catch (err) {
      error.value = err as Error;
      console.error("Error fetching bus stops:", err);
    } finally {
      loading.value = false;
    }
  };

  return {
    busStops,
    loading,
    error,
    fetchBusStops,
  };
}
