import {Component, Inject, OnInit} from '@angular/core';
import {environment} from '../../environments/environment';
import {DOCUMENT} from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: any = {
    type: 'local'
  };
  redirectUrl = 'https://discordapp.com/api/oauth2/authorize?client_id=' + environment.clientId + '&redirect_uri=http%3A%2F%2Flocalhost%3A4200%2Flogin%2Fcallback&response_type=code&scope=identify%20guilds';

  constructor(@Inject(DOCUMENT) private document: Document) { }

  ngOnInit(): void {
  }
  redirectToDiscord() {
    this.document.location.href = this.redirectUrl;
  }
}
