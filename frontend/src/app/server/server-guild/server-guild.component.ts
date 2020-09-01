import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {finalize} from 'rxjs/operators';
import {AuthService} from '../../_services/auth.service';
import {ClrLoadingState} from '@clr/angular';
import {Guild} from '../../_models/guild';
import {Alert} from '../../_models/alert';
import {User} from '../../_models/user';

@Component({
  selector: 'app-server-guild',
  templateUrl: './server-guild.component.html',
  styleUrls: ['./server-guild.component.css']
})
export class ServerGuildComponent implements OnInit {
  baseUrl = environment.apiUrl;
  guild: any = {};
  // @ts-ignore
  dbGuild: Guild = {};
  isLoading: boolean;
  isLoadingDbGuild = false;
  isLoadingCandidates = false;
  isManagersInitialized = false;
  isOpen = false;
  model: any = {managers: []};
  regions: any = [];
  selectedGuildId: string;
  selectedUsersToAdd: any = [];
  selectedUsersToRemove: any = [];
  managerCandidates: any = [];
  alerts: Alert[] = [];
  isDbGuildInitialized = false;
  btnState = ClrLoadingState.DEFAULT;
  managersBtnState = ClrLoadingState.DEFAULT;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.selectedGuildId = localStorage.getItem('selectedGuildId');
  }

  ngOnInit() {
    this.getGuild();
    this.getRegions();
    this.getDbGuild();
  }
  getRegions() {
    this.isLoading = true;
    this.http.get(this.baseUrl + 'guilds/regions')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe((data) => {
        this.regions = data;
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_REGIONS'
          });
        }
      });
  }
  getGuild() {
    this.http.get(this.baseUrl + 'guilds/' + this.selectedGuildId)
      .pipe(finalize(() => {
      }))
      .subscribe((data) => {
        this.guild = data;
        console.log(this.guild);
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_GUILD'
          });
        }
      });
  }
  getDbGuild() {
    if (!this.isDbGuildInitialized) {
      this.isLoadingDbGuild = true;
      this.managersBtnState = ClrLoadingState.LOADING;
      this.http.get(this.baseUrl + 'users/' + this.authService.currentUser.id + '/guilds/' + this.selectedGuildId)
        .pipe(finalize(() => {
        }))
        .subscribe((data) => {
          // @ts-ignore
          this.dbGuild = data;
          this.isDbGuildInitialized = true;
          this.isLoadingDbGuild = false;
          this.managersBtnState = ClrLoadingState.DEFAULT;
        }, err => {
          if (err.status === 429) {
            this.alerts.push({
              type: 'danger',
              message: 'RATE_LIMIT_EXCEEDED'
            });
          } else {
            this.alerts.push({
              type: 'danger',
              message: 'COULD_NOT_FETCH_GUILD'
            });
          }
          this.managersBtnState = ClrLoadingState.ERROR;
        });
    }
  }
  getManagerCandidates() {
    if (!this.isManagersInitialized) {
      this.isLoadingCandidates = true;
      const managerIds = this.dbGuild.managers.map(m => {
        // @ts-ignore
        return `${m.userName}#${m.discriminator}`;
      });
      this.guild.members.forEach(m => {
        if (!managerIds.includes(`${m.username}#${m.discriminator}`)) {
          this.managerCandidates.push(m);
        }
      });
      this.isLoadingCandidates = false;
      this.isManagersInitialized = true;
    }
  }
  updateGuild() {
    this.btnState = ClrLoadingState.LOADING;
    this.model.id = localStorage.getItem('selectedGuildId');
    this.model.name = this.guild.name;
    this.model.region = this.guild.region;
    this.model.managers = this.dbGuild.managers;
    this.model.verificationLevel = this.guild.verification_level;
    this.model.explicitContentFilter = this.guild.explicit_content_filter;
    this.model.defaultMessageNotifications = this.guild.default_message_notifications;
    console.log(this.model);
    this.http.patch(this.baseUrl + 'guilds', this.model).subscribe(() => {
      this.btnState = ClrLoadingState.SUCCESS;
      this.alerts.push({
        type: 'success',
        message: 'GUILD_UPDATE_SUCCESS'
      });
    }, err => {
      this.btnState = ClrLoadingState.ERROR;
      if (err.status === 429) {
        this.alerts.push({
          type: 'success',
          message: 'GUILD_UPDATE_SUCCESS'
        });
      } else {
        this.alerts.push({
          type: 'success',
          message: 'COULD_NOT_UPDATE_GUILD'
        });
      }
    });
  }
  handleAddToManagers() {
    this.selectedUsersToAdd.forEach(u => {
      console.log(u);
      // @ts-ignore
      this.dbGuild.managers.push({
        userName: u.username,
        discriminator: u.discriminator
      } as User);
      this.managerCandidates.splice(this.managerCandidates.indexOf(u), 1);
    });
  }
  handleRemoveFromManagers() {
    // console.log(this.selectedUsersToRemove);
    this.selectedUsersToRemove.forEach(m => {
      this.dbGuild.managers.splice(this.dbGuild.managers.indexOf(m), 1);
    });
  }
  handleSubmitManagers() {
    //
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
