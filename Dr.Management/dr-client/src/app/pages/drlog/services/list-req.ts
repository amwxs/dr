export class ListReq {
  appId: string | null | undefined;
  pageSize: number | undefined;
  pageIndex: number | undefined;
  message: string | null | undefined;
  traceId: string | null | undefined;
  startValue: Date | null | undefined;
  endValue: Date | null | undefined;
  exception: string | null | undefined;
  elapsedStart: string | null | undefined;
  elapsedEnd: string | null | undefined;
  logLevel: string | null | undefined;
  requestBody: string | null | undefined;
  responseBody: string | null | undefined;
}
