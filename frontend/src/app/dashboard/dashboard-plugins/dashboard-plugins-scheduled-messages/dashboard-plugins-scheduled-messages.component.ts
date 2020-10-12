import { Component, OnInit } from '@angular/core';
import {environment} from '../../../../environments/environment';
import {ClrLoadingState} from '@clr/angular';
import {Alert} from '../../../_models/alert';
import {HttpClient} from '@angular/common/http';
import {finalize} from 'rxjs/operators';
import {ScheduledMessagesPlugin} from '../../../_models/scheduled-messages-plugin';

@Component({
  selector: 'app-dashboard-plugins-scheduled-messages',
  templateUrl: './dashboard-plugins-scheduled-messages.component.html',
  styleUrls: ['./dashboard-plugins-scheduled-messages.component.css']
})
export class DashboardPluginsScheduledMessagesComponent implements OnInit {
  baseUrl: string = environment.apiUrl;
  isLoading: boolean;
  // @ts-ignore
  scheduledMessagesPlugin: ScheduledMessagesPlugin = {};
  btnState = ClrLoadingState.DEFAULT;
  alerts: Alert[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getScheduledMessagesPlugin();
  }
  getScheduledMessagesPlugin() {
    this.isLoading = true;
    this.http.get(this.baseUrl +
      localStorage.getItem('selectedGuildId') +
      '/plugins/scheduledmessages')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(data => {
        // @ts-ignore
        this.scheduledMessagesPlugin = data;
      });
  }
  triggerScheduledMessagesPluginState() {
    this.btnState = ClrLoadingState.LOADING;
    this.scheduledMessagesPlugin.isEnabled = !this.scheduledMessagesPlugin.isEnabled;
    this.http.patch(this.baseUrl +
      localStorage.getItem('selectedGuildId') +
      '/plugins/scheduledmessages', { isEnabled: this.scheduledMessagesPlugin.isEnabled,
      scheduledMessages: this.scheduledMessagesPlugin.scheduledMessages })
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
        this.btnState = ClrLoadingState.SUCCESS;
      }, err => {
        this.scheduledMessagesPlugin.isEnabled = !this.scheduledMessagesPlugin.isEnabled;
        this.btnState = ClrLoadingState.ERROR;
      });
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
