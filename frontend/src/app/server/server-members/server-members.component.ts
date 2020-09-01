import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {finalize} from 'rxjs/operators';
import {ClrLoadingState, ClrModal} from '@clr/angular';
import * as _ from 'underscore';
import {Ban} from '../../_models/ban';
import {TemporaryBan} from '../../_models/temporary-ban';
import {Alert} from '../../_models/alert';

@Component({
  selector: 'app-server-members',
  templateUrl: './server-members.component.html',
  styleUrls: ['./server-members.component.css']
})
export class ServerMembersComponent implements OnInit {
  baseUrl = environment.apiUrl;
  index: number;
  members: any = [];
  member: any = {};
  isLoading: boolean;
  isLoadingBanned: boolean;
  isLoadingTempBanned: boolean;
  isBannedInitialized = false;
  isTempBannedInitialized = false;
  model: any = {};
  showSubAlert = false;
  subAlertType = '';
  rolesToDelete: any = [];
  // newMember: any = {};
  newMemberRoles: any = [];
  membersToUnban = [];
  membersToTempUnban = [];
  selectedGuildId: string;
  alerts: Alert[] = [];
  bans: Ban[] = [];
  tempBans: TemporaryBan[] = [];
  isBannedUsersModalOpen = false;
  isTempBannedUsersModalOpen = false;
  selectedMembersToUnban: any = [];
  selectedMembersToTempUnban: any = [];
  btnState: ClrLoadingState;
  unbanBtnState: ClrLoadingState;
  tempUnbanBtnState: ClrLoadingState;
  tempbanMemberBtnState: ClrLoadingState;
  tempbanUntil: any;
  currentDate = new Date().toISOString();

  constructor(private http: HttpClient) {
    this.selectedGuildId = localStorage.getItem('selectedGuildId');
  }

