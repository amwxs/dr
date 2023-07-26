import { Component, OnDestroy } from '@angular/core';
import { DrLog } from '../services/drlog';
import { DrLogService } from '../services/dr-log.service';
import { ListReq } from '../services/list-req';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { Observable, Subscribable } from 'rxjs';

@Component({
  selector: 'app-log-list',
  templateUrl: './log-list.component.html',
  styleUrls: ['./log-list.component.less'],
})
export class LogListComponent implements OnDestroy {
  listOfData: DrLog[] = [];
  pageLoading = false;
  pageSize = 10;
  pageIndex = 1;
  total = 0;

  req: ListReq;

  constructor(private drlogService: DrLogService) {
    this.req = new ListReq();
    drlogService.SubjectQueryChange.subscribe((q) => {
      this.req = q;
      this.req.pageIndex = this.pageIndex;
      this.req.pageSize = this.pageSize;
      this.loadData(q);

      console.log(q);
    });
  }
  ngOnDestroy(): void {}

  onQueryParamsChange(params: NzTableQueryParams) {
    const { pageSize, pageIndex } = params;
    this.req.pageIndex = pageIndex;
    this.req.pageSize = pageSize;
    this.loadData(this.req);
  }

  loadData(req: ListReq) {
    this.pageLoading = true;
    this.drlogService.list(req).subscribe((s) => {
      this.listOfData = s.data;
      this.pageSize = s.pager.pageSize;
      this.pageIndex = s.pager.pageIndex;
      this.total = s.pager.total;
      this.pageLoading = false;
    });
  }
}
