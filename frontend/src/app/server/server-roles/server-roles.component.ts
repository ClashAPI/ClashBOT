import { Component, OnInit } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {finalize} from 'rxjs/operators';
import {environment} from '../../../environments/environment';
import {ClrModal} from '@clr/angular';
import {Alert} from '../../_models/alert';

@Component({
  selector: 'app-server-roles',
  templateUrl: './server-roles.component.html',
  styleUrls: ['./server-roles.component.css']
})
export class ServerRolesComponent implements OnInit {
  baseUrl = environment.apiUrl;
  index: number;
  roles: any = [];
  role: any = {};
  model: any = {};
  isLoading: boolean;
  selectedGuildId: string;
  alerts: Alert[] = [];

  constructor(private http: HttpClient) {
    this.model.noReason = true;
    this.selectedGuildId = localStorage.getItem('selectedGuildId');
  }

  ngOnInit() {
    this.getRoles();
  }
  getRoles() {
    this.isLoading = true;
    this.http.get(this.baseUrl + this.selectedGuildId + '/roles')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe((data) => {
        this.roles = data;
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_ROLES'
          });
        }
      });
  }
  deleteRole(modal: ClrModal, roleId: any, index: any) {
    console.log(roleId, index);
    this.isLoading = true;
    this.http.delete(this.baseUrl + this.selectedGuildId + '/roles/' + roleId)
      .pipe(finalize(() => {
        this.isLoading = false;
        modal.close();
      }))
      .subscribe(() => {
        this.roles.splice(index, 1);
      }, err => {
      if (err.status === 429) {
        this.alerts.push({
          type: 'danger',
          message: 'RATE_LIMIT_EXCEEDED'
        });
      } else {
        this.alerts.push({
          type: 'danger',
          message: 'COULD_NOT_DELETE_ROLE'
        });
      }
    });
  }
  openModal(modal: ClrModal, role: any) {
    this.role = role;
    modal.open();
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
