import { Component, OnInit } from '@angular/core';
import {finalize} from 'rxjs/operators';
import {Router} from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';

@Component({
  selector: 'app-admin-bot',
  templateUrl: './admin-bot.component.html',
  styleUrls: ['./admin-bot.component.css']
})
export class AdminBotComponent implements OnInit {
  baseUrl = environment.apiUrl;
  isLoading: boolean;
  ping: any;
  isBotOnline: any;
  constructor(router: Router, private http: HttpClient) { }

  ngOnInit() {
    this.getPing();
    this.getIfBotOnline();
  }
  connectBot() {
    this.isLoading = true;
    this.http.post(this.baseUrl + 'bot/connect', {})
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(() => {
        this.isBotOnline = !this.isBotOnline;
      });
  }
  reconnectBot() {
    this.isLoading = true;
    this.http.post(this.baseUrl + 'bot/reconnect', {})
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(() => {
        this.isBotOnline = !this.isBotOnline;
      });
  }
  disconnectBot() {
    this.isLoading = true;
    this.http.post(this.baseUrl + 'bot/disconnect', {})
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(() => {
        this.isBotOnline = !this.isBotOnline;
      });
  }
  getPing() {
    this.isLoading = true;
    this.http.get(this.baseUrl + 'bot/ping')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe((data) => {
        this.ping = data;
        console.log('Ping: ' + this.ping);
      });
  }
  getIfBotOnline() {
    this.isLoading = true;
    this.http.get(this.baseUrl + 'bot/is-online')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe((data) => {
        this.isBotOnline = data;
        console.log('isOnline: ' + this.isBotOnline);
      });
  }
}
