import { Component, OnInit } from '@angular/core';
import {environment} from '../../../../environments/environment';
import {ClrLoadingState} from '@clr/angular';
import {Alert} from '../../../_models/alert';
import {WelcomePlugin} from '../../../_models/welcome-plugin';
import {finalize} from 'rxjs/operators';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-dashboard-plugins-welcome',
  templateUrl: './dashboard-plugins-welcome.component.html',
  styleUrls: ['./dashboard-plugins-welcome.component.css']
})
export class DashboardPluginsWelcomeComponent implements OnInit {
  baseUrl: string = environment.apiUrl;
  isLoading: boolean;
  // @ts-ignore
  welcomePlugin: WelcomePlugin = {};
  btnState = ClrLoadingState.DEFAULT;
  alerts: Alert[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getWelcomePlugin();
  }
  getWelcomePlugin() {
    this.isLoading = true;
    this.http.get(this.baseUrl +
      localStorage.getItem('selectedGuildId') +
      '/plugins/welcome')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(data => {
        // @ts-ignore
        this.welcomePlugin = data;
      });
  }
  triggerWelcomePluginState() {
    this.btnState = ClrLoadingState.LOADING;
    this.welcomePlugin.isEnabled = !this.welcomePlugin.isEnabled;
    this.http.patch(this.baseUrl +
      localStorage.getItem('selectedGuildId') +
      '/plugins/welcome', { isEnabled: this.welcomePlugin.isEnabled })
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
        this.btnState = ClrLoadingState.SUCCESS;
      }, err => {
        this.welcomePlugin.isEnabled = !this.welcomePlugin.isEnabled;
        this.btnState = ClrLoadingState.ERROR;
      });
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
