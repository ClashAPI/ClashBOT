import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import {finalize} from 'rxjs/operators';
import {ClrLoadingState, ClrModal} from '@clr/angular';
import {FormControl} from '@angular/forms';
import {BadWord} from '../../../_models/bad-word';
import {ModeratorRole} from '../../../_models/moderator-role';
import {Alert} from '../../../_models/alert';
import {AutoModPlugin} from '../../../_models/auto-mod-plugin';
import {DiscordChannel} from '../../../_models/discord-channel';
import {DiscordRole} from '../../../_models/discord-role';
import _ from 'underscore';
import {MultiSelectComboboxModel} from '@clr/angular/forms/combobox/model/multi-select-combobox.model';
import {ComboboxModel} from '@clr/angular/forms/combobox/model/combobox.model';

@Component({
  selector: 'app-dashboard-plugins-moderation',
  templateUrl: './dashboard-plugins-moderation.component.html',
  styleUrls: ['./dashboard-plugins-moderation.component.css']
})
export class DashboardPluginsModerationComponent implements OnInit {
  baseUrl: any = environment.apiUrl;
  roles: any = [];
  rolesForModeratorRoles: any = [];
  rolesForBadWordsSettings: any = [];
  rolesForRepeatedTextSettings: any = [];
  rolesForServerInvitesSettings: any = [];
  rolesForExternalLinksSettings: any = [];
  rolesForExcessiveCapsSettings: any = [];
  rolesForExcessiveEmojisSettings: any = [];
  rolesForExcessiveSpoilersSettings: any = [];
  rolesForExcessiveMentionsSettings: any = [];
  rolesForZalgoSettings: any = [];
  badWordAllowedRoles: any = [];
  badWordIgnoredChannels: any = [];
  repeatedTextIgnoredChannels: any = [];
  repeatedTextAllowedRoles: any = [];
  serverInvitesIgnoredChannels: any = [];
  externalLinksIgnoredChannels: any = [];
  excessiveCapsIgnoredChannels: any = [];
  excessiveEmojisIgnoredChannels: any = [];
  excessiveSpoilersIgnoredChannels: any = [];
  excessiveMentionsIgnoredChannels: any = [];
  zalgoIgnoredChannels: any = [];
  channels: any = [];
  channelsForBadWordsSettings: any = [];
  channelsForRepeatedTextSettings: any = [];
  channelsForServerInvitesSettings: any = [];
  channelsForExternalLinksSettings: any = [];
  channelsForExcessiveCapsSettings: any = [];
  channelsForExcessiveEmojisSettings: any = [];
  channelsForExcessiveSpoilersSettings: any = [];
  channelsForExcessiveMentionsSettings: any = [];
  channelsForZalgoSettings: any = [];
  selectedRole: any;
  blacklistedWords: BadWord[] = [];
  isEnabled: boolean;
  isLoading: boolean;
  isModalOpen = false;
  isRepeatedTextSettingsModalOpen = false;
  isServerInvitesSettingsModalOpen = false;
  isExternalLinksSettingsModalOpen = false;
  isExcessiveCapsSettingsModalOpen = false;
  isExcessiveEmojisSettingsModalOpen = false;
  isExcessiveSpoilersSettingsModalOpen = false;
  isExcessiveMentionsSettingsModalOpen = false;
  isZalgoSettingsModalOpen = false;
  alerts: Alert[] = [];
  modRoles: ModeratorRole[] = [];
  selectedModRoles: any = [];
  // @ts-ignore
  // guild: any = {};
  modCommands: any = [];
  selectedGuildId: string;
  isChannelsLoading = true;
  isRolesLoading = true;
  isProcessingSelectableModRoles = false;
  roleFormControl = new FormControl('');
  badWordsAllowedRolesFormControl = new FormControl('');
  badWordsIgnoredChannelsFormControl = new FormControl('');
  repeatedTextSettingsAllowedRolesFormControl = new FormControl('');
  repeatedTextSettingsIgnoredChannelsFormControl = new FormControl('');
  serverInvitesSettingsAllowedRolesFormControl = new FormControl('');
  serverInvitesSettingsIgnoredChannelsFormControl = new FormControl('');
  externalLinksSettingsAllowedRolesFormControl = new FormControl('');
  externalLinksSettingsIgnoredChannelsFormControl = new FormControl('');
  excessiveCapsSettingsAllowedRolesFormControl = new FormControl('');
  excessiveCapsSettingsIgnoredChannelsFormControl = new FormControl('');
  excessiveEmojisSettingsAllowedRolesFormControl = new FormControl('');
  excessiveEmojisSettingsIgnoredChannelsFormControl = new FormControl('');
  excessiveSpoilersSettingsAllowedRolesFormControl = new FormControl('');
  excessiveSpoilersSettingsIgnoredChannelsFormControl = new FormControl('');
  excessiveMentionsSettingsAllowedRolesFormControl = new FormControl('');
  excessiveMentionsSettingsIgnoredChannelsFormControl = new FormControl('');
  zalgoSettingsAllowedRolesFormControl = new FormControl('');
  zalgoSettingsIgnoredChannelsFormControl = new FormControl('');
  autoModPlugin: AutoModPlugin;
  submitBtnState: ClrLoadingState = ClrLoadingState.DEFAULT;
  triggerBtnState: ClrLoadingState = ClrLoadingState.DEFAULT;
  badWordsSettingsModalBtnState = ClrLoadingState.DEFAULT;
  repeatedTextSettingsModalBtnState = ClrLoadingState.DEFAULT;
  serverInvitesSettingsModalBtnState = ClrLoadingState.DEFAULT;
  externalLinksSettingsModalBtnState = ClrLoadingState.DEFAULT;
  excessiveCapsSettingsModalBtnState = ClrLoadingState.DEFAULT;
  excessiveEmojisSettingsModalBtnState = ClrLoadingState.DEFAULT;
  excessiveSpoilersSettingsModalBtnState = ClrLoadingState.DEFAULT;
  excessiveMentionsSettingsModalBtnState = ClrLoadingState.DEFAULT;
  zalgoSettingsModalBtnState = ClrLoadingState.DEFAULT;

