import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { DrLogService } from '../../services/dr-log.service';
import { DrlogTableReq } from '../../services/dr-log-table-req';

@Component({
  selector: 'app-log-query',
  templateUrl: './log-query.component.html',
  styleUrls: ['./log-query.component.less'],
})
export class LogQueryComponent {
  queryForm = this.fb.group({
    appId: [''],
    message: [''],
    traceId: [''],
    startValue: [],
    endValue: [null],
    exception: [''],
    elapsedStart: [null],
    elapsedEnd: [null],
    logLevel: [-999],
    path: [''],
    requestBody: [''],
    responseBody: [''],
  });
  constructor(private fb: FormBuilder, private logService: DrLogService) {}

  disabledStartDate = (startValue: Date): boolean => {
    if (!startValue || !this.queryForm.value.endValue) {
      return false;
    }
    return (
      startValue.getTime() > (this.queryForm.value.endValue as Date).getTime()
    );
  };

  disabledEndDate = (endValue: Date): boolean => {
    if (!endValue || !this.queryForm.value.startValue) {
      return false;
    }
    return (
      endValue.getTime() <= (this.queryForm.value.startValue as Date).getTime()
    );
  };

  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
  }

  onSubmit() {
    var req = new DrlogTableReq();

    req.appId = this.queryForm.value.appId?.trim();
    req.traceId = this.queryForm.value.traceId?.trim();
    req.message = this.queryForm.value.message?.trim();

    if (this.queryForm.value.startValue !== null) {
      req.startTime = this.formatDate(
        this.queryForm.value.startValue as unknown as Date
      );
    }

    if (this.queryForm.value.endValue !== null) {
      req.endTime = this.formatDate(
        this.queryForm.value.endValue as unknown as Date
      );
    }
    req.exception = this.queryForm.value.exception?.trim();
    req.logLevel = this.queryForm.value.logLevel;
    req.elapsedRangeStart = this.queryForm.value.elapsedStart;
    req.elapsedRangeEnd = this.queryForm.value.elapsedEnd;
    req.requestPath = this.queryForm.value.path?.trim();
    req.requestBody = this.queryForm.value.requestBody?.trim();
    req.responseBody = this.queryForm.value.responseBody?.trim();
    this.logService.OnQueryChange(req);
  }
}
