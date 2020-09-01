import { Component, OnInit } from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {finalize} from 'rxjs/operators';

@Component({
  selector: 'app-admin-log',
  templateUrl: './admin-log.component.html',
  styleUrls: ['./admin-log.component.css']
})
export class AdminLogComponent implements OnInit {
  baseUrl = environment.apiUrl;
  log: any = [];
  isLoading: boolean;
  alerts = [];
  // guildName: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getLog();
  }
  getLog() {
    this.isLoading = true;
    this.http.get(this.baseUrl + 'logs')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(data => {
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
  /*
  getGuildDetails(id: any) {
    this.http.get(this.baseUrl + 'guilds/' + id)
      .pipe(finalize(() => {
      }))
      .subscribe((data) => {
        this.guildName = data.name;
      });
  }
  */
}