  ngOnInit() {
    this.getMembers();
  }
  /*
  getMember(userId: any) {
    this.isLoading = true;
    this.http.get(this.baseUrl + this.selectedGuildId + '/members/' + userId)
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe((data) => {
        this.newMember = data;
      });
  }
  */
  handleUnbanMembers() {
    this.selectedMembersToUnban.forEach(b => {
      this.bans.splice(this.selectedMembersToUnban.indexOf(b), 1);
    });
  }
  handleSubmitUnbans() {
    this.unbanBtnState = ClrLoadingState.LOADING;
    this.selectedMembersToUnban.forEach(m => {
      this.membersToUnban.push(
        {username: m.user.username, discriminator: m.user.discriminator}
      );
    });
    console.log(this.membersToUnban);
    this.http.post(this.baseUrl + this.selectedGuildId + '/members/unban', {members: this.membersToUnban} )
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
        this.unbanBtnState = ClrLoadingState.SUCCESS;
    }, err => {
        this.unbanBtnState = ClrLoadingState.ERROR;
      });
  }
  handleTempbanMember(modal: ClrModal, member: any, index: number) {
    this.tempbanMemberBtnState = ClrLoadingState.LOADING;
    this.http.post(this.baseUrl + 'guilds/' + this.selectedGuildId + '/tempbans', [{memberId: member.id, username: member.username,
      discriminator: member.discriminator, expiresAt: this.tempbanUntil}])
      .pipe(finalize(() => {
      }))
      .subscribe(data => {
        this.members.splice(index, 1);
        this.alerts.push({
          type: 'success',
          message: 'MEMBER_BAN_SUCCESS'
        });
        this.tempbanMemberBtnState = ClrLoadingState.SUCCESS;
        modal.close();
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_BAN_MEMBER'
          });
        }
        this.tempbanMemberBtnState = ClrLoadingState.ERROR;
      });
  }
  handleTempUnbanMembers() {
    this.selectedMembersToTempUnban.forEach(b => {
      this.tempBans.splice(this.selectedMembersToTempUnban.indexOf(b), 1);
    });
  }
  handleSubmitTempUnbans() {
    this.tempUnbanBtnState = ClrLoadingState.LOADING;
    this.selectedMembersToTempUnban.forEach(m => {
      this.membersToTempUnban.push(
        {username: m.user.username, discriminator: m.user.discriminator, expiresAt: m.user.expiresAt}
      );
    });
    console.log(this.membersToTempUnban);
    this.http.post(this.baseUrl + 'guilds/' + this.selectedGuildId + '/tempbans', this.membersToTempUnban)
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
        this.tempUnbanBtnState = ClrLoadingState.SUCCESS;
      }, err => {
        this.tempUnbanBtnState = ClrLoadingState.ERROR;
      });
  }
  getMemberRoles(userId: any) {
    this.isLoading = true;
    this.http.get(this.baseUrl + this.selectedGuildId + '/members/' + userId + '/roles')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe((data) => {
        this.newMemberRoles = data;
        this.model.roles = this.newMemberRoles;
        this.presentSubAlert('success');
      }, err => {
        this.presentSubAlert('danger');
      });
  }
  getMembers() {
    this.isLoading = true;
    this.http.get(this.baseUrl + this.selectedGuildId + '/members')
      .pipe(finalize(() => {
        console.log(this.members);
        this.isLoading = false;
      }))
      .subscribe((data) => {
        this.members = data;
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_MEMBERS'
          });
        }
      });
  }
  getBans() {
    if (!this.isBannedInitialized) {
      this.isLoadingBanned = true;
      this.http.get(this.baseUrl + 'guilds/' + this.selectedGuildId + '/bans')
        .pipe(finalize(() => {
          this.isLoadingBanned = false;
        }))
        .subscribe(data => {
          // @ts-ignore
          this.bans = data;
          console.log(this.bans);
          this.isBannedInitialized = true;
        }, err => {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_MEMBERS'
          });
        });
    }
  }
  getTempBans() {
    if (!this.isTempBannedInitialized) {
      this.isLoadingTempBanned = true;
      this.http.get(this.baseUrl + 'guilds/' + this.selectedGuildId + '/tempbans')
        .pipe(finalize(() => {
          this.isLoadingTempBanned = false;
        }))
        .subscribe(data => {
          // @ts-ignore
          this.tempBans = data;
          console.log(this.tempBans);
          this.isTempBannedInitialized = true;
        }, err => {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_MEMBERS'
          });
        });
    }
  }
  editMember() {
    this.isLoading = true;
    this.http.patch(this.baseUrl + localStorage.getItem('selectedGuildId') + '/members/' + this.member.memberId, {
      displayName: this.member.displayName
    })
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(() => {
        this.presentSubAlert('success');
      }, err => {
        this.presentSubAlert('danger');
      });
  }
  removeMember(modal: ClrModal, userId: any, index: any) {
    this.isLoading = true;
    this.http.post(this.baseUrl + this.selectedGuildId + '/members/remove', {userId: userId, reason: this.model.reason,
      notifyUser: this.model.notify, notifyAnonymously: this.model.anonymous, includeReason: this.model.includeReason})
      .pipe(finalize(() => {
        this.isLoading = false;
        modal.close();
      }))
      .subscribe(() => {
        this.members.splice(index, 1);
        this.alerts.push({
          type: 'success',
          message: 'REMOVE_MEMBER_SUCCESS'
        });
      }, err => {
        this.alerts.push({
          type: 'danger',
          message: 'REMOVE_MEMBER_FAIL'
        });
      });
  }
  banMember(modal: ClrModal, userId: any, index: any) {
    this.isLoading = true;
    this.http.post(this.baseUrl + this.selectedGuildId + '/members/ban', {userId: userId, reason: this.model.reason,
      notifyUser: this.model.notify, notifyAnonymously: this.model.anonymous, includeReason: this.model.includeReason})
      .pipe(finalize(() => {
        this.isLoading = false;
        modal.close();
      }))
      .subscribe(() => {
        this.members.splice(index, 1);
        this.alerts.push({
          type: 'success',
          message: 'MEMBER_BAN_SUCCESS'
        });
      }, err => {
        this.alerts.push({
          type: 'danger',
          message: 'MEMBER_BAN_FAIL'
        });
      });
  }
  revokeRoles(userId: any) {
    this.http.post(this.baseUrl + this.selectedGuildId + '/roles/revoke/multiple', {userId: userId,
      discordRoles: this.rolesToDelete})
      .pipe(finalize(() => {
      }))
      .subscribe(() => {
        this.getMemberRoles(userId);
      });
  }
  openModal(modal: ClrModal, member: any) {
    this.member = member;
    modal.open();
  }
  presentSubAlert(type: string) {
    this.subAlertType = type;
    this.showSubAlert = true;
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
  onCloseSubAlert() {
    this.showSubAlert = false;
  }
  rgbToHex(color) {
    // tslint:disable-next-line:no-bitwise
    return '#' + ((1 << 24) + (color.r << 16) + (color.g << 8) + color.b).toString(16).slice(1);
  }
  sortRoles(roles) {
    return _.sortBy(roles, 'position').reverse();
  }
}
