import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {finalize} from 'rxjs/operators';
import {TwitchPlugin} from '../../../_models/twitch-plugin';
import {environment} from 'src/environments/environment';
import {Alert} from '../../../_models/alert';
import {ClrLoadingState} from '@clr/angular';

@Component({
  selector: 'app-dashboard-plugins-twitch',
  templateUrl: './dashboard-plugins-twitch.component.html',
  styleUrls: ['./dashboard-plugins-twitch.component.css']
})
export class DashboardPluginsTwitchComponent implements OnInit {
  baseUrl: string = environment.apiUrl;
  isLoading: boolean;
  // @ts-ignore
  twitchPlugin: TwitchPlugin = {};
  btnState = ClrLoadingState.DEFAULT;
  alerts: Alert[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getTwitchPlugin();
  }
  getTwitchPlugin() {
    this.isLoading = true;
    this.http.get(this.baseUrl +
      localStorage.getItem('selectedGuildId') +
      '/plugins/twitch')
      .pipe(finalize(() => {
        this.isLoading = false;
    }))
      .subscribe(data => {
        // @ts-ignore
        this.twitchPlugin = data;
      });
  }
  triggerTwitchPluginState() {
    this.btnState = ClrLoadingState.LOADING;
    this.twitchPlugin.isEnabled = !this.twitchPlugin.isEnabled;
    this.http.patch(this.baseUrl +
      localStorage.getItem('selectedGuildId') +
      '/plugins/twitch', { isEnabled: this.twitchPlugin.isEnabled,
      twitchChannelSubscriptions: this.twitchPlugin.twitchChannelSubscriptions })
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
        this.btnState = ClrLoadingState.SUCCESS;
      }, err => {
        this.twitchPlugin.isEnabled = !this.twitchPlugin.isEnabled;
        this.btnState = ClrLoadingState.ERROR;
      });
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
