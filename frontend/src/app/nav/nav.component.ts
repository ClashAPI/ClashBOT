import {Component, Inject, OnInit} from '@angular/core';
import {AuthService} from '../_services/auth.service';
import {Router} from '@angular/router';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {TranslateService} from '@ngx-translate/core';
import {DOCUMENT} from '@angular/common';
import {finalize} from 'rxjs/operators';
import {Language} from '../_models/language';
import {ClrLoadingState} from '@clr/angular';
import {Theme} from '../_models/theme';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  baseUrl = environment.apiUrl;
  // TODO: Update when changes (currently changes reflected only when the whole page is reloaded)
  currentLangCode: string;
  currentTheme: string;
  currentLanguage: string;
  isLoading: boolean;
  isPreferencesModalOpen = false;
  guildName: string;
  isDarkMode = true;
  buttonState: ClrLoadingState;

  constructor(public authService: AuthService, private router: Router, private http: HttpClient,
              private translate: TranslateService, @Inject(DOCUMENT) private document) {
  }

  ngOnInit() {
    this.authService.currentGuildName.subscribe(m => this.guildName = m);
    // @ts-ignore
    this.currentLanguage = localStorage.getItem('language');
    // @ts-ignore
    this.currentTheme = localStorage.getItem('theme');
  }
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.router.navigate(['/login']);
  }
  processData(theme: string, languageCode: string) {
    this.buttonState = ClrLoadingState.LOADING;
    this.http.patch(this.baseUrl + 'users/preferences', { language: languageCode, theme })
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
        this.buttonState = ClrLoadingState.SUCCESS;
        if (theme === Theme.LIGHT.toString()) {
          this.document.getElementById('global-theme').setAttribute('href', 'assets/css/clr-ui.min.css');
        } else if (theme === Theme.DARK.toString()) {
          this.document.getElementById('global-theme').setAttribute('href', 'assets/css/clr-ui-dark.min.css');
        }
        this.translate.use(Language[languageCode].toLowerCase());
        localStorage.setItem('language', languageCode);
        localStorage.setItem('theme', theme);
        this.isPreferencesModalOpen = false;
      }, err => {
        this.buttonState = ClrLoadingState.ERROR;
      });
  }
}
