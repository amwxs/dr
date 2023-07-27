import { Component, OnDestroy } from '@angular/core';
import { DrLog } from '../services/drlog';
import { DrLogService } from '../services/dr-log.service';
import { DrlogTableReq } from '../services/dr-log-table-req';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-log-list',
  templateUrl: './log-list.component.html',
  styleUrls: ['./log-list.component.less'],
})
export class LogListComponent implements OnDestroy {
  subscriptions = new Subscription();
  listOfData: DrLog[] = [];
  pageLoading = false;
  pageSize = 10;
  pageIndex = 1;
  total = 0;
  req: DrlogTableReq;

  constructor(private drlogService: DrLogService) {
    this.req = new DrlogTableReq();
    drlogService.SubjectQueryChange.subscribe((q) => {
      this.req = q;
      this.req.pageIndex = this.pageIndex;
      this.req.pageSize = this.pageSize;
      this.loadData(q);

      console.log(q);
    });
  }
  ngOnDestroy(): void {
    this.drlogService.SubjectQueryChange.unsubscribe();
  }

  onQueryParamsChange(params: NzTableQueryParams) {
    const { pageSize, pageIndex } = params;
    this.req.pageIndex = pageIndex;
    this.req.pageSize = pageSize;
    this.loadData(this.req);
  }

  loadData(req: DrlogTableReq) {
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
