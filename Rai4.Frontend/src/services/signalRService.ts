import * as signalR from "@microsoft/signalr";
import type { StopDepartures } from "@/types";

class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private readonly HUB_URL = "http://localhost:5091/hubs/busstop";

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(this.HUB_URL, {
        accessTokenFactory: () => {
          return localStorage.getItem("auth_token") || "";
        },
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();
  }

  public async start(): Promise<void> {
    if (this.connection?.state === signalR.HubConnectionState.Disconnected) {
      try {
        await this.connection.start();
        console.log("SignalR Connected");
      } catch (error) {
        console.error("SignalR Connection Error:", error);
        throw error;
      }
    }
  }

  public async stop(): Promise<void> {
    if (this.connection) {
      try {
        await this.connection.stop();
        console.log("SignalR Disconnected");
      } catch (error) {
        console.error("SignalR Disconnect Error:", error);
        throw error;
      }
    }
  }

  public async trackStop(ztmStopId: number, stopName: string): Promise<void> {
    if (this.connection) {
      try {
        await this.connection.invoke("TrackStop", ztmStopId, stopName);
        console.log(`Started tracking stop: ${stopName} (${ztmStopId})`);
      } catch (error) {
        console.error("Error tracking stop:", error);
        throw error;
      }
    }
  }

  public async untrackStop(ztmStopId: number): Promise<void> {
    if (this.connection) {
      try {
        await this.connection.invoke("UntrackStop", ztmStopId);
        console.log(`Stopped tracking stop: ${ztmStopId}`);
      } catch (error) {
        console.error("Error untracking stop:", error);
        throw error;
      }
    }
  }

  public onReceiveStopUpdates(
    callback: (stopId: number, stopName: string, data: StopDepartures) => void
  ): void {
    if (this.connection) {
      this.connection.on(
        "ReceiveStopUpdatesAsync",
        (stopId: number, stopName: string, data: StopDepartures) => {
          callback(stopId, stopName, data);
        }
      );
    }
  }

  public offReceiveStopUpdates(): void {
    if (this.connection) {
      this.connection.off("ReceiveStopUpdatesAsync");
    }
  }

  public getState(): signalR.HubConnectionState | null {
    return this.connection?.state || null;
  }

  public isConnected(): boolean {
    return this.connection?.state === signalR.HubConnectionState.Connected;
  }
}

export const signalRService = new SignalRService();
