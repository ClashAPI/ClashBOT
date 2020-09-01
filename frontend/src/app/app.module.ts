import { BrowserModule } from '@angular/platform-browser';
import {ErrorHandler, Injectable, NgModule} from '@angular/core';

import { AppComponent } from './app.component';
import {ClarityModule, ClrModalModule} from '@clr/angular';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './login/login.component';
import { LoginProcessComponent } from './login/login-process/login-process.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { DashboardComponent } from './dashboard/dashboard.component';
import {NavComponent} from './nav/nav.component';
import { DashboardPluginsComponent } from './dashboard/dashboard-plugins/dashboard-plugins.component';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import { DashboardPluginsModerationComponent } from './dashboard/dashboard-plugins/dashboard-plugins-moderation/dashboard-plugins-moderation.component';
import {JwtModule} from '@auth0/angular-jwt';
import {AuthService} from './_services/auth.service';

import * as Sentry from '@sentry/browser';
import { Integrations as ApmIntegrations } from '@sentry/apm';
import { GuildSelectorComponent } from './guild-selector/guild-selector.component';
import {AdminBotComponent} from './admin/admin-manage-bot/admin-bot.component';
import {AdminComponent} from './admin/admin.component';
import { AdminUsersComponent } from './admin/admin-users/admin-users.component';
import { AdminLogComponent } from './admin/admin-log/admin-log.component';
import { DashboardPluginsCustomCommandsComponent } from './dashboard/dashboard-plugins/dashboard-plugins-custom-commands/dashboard-plugins-custom-commands.component';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {SafePipeModule} from 'safe-pipe';
import {RouterModule} from '@angular/router';
import {routes} from './routes';
import {ServerLogComponent} from './server/server-log/server-log.component';
import {ServerChannelsComponent} from './server/server-channels/server-channels.component';
import {ServerRolesComponent} from './server/server-roles/server-roles.component';
import {ServerMembersComponent} from './server/server-members/server-members.component';
import {ServerGuildComponent} from './server/server-guild/server-guild.component';
import {ServerComponent} from './server/server.component';
import { DashboardPluginsClashapiComponent } from './dashboard/dashboard-plugins/dashboard-plugins-clashapi/dashboard-plugins-clashapi.component';
import { LoginCallbackComponent } from './login/login-callback/login-callback.component';
import {environment} from '../environments/environment.prod';

Sentry.init({
  dsn: environment.sentryDsn,
  integrations: [
    new ApmIntegrations.Tracing(),
    new Sentry.Integrations.TryCatch({
      XMLHttpRequest: false,
    }),
  ],
  tracesSampleRate: 1.0,
});

@Injectable()
export class SentryErrorHandler implements ErrorHandler {
  constructor() {}
  handleError(error) {
    const eventId = Sentry.captureException(error.originalError || error);
    Sentry.showReportDialog({ eventId });
  }
}

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, '../assets/i18n/', '.json');
}

export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    LoginProcessComponent,
    LoginComponent,
    DashboardComponent,
    AdminComponent,
    DashboardPluginsComponent,
    DashboardPluginsModerationComponent,
    GuildSelectorComponent,
    AdminBotComponent,
    AdminUsersComponent,
    AdminLogComponent,
    DashboardPluginsCustomCommandsComponent,
    ServerLogComponent,
    ServerChannelsComponent,
    ServerRolesComponent,
    ServerMembersComponent,
    ServerGuildComponent,
    ServerComponent,
    DashboardPluginsClashapiComponent,
    LoginCallbackComponent,
  ],
  imports: [
    RouterModule.forRoot(routes),
    BrowserModule,
    ClarityModule,
    ClrModalModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: ['localhost:5001'],
        blacklistedRoutes: ['localhost:5001/api/v1/auth'],
      }
    }),
    TranslateModule.forRoot({
      defaultLanguage: 'hu',
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    ReactiveFormsModule,
    SafePipeModule
  ],
  providers: [
    AuthService,
    // { provide: ErrorHandler, useClass: SentryErrorHandler }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
