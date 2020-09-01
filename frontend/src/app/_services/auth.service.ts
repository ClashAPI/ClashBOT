import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {map} from 'rxjs/operators';
import {JwtHelperService} from '@auth0/angular-jwt';
import {Router} from '@angular/router';
import {LocalStorageUser} from '../_models/local-storage-user';
import {BehaviorSubject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: LocalStorageUser;
  guildName = localStorage.getItem('selectedGuildName') || 'NA';
  // @ts-ignore
  private guildNameSource = new BehaviorSubject<string>(this.guildName);
  currentGuildName = this.guildNameSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }
  sendGuildName(guildName: string) {
    this.guildNameSource.next(guildName);
  }
  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
  roleMatch(allowedRoles): boolean {
    let isMatch = false;
    const userRoles = this.decodedToken.role as Array<string>;
    allowedRoles.forEach(element => {
      if (userRoles.includes(element)) {
        isMatch = true;
        return;
      }
    });

    return isMatch;
  }
}
