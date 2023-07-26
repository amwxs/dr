import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DrLog } from './drlog';
import { CustResult } from 'src/app/core/cust-result';
import { ListReq } from './list-req';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DrLogService {
  SubjectQueryChange = new Subject<ListReq>();

  constructor(private http: HttpClient) {}

  OnQueryChange(req: ListReq) {
    this.SubjectQueryChange.next(req);
  }

  list(req: ListReq) {
    return this.http.get<CustResult<DrLog[]>>(
      `http://localhost:5056/Logging/query?pageIndex=${req.pageIndex}&pageSize=${req.pageSize}`
    );
  }
}
