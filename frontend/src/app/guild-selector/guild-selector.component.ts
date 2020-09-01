import {Component, OnInit, Output} from '@angular/core';
import {ClrModal} from '@clr/angular';
import {finalize} from 'rxjs/operators';
import {environment} from '../../environments/environment';
import {AuthService} from '../_services/auth.service';
import {Router} from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {Alert} from '../_models/alert';
import {EventEmitter} from '@clr/core/internal';
import {any} from 'codelyzer/util/function';

@Component({
  selector: 'app-guild-selector',
  templateUrl: './guild-selector.component.html',
  styleUrls: ['./guild-selector.component.css']
})
export class GuildSelectorComponent implements OnInit {
  baseUrl = environment.apiUrl;
  guilds: any = [];
  managedGuilds: any = [];
  isLoading: boolean;
  alerts: Alert[] = [];
  guildName: string;
  constructor(private authService: AuthService, private router: Router, private http: HttpClient) { }

  ngOnInit() {
    this.hasGuildSelected();
    this.getAvailableGuilds();
    this.getManagedGuilds();
    this.authService.currentGuildName.subscribe(m => this.guildName = m);
  }
  getAvailableGuilds() {
    this.isLoading = true;
    this.http.post(this.baseUrl + 'users/' + this.authService.decodedToken?.nameid + '/guilds',
      {token: this.authService.decodedToken.discord_access_token})
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe((data) => {
        this.guilds = data;
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_GUILDS'
          });
        }
        this.getAvailableGuilds();
      });
  }
  getManagedGuilds() {
    this.http.get(this.baseUrl + 'guilds/managed')
      .pipe(finalize(() => {
      }))
      .subscribe((data) => {
        this.managedGuilds = data;
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_MANAGED_GUILDS'
          });
        }
        this.getManagedGuilds();
      });
  }
  addToGuild() {
    window.open('https://discordapp.com/api/oauth2/authorize?client_id=606128037678809108&permissions=0&redirect_uri=https%3A%2F%2Flocalhost%3A5001%2Fauth%2Fcallback&scope=bot');
  }
  handleGuildSwitch(id: any, name: any) {
    localStorage.setItem('selectedGuildId', id);
    localStorage.setItem('selectedGuildName', name);
    this.authService.sendGuildName(name);
    this.router.navigate(['/dashboard']);
  }
  hasGuildSelected() {
    if (localStorage.getItem('selectedGuildId') === null) {
      this.alerts.unshift({
        type: 'info',
        message: 'SELECT_GUILD_FIRST'
      });
    }
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
