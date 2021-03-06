import {Component, OnInit} from '@angular/core';
import {environment} from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { finalize } from 'rxjs/operators';
import {AuthService} from '../_services/auth.service';
import {CustomCommandsPlugin} from '../_models/custom-commands-plugin';
import {AutoModPlugin} from '../_models/auto-mod-plugin';
import {Alert} from '../_models/alert';
import {PatchNote} from '../_models/patch-note';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  baseUrl = environment.apiUrl;
  isLoading: boolean;
  isLoadingPlugins: boolean;
  selectedGuildId: string;
  // @ts-ignore
  autoModPlugin: AutoModPlugin = {};
  // @ts-ignore
  customCommandsPlugin: CustomCommandsPlugin = {};
  // @ts-ignore
  guild: any = {};
  alerts: Alert[] = [];
  unseenPatchNotes: PatchNote[] = [];
  // @ts-ignore
  activePatchNote: PatchNote = {};
  isPatchNotesModalOpen = false;
  patchNotesLoaded = false;

  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit() {
    this.selectedGuildId = localStorage.getItem('selectedGuildId');
    this.getGuild();
    this.getGuildPlugins();
    this.getUnseenPatchNotes();
  }
  getUnseenPatchNotes() {
    this.http.get(this.baseUrl + 'users/patch-notes')
      .pipe()
      .subscribe(data => {
        // @ts-ignore
        this.unseenPatchNotes = data;
        this.activePatchNote = this.unseenPatchNotes[this.unseenPatchNotes.length - 1];
        this.patchNotesLoaded = true;
        this.isPatchNotesModalOpen = true;
      });
  }
  handleViewNextUnseenPatchNote() {
    this.http.post(this.baseUrl + 'users/patch-notes/' + this.activePatchNote.id, {})
      .subscribe();
    this.unseenPatchNotes.pop();
    if (this.unseenPatchNotes.length !== 0) {
      this.activePatchNote = this.unseenPatchNotes[this.unseenPatchNotes.length - 1];
    } else {
      this.isPatchNotesModalOpen = false;
    }
  }
  getGuild() {
    this.isLoading = true;
    this.http.get(this.baseUrl + 'guilds/' + localStorage.getItem('selectedGuildId'))
    .pipe(finalize(() => {
      this.isLoading = false;
    }))
    .subscribe((data) => {
      // @ts-ignore
      this.guild = data;
    }, err => {
      if (err.status === 429) {
        this.alerts.push({
          type: 'danger',
          message: 'COULD_NOT_FETCH_PLUGINS'
        });
      } else {
        this.alerts.push({
          type: 'danger',
          message: 'COULD_NOT_FETCH_GUILD'
        });
      }
    });
  }
  getGuildPlugins() {
    this.isLoadingPlugins = true;
    this.http.get(this.baseUrl + this.selectedGuildId + '/plugins')
      .pipe(finalize(() => {
        this.isLoadingPlugins = false;
      }))
      .subscribe((data) => {
        this.autoModPlugin = data['autoModPlugin'];
        this.customCommandsPlugin = data['customCommandPlugin'];
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
  getDiscordGuildWidget() {
    return `https://discordapp.com/widget?id=${(this.selectedGuildId)}&theme=dark&username=${this.authService.currentUser.nameId}`;
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