  constructor(private http: HttpClient) {
    this.selectedGuildId = localStorage.getItem('selectedGuildId');
  }

  ngOnInit() {
    this.getModPlugin();
    // this.getChannels();
    // this.getRoles();
    this.badWordsIgnoredChannelsFormControl.valueChanges.subscribe(channel => {
      const index = this.channelsForBadWordsSettings.findIndex(c => c.channelId === channel);
      const channelObject = this.channelsForBadWordsSettings[index];
      this.channelsForBadWordsSettings.splice(index, 1);
      if (channelObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.badWordsSettings.ignoredChannels.push({
          channelId: channelObject.channelId,
          name: channelObject.name
        } as DiscordChannel);
        // @ts-ignore
        this.badWordIgnoredChannels.push({
          channelId: channelObject.channelId,
          name: channelObject.name
        } as DiscordChannel);
      }
    });
    this.badWordsAllowedRolesFormControl.valueChanges.subscribe(role => {
      const index = this.rolesForBadWordsSettings.findIndex(r => r.roleId === role);
      const roleObject = this.rolesForBadWordsSettings[index];
      this.rolesForBadWordsSettings.splice(index, 1);
      if (roleObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.badWordsSettings.allowedRoles.push({
          roleId: roleObject.roleId,
          name: roleObject.name
        } as DiscordRole);
        // @ts-ignore
        this.badWordAllowedRoles.push({
          roleId: roleObject.roleId,
          name: roleObject.name
        } as DiscordRole);
      }
    });
    this.repeatedTextSettingsIgnoredChannelsFormControl.valueChanges.subscribe(channel => {
      const index = this.channelsForRepeatedTextSettings.findIndex(c => c.channelId === channel);
      const channelObject = this.channelsForRepeatedTextSettings[index];
      this.channelsForRepeatedTextSettings.splice(index, 1);
      if (channelObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.repeatedTextSettings.ignoredChannels.push({
          channelId: channelObject.channelId,
          name: channelObject.name
        } as DiscordChannel);
        // @ts-ignore
        this.repeatedTextIgnoredChannels.push({
          channelId: channelObject.channelId,
          name: channelObject.name
        } as DiscordChannel);
      }
    });
    this.repeatedTextSettingsAllowedRolesFormControl.valueChanges.subscribe(role => {
      const index = this.rolesForRepeatedTextSettings.findIndex(r => r.roleId === role);
      const roleObject = this.rolesForRepeatedTextSettings[index];
      this.rolesForRepeatedTextSettings.splice(index, 1);
      if (roleObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.repeatedTextSettings.allowedRoles.push({
          roleId: roleObject.roleId,
          name: roleObject.name
        } as DiscordRole);
        // @ts-ignore
        this.repeatedTextAllowedRoles.push({
          roleId: roleObject.roleId,
          name: roleObject.name
        } as DiscordRole);
      }
    });
    this.serverInvitesSettingsIgnoredChannelsFormControl.valueChanges.subscribe(channel => {
      const index = this.channelsForServerInvitesSettings.findIndex(c => c.channelId === channel);
      const channelObject = this.channelsForServerInvitesSettings[index];
      this.channelsForServerInvitesSettings.splice(index, 1);
      if (channelObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.serverInvitesSettings.ignoredChannels.push({
          channelId: channelObject.channelId,
          name: channelObject.name
        } as DiscordChannel);
      }
    });
    this.serverInvitesSettingsAllowedRolesFormControl.valueChanges.subscribe(role => {
      const index = this.rolesForServerInvitesSettings.findIndex(r => r.roleId === role);
      const roleObject = this.rolesForServerInvitesSettings[index];
      this.rolesForServerInvitesSettings.splice(index, 1);
      if (roleObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.serverInvitesSettings.allowedRoles.push({
          roleId: roleObject.roleId,
          name: roleObject.name
        } as DiscordRole);
      }
    });
    this.externalLinksSettingsIgnoredChannelsFormControl.valueChanges.subscribe(channel => {
      const index = this.channelsForExternalLinksSettings.findIndex(c => c.channelId === channel);
      const channelObject = this.channelsForExternalLinksSettings[index];
      this.channelsForExternalLinksSettings.splice(index, 1);
      if (channelObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.externalLinksSettings.ignoredChannels.push({
          channelId: channelObject.channelId,
          name: channelObject.name
        } as DiscordChannel);
      }
    });
    this.externalLinksSettingsAllowedRolesFormControl.valueChanges.subscribe(role => {
      const index = this.rolesForExternalLinksSettings.findIndex(r => r.roleId === role);
      const roleObject = this.rolesForExternalLinksSettings[index];
      this.rolesForExternalLinksSettings.splice(index, 1);
      if (roleObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.externalLinksSettings.allowedRoles.push({
          roleId: roleObject.roleId,
          name: roleObject.name
        } as DiscordRole);
      }
    });
    this.excessiveCapsSettingsIgnoredChannelsFormControl.valueChanges.subscribe(channel => {
      const index = this.channelsForExcessiveCapsSettings.findIndex(c => c.channelId === channel);
      const channelObject = this.channelsForExcessiveCapsSettings[index];
      this.channelsForExcessiveCapsSettings.splice(index, 1);
      if (channelObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.excessiveCapsSettings.ignoredChannels.push({
          channelId: channelObject.channelId,
          name: channelObject.name
        } as DiscordChannel);
      }
    });
    this.excessiveCapsSettingsAllowedRolesFormControl.valueChanges.subscribe(role => {
      const index = this.rolesForExcessiveCapsSettings.findIndex(r => r.roleId === role);
      const roleObject = this.rolesForExcessiveCapsSettings[index];
      this.rolesForExcessiveCapsSettings.splice(index, 1);
      if (roleObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.excessiveCapsSettings.allowedRoles.push({
          roleId: roleObject.roleId,
          name: roleObject.name
        } as DiscordRole);
      }
    });
    this.excessiveEmojisSettingsIgnoredChannelsFormControl.valueChanges.subscribe(channel => {
      const index = this.channelsForExcessiveEmojisSettings.findIndex(c => c.channelId === channel);
      const channelObject = this.channelsForExcessiveEmojisSettings[index];
      this.channelsForExcessiveEmojisSettings.splice(index, 1);
      if (channelObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.excessiveEmojisSettings.ignoredChannels.push({
          channelId: channelObject.channelId,
          name: channelObject.name
        } as DiscordChannel);
      }
    });
    this.excessiveEmojisSettingsAllowedRolesFormControl.valueChanges.subscribe(role => {
      const index = this.rolesForExcessiveEmojisSettings.findIndex(r => r.roleId === role);
      const roleObject = this.rolesForExcessiveEmojisSettings[index];
      this.rolesForExcessiveEmojisSettings.splice(index, 1);
      if (roleObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.excessiveEmojisSettings.allowedRoles.push({
          roleId: roleObject.roleId,
          name: roleObject.name
        } as DiscordRole);
      }
    });
    this.excessiveSpoilersSettingsIgnoredChannelsFormControl.valueChanges.subscribe(channel => {
      const index = this.channelsForExcessiveSpoilersSettings.findIndex(c => c.channelId === channel);
      const channelObject = this.channelsForExcessiveSpoilersSettings[index];
      this.channelsForExcessiveSpoilersSettings.splice(index, 1);
      if (channelObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.excessiveSpoilersSettings.ignoredChannels.push({
          channelId: channelObject.channelId,
          name: channelObject.name
        } as DiscordChannel);
      }
    });
    this.excessiveSpoilersSettingsAllowedRolesFormControl.valueChanges.subscribe(role => {
      const index = this.rolesForExcessiveSpoilersSettings.findIndex(r => r.roleId === role);
      const roleObject = this.rolesForExcessiveSpoilersSettings[index];
      this.rolesForExcessiveSpoilersSettings.splice(index, 1);
      if (roleObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.excessiveSpoilersSettings.allowedRoles.push({
          roleId: roleObject.roleId,
          name: roleObject.name
        } as DiscordRole);
      }
    });
    this.excessiveMentionsSettingsIgnoredChannelsFormControl.valueChanges.subscribe(channel => {
      const index = this.channelsForExcessiveMentionsSettings.findIndex(c => c.channelId === channel);
      const channelObject = this.channelsForExcessiveMentionsSettings[index];
      this.channelsForExcessiveMentionsSettings.splice(index, 1);
      if (channelObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.excessiveMentionsSettings.ignoredChannels.push({
          channelId: channelObject.channelId,
          name: channelObject.name
        } as DiscordChannel);
      }
    });
    this.excessiveMentionsSettingsAllowedRolesFormControl.valueChanges.subscribe(role => {
      const index = this.rolesForExcessiveMentionsSettings.findIndex(r => r.roleId === role);
      const roleObject = this.rolesForExcessiveMentionsSettings[index];
      this.rolesForExcessiveMentionsSettings.splice(index, 1);
      if (roleObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.excessiveMentionsSettings.allowedRoles.push({
          roleId: roleObject.roleId,
          name: roleObject.name
        } as DiscordRole);
      }
    });
    this.zalgoSettingsIgnoredChannelsFormControl.valueChanges.subscribe(channel => {
      const index = this.channelsForZalgoSettings.findIndex(c => c.channelId === channel);
      const channelObject = this.channelsForZalgoSettings[index];
      this.channelsForZalgoSettings.splice(index, 1);
      if (channelObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.zalgoSettings.ignoredChannels.push({
          channelId: channelObject.channelId,
          name: channelObject.name
        } as DiscordChannel);
      }
    });
    this.zalgoSettingsIgnoredChannelsFormControl.valueChanges.subscribe(role => {
      const index = this.rolesForZalgoSettings.findIndex(r => r.roleId === role);
      const roleObject = this.rolesForZalgoSettings[index];
      this.rolesForZalgoSettings.splice(index, 1);
      if (roleObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.zalgoSettings.allowedRoles.push({
          roleId: roleObject.roleId,
          name: roleObject.name
        } as DiscordRole);
      }
    });
    this.roleFormControl.valueChanges.subscribe(role => {
      const index = this.rolesForModeratorRoles.findIndex(r => r.roleId === role);
      const roleObject = this.rolesForModeratorRoles[index];
      this.rolesForModeratorRoles.splice(index, 1);
      if (roleObject !== undefined) {
        // @ts-ignore
        this.autoModPlugin.moderatorRoles.push({
          roleId: roleObject.roleId,
        } as ModeratorRole);
        this.updateModPlugin();
      }
    });
  }
  handleAddModRole(roleId: any) {
    console.log('triggered');
    if (this.autoModPlugin.moderatorRoles.findIndex(r => r.roleId === roleId)) {
      console.log(roleId);
      this.autoModPlugin.moderatorRoles.push({
        roleId: roleId
      } as ModeratorRole);
      this.rolesForModeratorRoles.splice(this.modRoles.indexOf(roleId), 1);
    }
  }
  populateRoleNames() {
    this.autoModPlugin.moderatorRoles.forEach(dbRole => {
      const object = this.roles.find(role => {
        return role.roleId === dbRole.roleId;
      });
      // @ts-ignore
      this.modRoles.push({
        roleId: object.roleId,
        guildId: this.selectedGuildId,
        name: object.name ?? 'Deleted role'
      } as ModeratorRole);
      // @ts-ignore
      dbRole.name = object.name;
      this.rolesForModeratorRoles.splice(this.roles.indexOf(object), 1);
    });
  }
  getChannels() {
    this.isChannelsLoading = true;
    this.http.get(this.baseUrl + 'guilds/' + this.selectedGuildId + '/channels')
      .pipe(finalize(() => {
        this.isChannelsLoading = false;
      }))
      .subscribe((data) => {
        // @ts-ignore
        this.channels = data;
        this.channelsForBadWordsSettings = [...this.channels];
        this.channelsForRepeatedTextSettings = [...this.channels];
        this.channelsForServerInvitesSettings = [...this.channels];
        this.channelsForExternalLinksSettings = [...this.channels];
        this.channelsForExcessiveCapsSettings = [...this.channels];
        this.channelsForExcessiveEmojisSettings = [...this.channels];
        this.channelsForExcessiveMentionsSettings = [...this.channels];
        this.channelsForExcessiveSpoilersSettings = [...this.channels];
        this.channelsForZalgoSettings = [...this.channels];
        this.autoModPlugin.badWordsSettings.ignoredChannels.forEach(dbChannel => {
          const object = this.channels.find(channel => {
            return channel.channelId === dbChannel.channelId;
          });
          // @ts-ignore
          this.badWordIgnoredChannels.push({
            channelId: object.channelId,
            guildId: this.selectedGuildId,
            name: object.name
          } as DiscordChannel);
          this.channelsForBadWordsSettings.splice(this.channels.indexOf(object), 1);
        });
        this.autoModPlugin.repeatedTextSettings.ignoredChannels.forEach(dbChannel => {
          const object = this.channels.find(channel => {
            return channel.channelId === dbChannel.channelId;
          });
          // @ts-ignore
          this.repeatedTextIgnoredChannels.push({
            channelId: object.channelId,
            guildId: this.selectedGuildId,
            name: object.name
          } as DiscordChannel);
          this.channelsForRepeatedTextSettings.splice(this.channels.indexOf(object), 1);
        });
        this.autoModPlugin.serverInvitesSettings.ignoredChannels.forEach(dbChannel => {
          const object = this.channels.find(channel => {
            return channel.channelId === dbChannel.channelId;
          });
          // @ts-ignore
          this.serverInvitesIgnoredChannels.push({
            channelId: object.channelId,
            guildId: this.selectedGuildId,
            name: object.name
          } as DiscordChannel);
          this.channelsForServerInvitesSettings.splice(this.channels.indexOf(object), 1);
        });
        this.autoModPlugin.externalLinksSettings.ignoredChannels.forEach(dbChannel => {
          const object = this.channels.find(channel => {
            return channel.channelId === dbChannel.channelId;
          });
          // @ts-ignore
          this.externalLinksIgnoredChannels.push({
            channelId: object.channelId,
            guildId: this.selectedGuildId,
            name: object.name
          } as DiscordChannel);
          this.channelsForExternalLinksSettings.splice(this.channels.indexOf(object), 1);
        });
        this.autoModPlugin.excessiveCapsSettings.ignoredChannels.forEach(dbChannel => {
          const object = this.channels.find(channel => {
            return channel.channelId === dbChannel.channelId;
          });
          // @ts-ignore
          this.excessiveCapsIgnoredChannels.push({
            channelId: object.channelId,
            guildId: this.selectedGuildId,
            name: object.name
          } as DiscordChannel);
          this.channelsForExcessiveCapsSettings.splice(this.channels.indexOf(object), 1);
        });
        this.autoModPlugin.excessiveEmojisSettings.ignoredChannels.forEach(dbChannel => {
          const object = this.channels.find(channel => {
            return channel.channelId === dbChannel.channelId;
          });
          // @ts-ignore
          this.excessiveEmojisIgnoredChannels.push({
            channelId: object.channelId,
            guildId: this.selectedGuildId,
            name: object.name
          } as DiscordChannel);
          this.channelsForExcessiveEmojisSettings.splice(this.channels.indexOf(object), 1);
        });
        this.autoModPlugin.excessiveMentionsSettings.ignoredChannels.forEach(dbChannel => {
          const object = this.channels.find(channel => {
            return channel.channelId === dbChannel.channelId;
          });
          // @ts-ignore
          this.excessiveMentionsIgnoredChannels.push({
            channelId: object.channelId,
            guildId: this.selectedGuildId,
            name: object.name
          } as DiscordChannel);
          this.channelsForExcessiveMentionsSettings.splice(this.channels.indexOf(object), 1);
        });
        this.autoModPlugin.excessiveSpoilersSettings.ignoredChannels.forEach(dbChannel => {
          const object = this.channels.find(channel => {
            return channel.channelId === dbChannel.channelId;
          });
          // @ts-ignore
          this.excessiveSpoilersIgnoredChannels.push({
            channelId: object.channelId,
            guildId: this.selectedGuildId,
            name: object.name
          } as DiscordChannel);
          this.channelsForExcessiveSpoilersSettings.splice(this.channels.indexOf(object), 1);
        });
        this.autoModPlugin.zalgoSettings.ignoredChannels.forEach(dbChannel => {
          const object = this.channels.find(channel => {
            return channel.channelId === dbChannel.channelId;
          });
          // @ts-ignore
          this.zalgoIgnoredChannels.push({
            channelId: object.channelId,
            guildId: this.selectedGuildId,
            name: object.name
          } as DiscordChannel);
          this.channelsForZalgoSettings.splice(this.channels.indexOf(object), 1);
        });
        this.getRoles();
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_PLUGINS'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_GUILD'
          });
        }
      });
  }
  getModeratorRoleName(roleId) {
    return this.roles.find(r => r.roleId === roleId).name ?? 'Deleted role';
  }
  handleAddWord(word: any) {
    if (!this.autoModPlugin.badWordsSettings.badWords.includes(word) && word.trim() !== '') {
      // @ts-ignore
      this.autoModPlugin.badWordsSettings.badWords.push({
        word: word
      });
    }
  }
  handleUpdateBadWordsSettings(modal: ClrModal) {
    this.badWordsSettingsModalBtnState = ClrLoadingState.LOADING;
    this.http.patch(this.baseUrl + this.selectedGuildId + '/plugins/mod', this.autoModPlugin)
      .pipe(finalize(() => {
      }))
      .subscribe(() => {
        this.badWordsSettingsModalBtnState = ClrLoadingState.SUCCESS;
        modal.close();
      }, err => {
        this.badWordsSettingsModalBtnState = ClrLoadingState.ERROR;
      });
  }
  handleUpdateRepeatedTextSettings(modal: ClrModal) {
    this.repeatedTextSettingsModalBtnState = ClrLoadingState.LOADING;
    this.http.patch(this.baseUrl + this.selectedGuildId + '/plugins/mod', this.autoModPlugin)
      .pipe(finalize(() => {
      }))
      .subscribe(() => {
        this.repeatedTextSettingsModalBtnState = ClrLoadingState.SUCCESS;
        modal.close();
      }, err => {
        this.repeatedTextSettingsModalBtnState = ClrLoadingState.ERROR;
      });
  }
  handleDeleteWord(word: any) {
    if (this.autoModPlugin.badWordsSettings.badWords.includes(word)) {
      this.autoModPlugin.badWordsSettings.badWords.splice(this.autoModPlugin.badWordsSettings.badWords.indexOf(word), 1);
    }
  }
  handleRemoveRole(role: any) {
    if (this.autoModPlugin.moderatorRoles.filter(r => r.roleId === role.roleId)) {
      this.autoModPlugin.moderatorRoles.splice(this.autoModPlugin.moderatorRoles.indexOf(role), 1);
      const objectIndex = this.modRoles.findIndex(r => r.roleId === role.roleId);
      this.modRoles.splice(objectIndex, 1);
      this.rolesForModeratorRoles.push(role);
      this.updateModPlugin();
    }
  }
  handleRemoveIgnoredChannel(from: any, channel: any) {
    if (from.includes(channel)) {
      from.splice(from.indexOf(channel), 1);
      this.channels.push(channel);
    }
  }
  handleRemoveAllowedRole(from: any, role: any) {
    const index = from.findIndex(r => r.roleId === role.roleId);
    const object = from[index];
    if (object) {
      from.splice(from.indexOf(object), 1);
      this.rolesForBadWordsSettings.push({
        roleId: role.roleId,
        name: role.name
      });
    }
  }
  getRoles() {
    this.isRolesLoading = true;
    this.http.get(this.baseUrl + this.selectedGuildId + '/roles')
      .pipe(finalize(() => {
        this.isRolesLoading = false;
      }))
      .subscribe((data) => {
        this.roles = data;
        this.rolesForModeratorRoles = [...this.roles];
        this.rolesForBadWordsSettings = [...this.roles];
        this.rolesForRepeatedTextSettings = [...this.roles];
        this.rolesForServerInvitesSettings = [...this.roles];
        this.rolesForExternalLinksSettings = [...this.roles];
        this.rolesForExcessiveCapsSettings = [...this.roles];
        this.rolesForExcessiveEmojisSettings = [...this.roles];
        this.rolesForExcessiveSpoilersSettings = [...this.roles];
        this.rolesForExcessiveMentionsSettings = [...this.roles];
        this.rolesForZalgoSettings = [...this.roles];
        this.rolesForModeratorRoles = this.rolesForModeratorRoles.map(role => {
          return _.pick(role, ['roleId']);
        });
        this.autoModPlugin.moderatorRoles.forEach(role => {
          const index = this.rolesForModeratorRoles.findIndex(r => r.roleId === role.roleId);
          this.rolesForModeratorRoles.splice(index, 1);
        });
        this.autoModPlugin.badWordsSettings.allowedRoles.forEach(dbRole => {
          const object = this.roles.find(role => {
            return role.roleId === dbRole.roleId;
          });
          // @ts-ignore
          this.badWordAllowedRoles.push({
            roleId: object.roleId,
            guildId: this.selectedGuildId,
            name: object.name ?? 'Deleted role'
          } as DiscordRole);
          this.rolesForBadWordsSettings.splice(this.roles.indexOf(object), 1);
        });
        this.autoModPlugin.repeatedTextSettings.allowedRoles.forEach(dbRole => {
          const object = this.roles.find(role => {
            return role.roleId === dbRole.roleId;
          });
          // @ts-ignore
          this.repeatedTextAllowedRoles.push({
            roleId: object.roleId,
            guildId: this.selectedGuildId,
            name: object.name ?? 'Deleted role'
          } as DiscordRole);
          this.rolesForRepeatedTextSettings.splice(this.roles.indexOf(object), 1);
        });
        this.populateRoleNames();
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
  getModPlugin() {
    this.isLoading = true;
    this.http.get(this.baseUrl + this.selectedGuildId + '/plugins/mod')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe((data) => {
        // @ts-ignore
        this.autoModPlugin = data;
        // this.populateRoleNames();
        /*
        this.autoModPlugin.moderatorRoles = this.autoModPlugin.moderatorRoles.map(role => {
          return _.pick(role, ['roleId']);
        });
         */
        this.getChannels();
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_MOD_PLUGIN'
          });
        }
      });
  }
  /*
  handleModRoleModelChange(event: ComboboxModel<any>) {
    this.updateModPlugin();
    event.model.forEach(selectedRole => {
      console.log(selectedRole);
      this.rolesForModeratorRoles.splice(this.rolesForModeratorRoles.findIndex(r => r.roleId === selectedRole.roleId), 1);
    });
    this.roles.forEach(role => {
      if (!event.model.findIndex(r => r.roleId === role.roleId)) {
        this.rolesForModeratorRoles.push({
          roleId: role.roleId
        });
      }
    });
    console.log(this.rolesForModeratorRoles);
  }
  getSelectableModRoles(event: any) {
    this.isProcessingSelectableModRoles = true;
    const availableRoles = [];
    this.rolesForModeratorRoles.forEach(role => {
      if (!event.model.findIndex(r => r.roleId === role.roleId)) {
        availableRoles.push(role);
      }
    });
    this.isProcessingSelectableModRoles = false;
    return availableRoles;
  }
   */
  updateModPlugin(isTrigger?: boolean) {
    if (isTrigger) {
      this.triggerBtnState = ClrLoadingState.LOADING;
    }
    this.submitBtnState = ClrLoadingState.LOADING;
    this.autoModPlugin.moderatorRoles = this.autoModPlugin.moderatorRoles.map(role => {
      return _.pick(role, ['roleId']);
    });
    this.http.patch(this.baseUrl + this.selectedGuildId + '/plugins/mod', this.autoModPlugin)
      .pipe(finalize(() => {
        // this.populateRoleNames();
      }))
      .subscribe(() => {
        if (isTrigger) {
          this.triggerBtnState = ClrLoadingState.SUCCESS;
        }
        this.submitBtnState = ClrLoadingState.SUCCESS;
      }, err => {
        if (isTrigger) {
          this.triggerBtnState = ClrLoadingState.ERROR;
        }
        this.submitBtnState = ClrLoadingState.ERROR;
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_UPDATE_MOD_PLUGIN'
          });
        }
      });
  }
  triggerAutoModPluginState() {
    this.autoModPlugin.isEnabled = !this.autoModPlugin.isEnabled;
    this.updateModPlugin(true);
  }
  triggerModCommandState(index: number) {
    this.autoModPlugin.moderatorCommands[index].isEnabled = !this.autoModPlugin.moderatorCommands[index].isEnabled;
    this.http.patch(this.baseUrl + this.selectedGuildId + '/plugins/mod', this.autoModPlugin)
      .pipe(finalize(() => {
      }))
      .subscribe(() => {
      }, err => {
        this.autoModPlugin.moderatorCommands[index].isEnabled = !this.autoModPlugin.moderatorCommands[index].isEnabled;
      });
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
