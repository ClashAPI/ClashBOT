import { Component, OnInit } from '@angular/core';
import {environment} from '../../../../environments/environment';
import {TwitchPlugin} from '../../../_models/twitch-plugin';
import {ClrLoadingState} from '@clr/angular';
import {Alert} from '../../../_models/alert';
import {HttpClient} from '@angular/common/http';
import {finalize} from 'rxjs/operators';
import {ClashAPIPlugin} from '../../../_models/clashapi-plugin';

@Component({
  selector: 'app-dashboard-plugins-clashapi',
  templateUrl: './dashboard-plugins-clashapi.component.html',
  styleUrls: ['./dashboard-plugins-clashapi.component.css']
})
export class DashboardPluginsClashapiComponent implements OnInit {
  baseUrl: string = environment.apiUrl;
  isLoading: boolean;
  // @ts-ignore
  clashAPIPlugin: ClashAPIPlugin = {};
  btnState = ClrLoadingState.DEFAULT;
  alerts: Alert[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getClashAPIPlugin();
  }
  getClashAPIPlugin() {
    this.isLoading = true;
    this.http.get(this.baseUrl +
      localStorage.getItem('selectedGuildId') +
      '/plugins/clashapi')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(data => {
        // @ts-ignore
        this.clashAPIPlugin = data;
      });
  }
  triggerClashAPIPluginState() {
    this.btnState = ClrLoadingState.LOADING;
    this.clashAPIPlugin.isEnabled = !this.clashAPIPlugin.isEnabled;
    this.http.patch(this.baseUrl +
      localStorage.getItem('selectedGuildId') +
      '/plugins/clashapi', { isEnabled: this.clashAPIPlugin.isEnabled })
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
        this.btnState = ClrLoadingState.SUCCESS;
      }, err => {
        this.clashAPIPlugin.isEnabled = !this.clashAPIPlugin.isEnabled;
        this.btnState = ClrLoadingState.ERROR;
      });
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
