export class DrlogTableReq {
  appId: string | null | undefined;
  pageSize: number | undefined;
  pageIndex: number | undefined;
  message: string | null | undefined;
  traceId: string | null | undefined;
  startTime: string | null | undefined;
  endTime: string | null | undefined;
  exception: string | null | undefined;
  elapsedRangeStart: number | null | undefined;
  elapsedRangeEnd: number | null | undefined;
  logLevel: number | null | undefined;
  requestBody: string | null | undefined;
  responseBody: string | null | undefined;
  requestPath: string | null | undefined;
}
