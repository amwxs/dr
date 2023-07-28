export interface DrLog {
id: String;
  appId: string;
  hostIp: string;
  traceId: string;
  logLevel: number;
  elapsed: number;
  eventId: number;
  createTime: Date;
  message: string;
  exception: string;
}
