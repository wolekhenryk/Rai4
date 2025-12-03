// Types will be defined here
export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  email: string;
  firstName: string;
  lastName: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface Departure {
  id: string;
  delayInSeconds: number;
  estimatedTime: Date;
  headsign: string;
  routeId: number;
  routeShortName: string;
  scheduledTripStartTime: Date;
  tripId: number;
  status: string;
  theoreticalTime: Date;
  timestamp: Date;
  trip: number;
  vehicleCode: number;
  vehicleId: number;
  vehicleService: string;
}

export interface StopDepartures {
  lastUpdate: Date;
  departures: Departure[];
}

export interface FriendlyBusStop {
  stopId: number;
  stopName: string;
}
