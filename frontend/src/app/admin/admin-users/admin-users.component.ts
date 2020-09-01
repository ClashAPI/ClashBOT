import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {finalize} from 'rxjs/operators';
import {ClrLoadingState, ClrModal} from '@clr/angular';
import {AuthService} from '../../_services/auth.service';

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent implements OnInit {
  baseUrl = environment.apiUrl;
  isLoading: boolean;
  index: number;
  users: any = [];
  user: any = {};
  date = Date;
  alerts = [];
  suspendUntil: string;
  currentDate = new Date().toISOString();
  suspendUserBtnState = ClrLoadingState.DEFAULT;
  enableUserBtnState = ClrLoadingState.DEFAULT;
  deleteUserBtnState = ClrLoadingState.DEFAULT;
  banUserBtnState = ClrLoadingState.DEFAULT;

  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit() {
    this.getUsers();
  }
  getUsers() {
    this.isLoading = true;
    this.http.get(this.baseUrl + 'users')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe((data) => {
        this.users = data;
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_USERS'
          });
        }
      });
  }
  checkSelf(userId: string) {
    return userId === this.authService.currentUser.id;
  }
  banUser(modal: ClrModal, userId: number) {
    this.banUserBtnState = ClrLoadingState.LOADING;
    this.http.post(this.baseUrl + 'users/' + userId + '/ban', null)
      .pipe(finalize(() => {
      }))
      .subscribe(() => {
        this.getUsers();
        this.alerts.push({
          type: 'success',
          message: 'USER_BAN_SUCCESS'
        });
        this.banUserBtnState = ClrLoadingState.SUCCESS;
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
            message: 'COULD_NOT_BAN_USER'
          });
        }
        this.banUserBtnState = ClrLoadingState.ERROR;
      });
  }
  deleteUser(modal: ClrModal, userId: any, index: number) {
    this.deleteUserBtnState = ClrLoadingState.LOADING;
    this.http.delete(this.baseUrl + 'users/' + userId)
      .pipe(finalize(() => {
      }))
      .subscribe(() => {
        this.users.splice(index, 1);
        this.alerts.push({
          type: 'success',
          message: 'USER_DELETE_SUCCESS'
        });
        this.deleteUserBtnState = ClrLoadingState.SUCCESS;
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
            message: 'COULD_NOT_DELETE_USER'
          });
        }
        this.deleteUserBtnState = ClrLoadingState.ERROR;
      });
  }
  suspendUser(modal: ClrModal, userId: any, index: number) {
    this.suspendUserBtnState = ClrLoadingState.LOADING;
    this.http.post(this.baseUrl + 'users/' + userId + '/suspend', { expiresAt: this.suspendUntil })
      .pipe(finalize(() => {
      }))
      .subscribe(data => {
        this.getUsers();
        this.alerts.push({
          type: 'success',
          message: 'USER_SUSPEND_SUCCESS'
        });
        this.suspendUserBtnState = ClrLoadingState.SUCCESS;
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
            message: 'COULD_NOT_SUSPEND_USER'
          });
        }
        this.suspendUserBtnState = ClrLoadingState.ERROR;
      });
  }
  enableUser(modal: ClrModal, userId: any) {
    this.enableUserBtnState = ClrLoadingState.LOADING;
    this.http.post(this.baseUrl + 'users/' + userId + '/enable', null)
      .pipe(finalize(() => {
      }))
      .subscribe(() => {
        this.getUsers();
        this.alerts.push({
          type: 'success',
          message: 'USER_ENABLE_SUCCESS'
        });
        this.enableUserBtnState = ClrLoadingState.SUCCESS;
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
            message: 'COULD_NOT_ENABLE_USER'
          });
        }
        this.enableUserBtnState = ClrLoadingState.ERROR;
      });
  }
  openModal(modal: ClrModal, user: any) {
    this.user = user;
    modal.open();
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
