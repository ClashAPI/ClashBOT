import { Component, OnInit } from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {finalize} from 'rxjs/operators';
import {CustomCommandsPlugin} from '../../_models/custom-commands-plugin';
import {AutoModPlugin} from '../../_models/auto-mod-plugin';
import {Alert} from '../../_models/alert';
import {ClashAPIPlugin} from '../../_models/clashapi-plugin';
import {TwitchPlugin} from '../../_models/twitch-plugin';
import {ScheduledMessagesPlugin} from '../../_models/scheduled-messages-plugin';
import {WelcomePlugin} from '../../_models/welcome-plugin';

@Component({
  selector: 'app-dashboard-plugins',
  templateUrl: './dashboard-plugins.component.html',
  styleUrls: ['./dashboard-plugins.component.css']
})
export class DashboardPluginsComponent implements OnInit {
  baseUrl = environment.apiUrl;
  isLoading: boolean;
  selectedGuildId: string;
  // @ts-ignore
  autoModPlugin: AutoModPlugin = {};
  // @ts-ignore
  customCommandsPlugin: CustomCommandsPlugin = {};
  // @ts-ignore
  clashAPIPlugin: ClashAPIPlugin = {};
  // @ts-ignore
  twitchPlugin: TwitchPlugin = {};
  // @ts-ignore
  scheduledMessagesPlugin: ScheduledMessagesPlugin = {};
  // @ts-ignore
  welcomePlugin: WelcomePlugin = {};
  alerts: Alert[] = [];

  constructor(private http: HttpClient) {
    this.selectedGuildId = localStorage.getItem('selectedGuildId');
  }

  ngOnInit() {
    this.getGuildPlugins();
  }
  getGuildPlugins() {
    this.isLoading = true;
    this.http.get(this.baseUrl + this.selectedGuildId + '/plugins')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe((data) => {
        // @ts-ignore
        this.autoModPlugin = data.autoModPlugin;
        // @ts-ignore
        this.customCommandsPlugin = data.customCommandPlugin;
        // @ts-ignore
        this.clashAPIPlugin = data.clashAPIPlugin;
        // @ts-ignore
        this.twitchPlugin = data.twitchPlugin;
        // @ts-ignore
        this.scheduledMessagesPlugin = data.scheduledMessagesPlugin;
        // @ts-ignore
        this.welcomePlugin = data.welcomePlugin;
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_PLUGINS'
          });
        }
      });
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
