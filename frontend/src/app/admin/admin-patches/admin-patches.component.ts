import {Component, OnInit} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {finalize} from 'rxjs/operators';
import {PatchNote} from '../../_models/patch-note';
import * as DecoupledEditor from '@ckeditor/ckeditor5-build-decoupled-document';
import {ClrLoadingState, ClrModal} from '@clr/angular';

@Component({
  selector: 'app-admin-patches',
  templateUrl: './admin-patches.component.html',
  styleUrls: ['./admin-patches.component.css']
})
export class AdminPatchesComponent implements OnInit {
  baseUrl = environment.apiUrl;
  isLoading: boolean;
  alerts = [];
  patchNotes: PatchNote[] = [];
  isCreateModalOpen = false;
  isEditModalOpen = false;
  // @ts-ignore
  patchNoteModel: PatchNote = {
    title: '',
    content: '',
    commitId: ''
  };
  modalButton = ClrLoadingState.DEFAULT;
  public editor = DecoupledEditor;
  public editorConfig = {
    /*
    plugins: [SimpleUploadAdapter],
    simpleUpload: {
      uploadUrl: 'https://localhost:5001/api/v1/patch-notes/image-upload',
      withCredentials: true,
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`
      }
    }
    */
  };

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getPatchNotes();
  }
  public onReady(editor) {
    editor.ui.getEditableElement().parentElement.insertBefore(
      editor.ui.view.toolbar.element,
      editor.ui.getEditableElement()
    );
  }
  prepareEditPatchNote(index: number) {
    this.patchNoteModel = this.patchNotes[index];
    this.isEditModalOpen = true;
  }
  prepareCreatePatchNote() {
    this.resetPatchNoteModel();
    this.isCreateModalOpen = true;
  }
  resetPatchNoteModel() {
    // @ts-ignore
    this.patchNoteModel = {
      title: '',
      content: '',
      commitId: ''
    };
  }
  editPatchNote(modal: ClrModal) {
    this.modalButton = ClrLoadingState.LOADING;
    this.http.patch(this.baseUrl + 'patch-notes/' + this.patchNoteModel.id, this.patchNoteModel)
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
        // @ts-ignore
        const index = this.patchNotes.findIndex(p => p.id === response.id);
        // @ts-ignore
        this.patchNotes[index] = response;
        this.modalButton = ClrLoadingState.SUCCESS;
        this.alerts.push({
          type: 'success',
          message: 'PATCH_NOTE_UPDATED'
        });
        modal.close();
      }, err => {
        this.modalButton = ClrLoadingState.ERROR;
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_UPDATE_PATCH_NOTE'
          });
        }
      });
  }
  getPatchNotes() {
    this.isLoading = true;
    this.http.get(this.baseUrl + 'patch-notes')
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(response => {
        // @ts-ignore
        this.patchNotes = response;
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_FETCH_PATCH_NOTES'
          });
        }
      });
  }
  addPatchNote(modal: ClrModal) {
    this.modalButton = ClrLoadingState.LOADING;
    this.http.post(this.baseUrl + 'patch-notes', this.patchNoteModel)
      .pipe(finalize(() => {
      }))
      .subscribe(response => {
        this.patchNotes.push(response as PatchNote);
        this.modalButton = ClrLoadingState.SUCCESS;
        this.alerts.push({
          type: 'success',
          message: 'PATCH_NOTE_CREATED'
        });
        modal.close();
      }, err => {
        this.modalButton = ClrLoadingState.ERROR;
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_ADD_PATCH_NOTE'
          });
        }
      });
  }
  deletePatchNote(id: string, index: number) {
    this.isLoading = true;
    this.http.delete(this.baseUrl + 'patch-notes/' + id)
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(response => {
        this.patchNotes.splice(index, 1);
        this.alerts.push({
          type: 'success',
          message: 'PATCH_NOTE_DELETED'
        });
      }, err => {
        if (err.status === 429) {
          this.alerts.push({
            type: 'danger',
            message: 'RATE_LIMIT_EXCEEDED'
          });
        } else {
          this.alerts.push({
            type: 'danger',
            message: 'COULD_NOT_DELETE_PATCH_NOTE'
          });
        }
      });
  }
  onCloseAlert(index: number) {
    this.alerts.splice(index, 1);
  }
}
