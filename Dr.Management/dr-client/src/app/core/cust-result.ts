export interface CustResult<T> {
  code: string | '';
  message: string | '';
  data: T;
  pager: Pager;
}

export interface Pager {
  pageIndex: number;
  pageSize: number;
  total: number;
  HasPreviousPage: boolean;
  HasNextPage: boolean;
}
