import { Component, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NzDatePickerComponent } from 'ng-zorro-antd/date-picker';
import { DrLogService } from '../../services/dr-log.service';
import { ListReq } from '../../services/list-req';

@Component({
  selector: 'app-log-query',
  templateUrl: './log-query.component.html',
  styleUrls: ['./log-query.component.less'],
})
export class LogQueryComponent {
  startValue: Date | null = new Date();
  endValue: Date | null = null;
  @ViewChild('endDatePicker') endDatePicker!: NzDatePickerComponent;

  queryForm = this.fb.group({
    appId: [''],
    message: [''],
    traceId: [''],
    startValue: [],
    endValue: [],
    exception: [''],
    elapsedStart: [''],
    elapsedEnd: [''],
    logLevel: [''],
    path: [''],
    requestBody: [''],
    responseBody: [''],
  });
  constructor(private fb: FormBuilder, private logService: DrLogService) {}

  disabledStartDate = (startValue: Date): boolean => {
    if (!startValue || !this.endValue) {
      return false;
    }
    return startValue.getTime() > this.endValue.getTime();
  };

  disabledEndDate = (endValue: Date): boolean => {
    if (!endValue || !this.startValue) {
      return false;
    }
    return endValue.getTime() <= this.startValue.getTime();
  };

  onSubmit() {
    //console.log(this.queryForm.value);
    var req = new ListReq();
    req.appId = this.queryForm.value.appId;
    req.message = this.queryForm.value.message;
    req.traceId = this.queryForm.value.traceId;
    // req.startValue = this.queryForm.value.startValue as Date;
    // req.endValue = this.queryForm.value.endValue;
    req.exception = this.queryForm.value.exception;
    req.elapsedStart = this.queryForm.value.elapsedStart;
    req.elapsedEnd = this.queryForm.value.elapsedEnd;
    req.logLevel = this.queryForm.value.logLevel;
    req.requestBody = this.queryForm.value.requestBody;
    req.responseBody = this.queryForm.value.responseBody;
    this.logService.OnQueryChange(req);
  }
}
