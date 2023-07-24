import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { DrlogRoutingModule } from './drlog-routing.module';
import { LogListComponent } from './log-list/log-list.component';
import { HttpClientModule } from '@angular/common/http';
import { LogQueryComponent } from './log-list/log-query/log-query.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzButtonModule } from 'ng-zorro-antd/button';
@NgModule({
  declarations: [LogListComponent, LogQueryComponent],
  imports: [
    CommonModule,
    DrlogRoutingModule,
    NzTableModule,
    NzDividerModule,
    HttpClientModule,
    ReactiveFormsModule,
    NzFormModule,
    NzButtonModule,
  ],
})
export class DrlogModule {}
