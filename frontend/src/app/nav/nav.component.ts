import {Component, Input, OnInit} from '@angular/core';
import {AuthService} from '../_services/auth.service';
import {Router} from '@angular/router';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  baseUrl = environment.apiUrl;
  // TODO: Update when changes (currently changes reflected only when the whole page is reloaded)
  currentLangCode: string;
  isLoading: boolean;
  isLangChangeModalOpen = false;
  guildName: string;

  constructor(public authService: AuthService, private router: Router, private http: HttpClient, private translate: TranslateService) {
  }

  ngOnInit() {
    this.authService.currentGuildName.subscribe(m => this.guildName = m);
    this.currentLangCode = this.translate.currentLang;
  }
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.router.navigate(['/login']);
  }
  changeLanguage(languageCode: string) {
    this.translate.use(languageCode);
    this.isLangChangeModalOpen = false;
  }
}
