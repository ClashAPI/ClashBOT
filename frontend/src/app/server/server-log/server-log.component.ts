import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {finalize} from 'rxjs/operators';
import {ClrLoadingState} from '@clr/angular';
import {Alert} from '../../_models/alert';
import {LogEntry} from '../../_models/log-entry';

@Component({
  selector: 'app-server-log',
  templateUrl: './server-log.component.html',
  styleUrls: ['./server-log.component.css']
})
export class ServerLogComponent implements OnInit {
  baseUrl = environment.apiUrl;
  log: LogEntry[] = [];
  isLoading: boolean;
  isOpen = false;
  alerts: Alert[] = [];
  btnState = ClrLoadingState.DEFAULT;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getLog();
  }
  getLog() {
    this.isLoading = true;
    this.http.get(this.baseUrl + 'logs/guild/' + localStorage.getItem('selectedGuildId'))
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(data => {
        // @ts-ignore
        this.log = data;
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_LOG'
          });
        }
      });
  }
  deleteLogEntries() {
    this.btnState = ClrLoadingState.LOADING;
    this.isLoading = true;
    this.http.delete(this.baseUrl + 'logs/guild/' + localStorage.getItem('selectedGuildId'))
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(response => {
        this.btnState = ClrLoadingState.SUCCESS;
        // @ts-ignore
        this.log = response;
        this.alerts.push({
          type: 'success',
          message: 'LOG_DELETE_SUCCESS'
        });
      }, err => {
        this.btnState = ClrLoadingState.ERROR;
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_DELETE_LOG'
          });
        }
      });
  }
  deleteLogEntry(id: any, index: any) {
    this.isLoading = true;
    this.http.delete(this.baseUrl + 'logs/' + id)
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(response => {
        this.log.splice(index, 1);
        this.alerts.push({
          type: 'success',
          message: 'LOG_ENTRY_DELETE_SUCCESS'
        });
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_DELETE_LOG_ENTRY'
          });
        }
      });
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
