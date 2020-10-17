import {Component, Inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {environment} from '../../../environments/environment';
import {finalize} from 'rxjs/operators';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {AuthService} from '../../_services/auth.service';
import {Theme} from '../../_models/theme';
import {TranslateService} from '@ngx-translate/core';
import {DOCUMENT} from '@angular/common';
import {Language} from '../../_models/language';

@Component({
  selector: 'app-login-callback',
  templateUrl: './login-callback.component.html',
  styleUrls: ['./login-callback.component.css']
})
export class LoginCallbackComponent implements OnInit {
  baseUrl = environment.apiUrl;
  code: string;
  apiResponse: any;
  clientId = environment.clientId;
  clientSecret = environment.clientSecret;

  constructor(private route: ActivatedRoute, private http: HttpClient,
              private authService: AuthService, private router: Router,
              private translate: TranslateService, @Inject(DOCUMENT) private document) {
    this.route.queryParams.subscribe(params => {
      this.code = params.code;
      console.log(this.code);
    });
  }

  ngOnInit(): void {
    this.getDiscordUserAccessTokenAsync();
  }
  getDiscordUserAccessTokenAsync() {
    const body = new HttpParams()
      .set('client_id', this.clientId)
      .set('client_secret', this.clientSecret)
      .set('code', this.code)
      .set('redirect_uri', 'http://localhost:4200/login/callback')
      .set('scope', 'identify guilds')
      .set('grant_type', 'authorization_code');
    this.http.post('https://discordapp.com/api/v6/oauth2/token', body.toString(), {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/x-www-form-urlencoded')
    })
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
        // @ts-ignore
        this.http.post(this.baseUrl + 'auth/login', {access_token: response.access_token})
          .pipe(finalize(() => {
          }))
          .subscribe(data => {
            this.apiResponse = data;
            localStorage.setItem('token', this.apiResponse.token);
            this.authService.decodedToken = this.authService.jwtHelper.decodeToken(this.apiResponse.token);
            localStorage.setItem('user', JSON.stringify(this.apiResponse.user));
            this.authService.currentUser = this.authService.jwtHelper.decodeToken(JSON.stringify(this.apiResponse.currentUser));
            localStorage.setItem('language', JSON.stringify(this.apiResponse.preferences.language));
            this.translate.use(Language[localStorage.getItem('language')].toLowerCase());
            localStorage.setItem('theme', JSON.stringify(this.apiResponse.preferences.theme));
            if (localStorage.getItem('theme') === Theme.LIGHT.toString()) {
              this.document.getElementById('global-theme').setAttribute('href', 'assets/css/clr-ui.min.css');
            } else if (localStorage.getItem('theme') === Theme.DARK.toString()) {
              this.document.getElementById('global-theme').setAttribute('href', 'assets/css/clr-ui-dark.min.css');
            }
            this.router.navigate(['/dashboard/']).then();
          }, err => {
            if (err.reason === 'banned') {
              // TODO: Handle user got banned
            } else if (err.reason === 'suspended') {
              // TODO: Handle user got suspended
            }
            this.router.navigate(['/login/']).then();
          });
      }, err => {
        this.router.navigate(['/login/']).then();
      });
  }

}
