import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DrLog } from './drlog';
import { CustResult } from 'src/app/core/cust-result';
import { DrlogTableReq } from './dr-log-table-req';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DrLogService {
  SubjectQueryChange = new Subject<DrlogTableReq>();

  constructor(private http: HttpClient) {}

  OnQueryChange(req: DrlogTableReq) {
    this.SubjectQueryChange.next(req);
  }

  list(req: DrlogTableReq) {
    const urlParams = new URLSearchParams();
    for (const [key, value] of Object.entries(req)) {
      if (value === null) {
        continue;
      }
      urlParams.append(key, value);
    }
    return this.http.get<CustResult<DrLog[]>>(
      `http://localhost:5056/Logging/query?${urlParams.toString()}`
    );
  }

  detail(id: String) {
    return this.http.get<CustResult<any>>(
      `http://localhost:5056/Logging/detail?Id=${id}`
    );
  }
}
