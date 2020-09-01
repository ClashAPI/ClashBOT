import { Component } from '@angular/core';
import {NavigationStart, Router} from '@angular/router';
import {JwtHelperService} from '@auth0/angular-jwt';
import {AuthService} from './_services/auth.service';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ClashBOT';
  jwtHelper = new JwtHelperService();

  showNav: boolean;

  constructor(private router: Router, private authService: AuthService, translate: TranslateService) {
    translate.setDefaultLang('en');
    translate.use('hu');
    router.events.forEach((event) => {
      if (event instanceof NavigationStart) {
        this.showNav = !event.url.includes('/login');
      }
    }).then();
  }
  // tslint:disable-next-line:use-lifecycle-interface
  ngOnInit() {
    const token = localStorage.getItem('token');
    const user: any = JSON.parse(localStorage.getItem('user'));
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }

    if (user) {
      this.authService.currentUser = user;
    }
  }
  checkGuild() {
    if (this.authService.loggedIn())  {
      if (localStorage.getItem('selectedGuildId') === null || localStorage.getItem('selectedGuild') === undefined) {
        this.router.navigate(['/select-guild']);
      }
    }
  }
}
