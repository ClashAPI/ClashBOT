import {Routes} from '@angular/router';
import {LoginComponent} from './login/login.component';
import {LoginProcessComponent} from './login/login-process/login-process.component';
import {AuthGuard} from './_guards/auth.guard';
import {GuildSelectorComponent} from './guild-selector/guild-selector.component';
import {DashboardComponent} from './dashboard/dashboard.component';
import {DashboardPluginsComponent} from './dashboard/dashboard-plugins/dashboard-plugins.component';
import {DashboardPluginsModerationComponent} from './dashboard/dashboard-plugins/dashboard-plugins-moderation/dashboard-plugins-moderation.component';
import {DashboardPluginsCustomCommandsComponent} from './dashboard/dashboard-plugins/dashboard-plugins-custom-commands/dashboard-plugins-custom-commands.component';
import {ServerComponent} from './server/server.component';
import {ServerGuildComponent} from './server/server-guild/server-guild.component';
import {ServerMembersComponent} from './server/server-members/server-members.component';
import {ServerRolesComponent} from './server/server-roles/server-roles.component';
import {ServerChannelsComponent} from './server/server-channels/server-channels.component';
import {ServerLogComponent} from './server/server-log/server-log.component';
import {AdminLogComponent} from './admin/admin-log/admin-log.component';
import {AdminComponent} from './admin/admin.component';
import {AdminBotComponent} from './admin/admin-manage-bot/admin-bot.component';
import {AdminUsersComponent} from './admin/admin-users/admin-users.component';
import {DashboardPluginsClashapiComponent} from './dashboard/dashboard-plugins/dashboard-plugins-clashapi/dashboard-plugins-clashapi.component';
import {LoginCallbackComponent} from './login/login-callback/login-callback.component';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  // { path: 'login/:id', component: LoginProcessComponent },
  { path: 'login/callback', component: LoginCallbackComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'server', redirectTo: '/server/guild', pathMatch: 'full' },
      { path: 'select-guild', component: GuildSelectorComponent },
      { path: 'dashboard', redirectTo: '/dashboard/dashboard', pathMatch: 'full' },
      { path: 'dashboard/dashboard', component: DashboardComponent },
      { path: 'dashboard/plugins', component: DashboardPluginsComponent },
      { path: 'dashboard/plugins/moderation', component: DashboardPluginsModerationComponent },
      { path: 'dashboard/plugins/commands', component: DashboardPluginsCustomCommandsComponent },
      { path: 'dashboard/plugins/clashapi', component: DashboardPluginsClashapiComponent },
      { path: 'server', component: ServerComponent },
      { path: 'server/guild', component: ServerGuildComponent },
      { path: 'server/members', component: ServerMembersComponent },
      { path: 'server/roles', component: ServerRolesComponent },
      { path: 'server/channels', component: ServerChannelsComponent },
      { path: 'server/log', component: ServerLogComponent },
      { path: 'admin', redirectTo: '/admin/bot', pathMatch: 'full' },
      { path: 'admin/log', component: AdminLogComponent },
      { path: 'admin/index', component: AdminComponent },
      { path: 'admin/bot', component: AdminBotComponent },
      { path: 'admin/users', component: AdminUsersComponent },
    ]
  },
  { path: '**', redirectTo: '/dashboard', pathMatch: 'full' },
];
