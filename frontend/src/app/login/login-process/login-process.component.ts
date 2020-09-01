import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {finalize} from 'rxjs/operators';
import {AuthService} from '../../_services/auth.service';

@Component({
  selector: 'app-login-process',
  templateUrl: './login-process.component.html',
  styleUrls: ['./login-process.component.css']
})
export class LoginProcessComponent implements OnInit {
  baseUrl = environment.apiUrl;
  accessToken: string;
  response: any;

  constructor(private router: Router, private route: ActivatedRoute,
              private http: HttpClient, private authService: AuthService) { }

  ngOnInit() {
    this.accessToken = this.route.snapshot.paramMap.get('id');
    this.login();
  }
  // TODO: Notify the user if they got banned or suspended on the login screen
  login() {
    this.http.post(this.baseUrl + 'auth/login/' + this.accessToken, null)
      .subscribe((data) => {
        this.response = data;
        localStorage.setItem('token', this.response.token);
        this.authService.decodedToken = this.authService.jwtHelper.decodeToken(this.response.token);
        localStorage.setItem('user', JSON.stringify(this.response.user));
        this.authService.currentUser = this.authService.jwtHelper.decodeToken(JSON.stringify(this.response.currentUser));
        this.router.navigate(['/dashboard/']).then();
    }, (response) => {
        if (response.reason === 'banned') {
          // TODO: Handle user got banned
        } else if (response.reason === 'suspended') {
          // TODO: Handle user got suspended
        }
        this.router.navigate(['/login/']).then();
      });
  }
}
