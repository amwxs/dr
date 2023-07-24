import { Component } from '@angular/core';
import { DrLog } from '../services/drlog';
import { DrLogService } from '../services/dr-log.service';
import { ListReq } from '../services/list-req';
import { NzTableQueryParams } from 'ng-zorro-antd/table';

@Component({
  selector: 'app-log-list',
  templateUrl: './log-list.component.html',
  styleUrls: ['./log-list.component.less'],
})
export class LogListComponent {
  listOfData: DrLog[] = [];
  pageLoading = false;
  pageSize = 10;
  pageIndex = 1;
  total = 0;
  constructor(private drlogService: DrLogService) {
    this.loadData(new ListReq());
  }
  onQueryParamsChange(params: NzTableQueryParams) {
    const { pageSize, pageIndex } = params;

    var req = new ListReq();
    req.pageIndex = pageIndex;
    req.pageSize = pageSize;

    this.loadData(req);
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
