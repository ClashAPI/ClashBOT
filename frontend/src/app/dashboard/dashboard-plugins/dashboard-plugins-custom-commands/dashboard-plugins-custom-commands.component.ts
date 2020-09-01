import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import {finalize} from 'rxjs/operators';
import {ClrLoadingState} from '@clr/angular';
import {CustomCommandsPlugin} from '../../../_models/custom-commands-plugin';
import {AdvancedCommand} from '../../../_models/advanced-command';
import {Alert} from '../../../_models/alert';

@Component({
  selector: 'app-dashboard-plugins-custom-commands',
  templateUrl: './dashboard-plugins-custom-commands.component.html',
  styleUrls: ['./dashboard-plugins-custom-commands.component.css'],
})
export class DashboardPluginsCustomCommandsComponent implements OnInit {
  baseUrl: string = environment.apiUrl;
  isLoading: boolean;
  isOpen = false;
  // @ts-ignore
  commandsPlugin: CustomCommandsPlugin = [];
  deletedCommand: any = {};
  isNewCommandModalOpen = false;
  isEditCommandModalOpen = false;
  isNewAdvancedCommandModalOpen = false;
  isEditAdvancedCommandModalOpen = false;
  commandModel: any = {};
  advancedCommandModel: any = {};
  advancedCommands: AdvancedCommand[] = [];
  oldCommand: any = {};
  newCommand: any = {};
  index: number;
  btnState = ClrLoadingState.DEFAULT;
  modalBtnState = ClrLoadingState.DEFAULT;
  alerts: Alert[] = [];
  roles: any = [];
  isRolesInit = false;
  isNewAdvancedCommandLoading = false;
  // @ts-ignore
  deletedAdvancedCommand: AdvancedCommand = {};

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.commandModel.commandCall = '!';
    this.getCustomCommands();
    this.getRoles();
  }
  getRoles() {
    if (!this.isRolesInit) {
      this.isNewAdvancedCommandLoading = true;
      this.http.get(this.baseUrl + localStorage.getItem('selectedGuildId') + '/roles')
        .pipe(finalize(() => {
          this.isNewAdvancedCommandLoading = false;
        }))
        .subscribe((data) => {
          this.roles = data;
          this.isRolesInit = true;
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
  }
  handleAddAdvancedCommand() {
    // @ts-ignore
    this.advancedCommands.push({isEnabled: false, commandCall: null, response: null, description: null, actions: [{type: null}]});
    // console.log(this.advancedCommands);
  }
  handleAddAdvancedCommandAction(index) {
    // @ts-ignore
    this.advancedCommands[index].actions.push({type: null});
  }
  handleDeleteAdvancedCommandAction(commandIndex, actionIndex) {
    // @ts-ignore
    this.advancedCommands[commandIndex].actions.splice(actionIndex, 1);
  }
  getCustomCommands() {
    this.isLoading = true;
    this.http
      .get(
        this.baseUrl +
          localStorage.getItem('selectedGuildId') +
          '/plugins/customcommand'
      )
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe(
        (data) => {
          // @ts-ignore
          this.commandsPlugin = data;
        }, err => {
          if (err.status === 429) {
            this.alerts.push({
              type: 'danger',
              message: 'RATE_LIMIT_EXCEEDED'
            });
          } else {
            this.alerts.push({
              type: 'danger',
              message: 'COULD_NOT_FETCH_CUSTOM_COMMANDS_PLUGIN'
            });
          }
        }
      );
  }
  triggerCustomCommandsPluginState() {
    this.btnState = ClrLoadingState.LOADING;
    this.commandsPlugin.isEnabled = !this.commandsPlugin.isEnabled;
    this.http.patch(this.baseUrl + localStorage.getItem('selectedGuildId') + '/plugins/customcommand',
      {isEnabled: this.commandsPlugin.isEnabled, commands: this.commandsPlugin.commands,
        advancedCommands: this.commandsPlugin.advancedCommands})
      .pipe(finalize(() => {}))
      .subscribe(
        response => {
          this.btnState = ClrLoadingState.SUCCESS;
        },
        err => {
          this.btnState = ClrLoadingState.ERROR;
          if (err.status === 429) {
            this.alerts.push({
              type: 'danger',
              message: 'RATE_LIMIT_EXCEEDED'
            });
          } else {
            this.alerts.push({
              type: 'danger',
              message: 'COULD_NOT_UPDATE_CUSTOM_COMMANDS_PLUGIN'
            });
          }
          this.commandsPlugin.isEnabled = !this.commandsPlugin.isEnabled;
        }
      );
  }
  addCustomCommand() {
    this.modalBtnState = ClrLoadingState.LOADING;
    this.commandModel.prefix = this.commandModel.commandCall[0];
    this.commandModel.commandCall = this.commandModel.commandCall.substr(1);
    this.commandsPlugin.commands.push(this.commandModel);
    this.http.patch(this.baseUrl + localStorage.getItem('selectedGuildId') + '/plugins/customcommand',
      {isEnabled: this.commandsPlugin.isEnabled, commands: this.commandsPlugin.commands,
        advancedCommands: this.commandsPlugin.advancedCommands})
      .pipe(finalize(() => {
        this.commandModel = { commandCall: '!' };
      }))
      .subscribe(response => {
        this.modalBtnState = ClrLoadingState.SUCCESS;
        this.isNewCommandModalOpen = false;
      }, err => {
        this.modalBtnState = ClrLoadingState.ERROR;
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_UPDATE_CUSTOM_COMMANDS_PLUGIN'
          });
        }
        this.commandsPlugin.commands.pop();
      });
  }
  prepareEditAdvancedCommand(i: number) {
    this.oldCommand = {...this.commandsPlugin.advancedCommands[i]};
    this.advancedCommandModel = this.commandsPlugin.advancedCommands[i];
    this.advancedCommandModel.commandCall = this.advancedCommandModel.prefix + this.advancedCommandModel.commandCall;
    // console.log(this.advancedCommandModel);
    this.index = i;
  }
  prepareEditCommand(i: number) {
    this.oldCommand = {...this.commandsPlugin.commands[i]};
    this.newCommand = {...this.commandsPlugin.commands[i]};
    this.newCommand.commandCall = this.newCommand.prefix + this.newCommand.commandCall;
    this.index = i;
  }
  handleEditAdvancedCommand() {
    this.modalBtnState = ClrLoadingState.LOADING;
    this.advancedCommandModel.prefix = this.advancedCommandModel.commandCall[0];
    this.advancedCommandModel.commandCall = this.advancedCommandModel.commandCall.substr(1);
    this.http.patch(this.baseUrl + localStorage.getItem('selectedGuildId') + '/plugins/customcommand',
      {isEnabled: this.commandsPlugin.isEnabled, advancedCommands: this.commandsPlugin.advancedCommands,
        commands: this.commandsPlugin.commands})
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
        this.advancedCommandModel = {};
        this.modalBtnState = ClrLoadingState.SUCCESS;
        this.isEditAdvancedCommandModalOpen = false;
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_UPDATE_CUSTOM_COMMANDS_PLUGIN'
          });
        }
        this.commandsPlugin.commands.push(this.deletedCommand);
      });
  }
  handleEditCommand() {
    this.modalBtnState = ClrLoadingState.LOADING;
    this.commandsPlugin.commands[this.index] = this.newCommand;
    this.newCommand.commandCall = this.newCommand.commandCall.substr(1);
    this.http.patch(this.baseUrl + localStorage.getItem('selectedGuildId') + '/plugins/customcommand',
      {isEnabled: this.commandsPlugin.isEnabled, commands: this.commandsPlugin.commands,
        advancedCommands: this.commandsPlugin.advancedCommands})
      .pipe(finalize(() => {
        this.oldCommand = {};
        this.newCommand = {};
        this.index = null;
      }))
      .subscribe(response => {
        this.modalBtnState = ClrLoadingState.SUCCESS;
        this.isEditCommandModalOpen = false;
      }, err => {
        this.modalBtnState = ClrLoadingState.ERROR;
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_UPDATE_CUSTOM_COMMANDS_PLUGIN'
          });
        }
        this.commandsPlugin.commands[this.index] = this.oldCommand;
      });
  }
  handlePrepareAddCommand(type: string) {
    if (type === 'simple') {
      this.commandModel.commandCall = '!';
      this.commandModel.isEnabled = true;
    } else if (type === 'advanced') {
      this.advancedCommands[this.advancedCommands.length - 1].commandCall = '!';
      this.advancedCommands[this.advancedCommands.length - 1].isEnabled = true;
    }
  }
  handleUndoNewCommand() {
    this.commandModel = {};
    this.commandModel.commandCall = '!';
  }
  handleUndoEditCommand() {
    this.oldCommand = {};
    this.newCommand = {};
    this.index = null;
  }
  handleUndoNewAdvancedCommand() {
    this.advancedCommandModel = {};
  }
  handleRemoveNewAdvancedCommand(index: number) {
    this.advancedCommands.splice(index, 1);
  }
  handleDeleteAdvancedCommandActionEditModal(index: number) {
    this.advancedCommandModel.actions.splice(index, 1);
  }
  handleUndoEditAdvancedCommand() {
    this.commandsPlugin.advancedCommands[this.index] = this.oldCommand;
  }
  addCustomAdvancedCommand() {
    this.modalBtnState = ClrLoadingState.LOADING;
    this.advancedCommands.forEach(c => {
      c.prefix = c.commandCall[0];
      c.commandCall = c.commandCall.substr(1);
    });
    this.http.post(this.baseUrl + localStorage.getItem('selectedGuildId') + '/plugins/customcommand/advancedcommands',
      this.advancedCommands)
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
        this.advancedCommands.forEach(c => {
          this.commandsPlugin.advancedCommands.push(c);
        });
        this.advancedCommands = [];
        this.modalBtnState = ClrLoadingState.SUCCESS;
        this.isNewAdvancedCommandModalOpen = false;
      }, err => {
        this.modalBtnState = ClrLoadingState.ERROR;
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_UPDATE_CUSTOM_COMMANDS_PLUGIN'
          });
        }
        // this.commandsPlugin.commands[this.index] = this.oldCommand;
      });
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
  onDelete(i: number) {
    this.deletedCommand = {...this.commandsPlugin.commands[i]};
    this.commandsPlugin.commands.splice(i, 1);
    this.http.patch(this.baseUrl + localStorage.getItem('selectedGuildId') + '/plugins/customcommand',
      {isEnabled: this.commandsPlugin.isEnabled, advancedCommands: this.commandsPlugin.advancedCommands,
        commands: this.commandsPlugin.commands})
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_UPDATE_CUSTOM_COMMANDS_PLUGIN'
          });
        }
        this.commandsPlugin.commands.push(this.deletedCommand);
      });
  }
  onDeleteAdvancedCommand(i: number) {
    this.deletedAdvancedCommand = {...this.commandsPlugin.advancedCommands[i]};
    this.commandsPlugin.advancedCommands.splice(i, 1);
    this.http.patch(this.baseUrl + localStorage.getItem('selectedGuildId') + '/plugins/customcommand',
      {isEnabled: this.commandsPlugin.isEnabled, advancedCommands: this.commandsPlugin.advancedCommands,
        commands: this.commandsPlugin.commands})
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_UPDATE_CUSTOM_COMMANDS_PLUGIN'
          });
        }
        this.commandsPlugin.advancedCommands.push(this.deletedAdvancedCommand);
      });
  }
  onToggle(i: number, type: string) {
    let command;
    if (type === 'advanced') {
      command = this.commandsPlugin.advancedCommands[i];
      command.isEnabled = !command.isEnabled;
    } else if (type === 'simple') {
      command = this.commandsPlugin.commands[i];
      command.isEnabled = !command.isEnabled;
    }
    this.http.patch(this.baseUrl + localStorage.getItem('selectedGuildId') + '/plugins/customcommand',
      {isEnabled: this.commandsPlugin.isEnabled, commands: this.commandsPlugin.commands,
        advancedCommands: this.commandsPlugin.advancedCommands})
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_UPDATE_CUSTOM_COMMANDS_PLUGIN'
          });
        }
        command.isEnabled = !command.isEnabled;
      });
  }
  eventHandler(event) {
    if (
      event.target.value.length === 1 &&
      (event.code === 'Backspace' || event.code === 'Delete')
    ) {
      return false;
    }

    return true;
  }
}
