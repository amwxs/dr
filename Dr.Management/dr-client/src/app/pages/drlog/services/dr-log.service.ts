import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DrLog } from './drlog';
import { CustResult } from 'src/app/core/cust-result';
import { ListReq } from './list-req';
import { filter } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DrLogService {
  constructor(private http: HttpClient) {}
  list(req: ListReq) {
    return this.http.get<CustResult<DrLog[]>>(
      `http://localhost:5056/Logging/query?pageIndex=${req.pageIndex}&pageSize=${req.pageSize}`
    );
  }
}
